using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EADS.Model;
using EADS.BLL;
using EADS.Common;
using System.Threading;

namespace EADS.Server.Handler
{
    public class OrderHandler //该类负责将用户完成的订单回传给服务器
    {

        BLL_Order _bllOrder = new BLL_Order(); 
        Queue<Model_Order> _queueOrder;//待回调的消息队列
        private int _sleepTimeOfSelectDatabase = 30;//查询数据库中待回传订单的间隔时间 单位为s
        private int _sleepTimeOfDoCallbackWhenNoOrder = 30;//回传任务发现队列中无需回传的订单时的休眠时间 单位为s
        private int _onceCallbackOrderMaxNum = 3;//一次回传订单条数
        private bool isRunning;//标记处理服务是否开启
        private Thread _threadFindOrderFromDb;//查询数据库线程
        private int _callbackThreadCount = 1;//回传订单的线程数量
        private Thread[] _threadsCallback; //处理回传的线程集

        private string _callbackServerUrl = "http://116.228.72.129:6688/irds/rcv/reco_rep/addData.do";//回传url地址
        private string _companyCode = "666666";
        private string _secureCode = "b2QwgcG54jsmAdVGaH6nuRTSaJHCcD5j";
        private bool isFirstLoad = false;

        public int OrderHandleNum;//订单识别完的数量 （以查询数据库的线程来累计）
        public int WaitForCallbackOrderNum;//待回传订单数
        public int CallbcakSucceedNum;//回传成功数
        public int CallbackFailedNum;//回传失败数
        private NumChangedListener _numChangedListener;//通知页面刷新数字



        public OrderHandler() {
            _queueOrder = new Queue<Model_Order>();

            IniFile iniFile = new IniFile(CommonConst.GetIniFilePath());
            _callbackServerUrl = iniFile.ReadString("SYSTEM", "CallbackUrl", 
                "http://116.228.72.129:6688/irds/rcv/reco_rep/addData.do");
            _companyCode = iniFile.ReadString("SYSTEM", "CompanyCode", "666666");
            _secureCode = iniFile.ReadString("SYSTEM", "SecureCode", "b2QwgcG54jsmAdVGaH6nuRTSaJHCcD5j");
            _sleepTimeOfSelectDatabase = iniFile.ReadInteger("SYSTEM", "FindWaitForCallbackTimespan", 30);
            _sleepTimeOfDoCallbackWhenNoOrder = iniFile.ReadInteger("SYSTEM", "CallbackThreadSleepTimeIfNoOrder", 30);
            _callbackThreadCount = iniFile.ReadInteger("SYSTEM", "CallbackThreadCount", 3);
            _onceCallbackOrderMaxNum = iniFile.ReadInteger("SYSTEM", "OnceCallbackOrderCount", 3);
              
        }

        public void setNumChangedListener(NumChangedListener listener) {
            _numChangedListener = listener;
        }
        private void refreshFormNum() {
            if (_numChangedListener != null) {
                _numChangedListener.NumberChanged();
            }
        }

        // 开始处理订单回传任务
        public void StartHandle() {
            isFirstLoad = true;
            isRunning = true; 
            _threadFindOrderFromDb = new Thread(FindWaitCallbackOrders);
            _threadFindOrderFromDb.Start();
            _threadsCallback = new Thread[_callbackThreadCount];
            for (int i = 0; i < _callbackThreadCount; i++) {
                _threadsCallback[i]  = new Thread(CallbackOrder);
                _threadsCallback[i].Start(); 
            }
        }

        //停止处理
        public void Stop() { 
            isRunning = false;
            if (_threadFindOrderFromDb != null && _threadFindOrderFromDb.IsAlive) {
                _threadFindOrderFromDb.Abort();
            }
            if (_threadsCallback != null) {
                for (int i = 0; i < _threadsCallback.Length; i++)
                {
                    if (_threadsCallback[i] != null && _threadsCallback[i].IsAlive)
                    {
                        _threadsCallback[i].Abort();
                    }
                }
            } 
            _queueOrder.Clear();
        }

        //读取order表中状态为已识别地址待回调的订单 加入队列
        private void FindWaitCallbackOrders() {
            while (isRunning) { 
                List<Model_Order> list = _bllOrder.GetWaitForCallbackOrderList(isFirstLoad);
                if (isFirstLoad) {
                    isFirstLoad = false;
                }
                if (list != null && list.Count > 0) {
                    lock (_queueOrder)
                    {
                        foreach (Model_Order order in list)
                        {
                            _queueOrder.Enqueue(order);
                        }
                    }
                    OrderHandleNum += list.Count;
                    WaitForCallbackOrderNum = _queueOrder.Count;
                    refreshFormNum();
                } 
                Thread.Sleep(_sleepTimeOfSelectDatabase * 1000);
            } 
        }

        private void CallbackOrder() {
            //延后2s开启 查询数据库操作完成 队列中有数据直接处理
            Thread.Sleep(2000);
            while (isRunning)
            { 
                if (_queueOrder.Count > 0)
                {
                    List<Model_Order> list = new List<Model_Order>();
                    string ids = "";
                    lock (_queueOrder) {
                        while (list.Count < _onceCallbackOrderMaxNum && _queueOrder.Count > 0)
                        {
                            Model_Order order = _queueOrder.Dequeue();
                            list.Add(order);
                            ids += order.ID + ",";
                        }
                    }
                    if (list.Count == 0) continue;
                    //此时应该至少得到了一条数据
                    string postData = XmlHandler.getXmlStrngByList(list, _companyCode,_secureCode);
                    try
                    {
                        string result = HttpHelper.doPostXml(_callbackServerUrl, postData).Trim();
                        //判断结果 更新数据库
                        // <list><item><resultCode>1</resultCode></item></list> 1：成功  2：认证失败 3：操作异常 4：数据重复
                        if (result.Contains("<list><item><resultCode>"))
                        {
                            result = result.Replace("<list><item><resultCode>", "").Replace("</resultCode></item></list>", "");
                            switch (result)
                            {
                                case "1"://成功 
                                case "4"://数据重复
                                    for (int i = 1; i < 4; i++)
                                    {
                                        bool update = _bllOrder.UpdateCallbackResult(ids.TrimEnd(','), byte.Parse(result));
                                        if (update)
                                        {
                                            //回传成功
                                            CallbcakSucceedNum +=list.Count;
                                            refreshFormNum(); 
                                            break;
                                        }
                                        else
                                        {
                                            //更新订单完成状态时失败 尝试3次 仍失败 则记录进Log
                                            Thread.Sleep(5 * 1000);
                                            if (i == 3)
                                                LogHelper.WriteDataLog("订单回调服务器返回成功，但本地数据库状态更新失败，订单ID集合为：" + ids);
                                            CallbcakSucceedNum += list.Count;
                                            refreshFormNum();
                                        }
                                    }
                                    break;
                                case "2"://认证失败
                                    LogHelper.WriteLog("OrderHandler-CallbackOrder回调请求认证失败，订单ID集合：" + ids);
                                    LogHelper.WriteLog("postData:" + postData);
                                    break;
                                case "3"://操作异常
                                    LogHelper.WriteLog("OrderHandler-CallbackOrder回调请求返回为操作异常，订单ID集合：" + ids);
                                    LogHelper.WriteLog("postData:" + postData);
                                    break;
                                default:// 
                                    break;
                            }
                        }
                        else
                        {
                            //请求返回未知的数据格式
                            LogHelper.WriteLog("OrderHandler-CallbackOrder回调请求失败，返回了未知格式的数据，postData："); 
                            LogHelper.WriteLog(postData);
                            LogHelper.WriteLog("result :" + result);
                        }
                    }
                    catch (Exception ex)
                    {
                         
                        CallbackFailedNum += list.Count;
                        //请求异常 可能是网络问题   此时这些订单已从队列中移除 可重新加入队列
                        //if (list.Count > 0) {
                        //    lock (_queueOrder) {
                        //        foreach (Model_Order model in list)
                        //        {
                        //            _queueOrder.Enqueue(model);
                        //        }
                        //    }  
                        //}
                        //更新数据库  将这些订单状态还原为2-已处理 待回传
                        bool update = _bllOrder.UpdateStateAsWaitForCallback(ids.TrimEnd(','));
                        if (update) {
                            OrderHandleNum = OrderHandleNum - list.Count;
                        }
                        // LogHelper.WriteLog("OrderHandler-CallbackOrder回调请求异常：" + ex.Message);
                        //  LogHelper.WriteLog("postData:" + postData);
                    }
                }
                else
                {
                    //队列中已无数据 则休眠指定时间
                    Thread.Sleep(_sleepTimeOfDoCallbackWhenNoOrder * 1000);
                }
                //刷新待完成订单数
                WaitForCallbackOrderNum = _queueOrder.Count;
                refreshFormNum();
            }
        }

     
          
//        public string callbackOrder()
//        {
//-
//           // BLL_Order bll = new BLL_Order();
//            //int count = 0;
//            //int unhand = 0;
//            //  Model_Order order = bll.getUserFirstUnHandlerOrder(1,out count,out unhand);
//            Model_Order order = new Model_Order();
//            order.OrderNO = "911475a7c141416a95212c53c9968ee3";
//            order.DestinationCity = "武汉";//武汉
//            order.RecipientAddress = "湖北省武汉市洪山区光谷软件园";//湖北省武汉市洪山区光谷软件园E2栋
//            List<Model_Order> orderList = new List<Model_Order>();
//            orderList.Add(order);
//            string responseData = XmlHandler.getXmlStrngByList(orderList);
//            string url = "http://116.228.72.129:6688/irds/rcv/reco_rep/addData.do";
//            string result = HttpHelper.doPostXml(url,responseData);

//            LogHelper.WriteLog("XML===========RESPONSE:");
//            LogHelper.WriteLog(result);
//            return result;
//        }

    }
}
