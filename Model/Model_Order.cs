using System;
using System.ComponentModel; 

namespace EADS.Model
{
	[Serializable]
	public class Model_Order
    {

        public static byte STATE_NEW = 0;//尚未处理
        public static byte STATE_WAIT_FOR_HANDLE = 1;//已分单 待处理
        public static byte STATE_HANDLED_WAIT_FOR_CALLBACK = 2;//已处理  待回传
        public static byte STATE_CALLBACKING = 3;//正在回传
        public static byte STATE_FINISHED = 4;//完成
        public static byte STATE_CALLBACK_FAILED = 5;//回传失败 

        public static  byte RESULT_CODE_NONE = 99;//结果-尚未处理 

		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		public int ID { get; set; }

		/// <summary>
		/// 订单号 如：2d529fbd68ad44659d66224b5f24fc31
		/// </summary>
		[DisplayName("订单号 如：2d529fbd68ad44659d66224b5f24fc31")]
		public string OrderNO { get; set; }

		/// <summary>
		/// 目的城市
		/// </summary>
		[DisplayName("目的城市")]
		public string DestinationCity { get; set; }

		/// <summary>
		/// 发件人姓名
		/// </summary>
		[DisplayName("发件人姓名")]
		public string SenderName { get; set; }

		/// <summary>
		/// 发件人电话
		/// </summary>
		[DisplayName("发件人电话")]
		public string SenderPhone { get; set; }

		/// <summary>
		/// 发件人地址
		/// </summary>
		[DisplayName("发件人地址")]
		public string SenderAddress { get; set; }

		/// <summary>
		/// 收件人姓名
		/// </summary>
		[DisplayName("收件人姓名")]
		public string RecipientName { get; set; }

		/// <summary>
		/// 收件人电话
		/// </summary>
		[DisplayName("收件人电话")]
		public string RecipientPhone { get; set; }

		/// <summary>
		/// 收件人地址
		/// </summary>
		[DisplayName("收件人地址")]
		public string RecipientAddress { get; set; }

		/// <summary>
		/// 物品
		/// </summary>
		[DisplayName("物品")]
		public string Goods { get; set; }

		/// <summary>
		/// 订单状态：0-新单入库 1-已下发待处理 2-已处理待回传通知 3-回传成功 4-回传失败
		/// </summary>
		[DisplayName("订单状态：0-新单入库 1-已下发待处理 2-已处理待回传通知 3-回传成功 4-回传失败")]
		public byte State { get; set; }

		/// <summary>
		/// 识别结果：99-未处理 0-识别成功 1-识别失败(图片空白) 2-识别失败(其他原因) 3-识别成功（部分模糊）
		/// </summary>
		[DisplayName("识别结果：99-未处理 0-识别成功 1-识别失败(图片空白) 2-识别失败(其他原因) 3-识别成功（部分模糊）")]
		public byte ResultCode { get; set; }

		/// <summary>
		/// 分配处理该订单的用户ID
		/// </summary>
		[DisplayName("分配处理该订单的用户ID")]
		public int DisposeUserId { get; set; }

		/// <summary>
		/// 订单创建时间
		/// </summary>
		[DisplayName("订单创建时间")]
		public DateTime CreateTime { get; set; }

        [DisplayName("订单接单时间")]
        public DateTime? TakeTime { get; set; }

        [DisplayName("订单处理时间")]
        public DateTime? HandleTime { get; set; }

         
        /// <summary>
        /// 订单完成时间
        /// </summary>
        [DisplayName("订单完成时间")]
		public DateTime? FinishTime { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[DisplayName("备注")]
		public string Remark { get; set; }

		/// <summary>
		/// 图片地址
		/// </summary>
		[DisplayName("图片地址")]
		public string ImagePath { get; set; }

        /// <summary>
        /// 回传结果：99-默认值(尚未回传) 1：成功  2：认证失败 3：操作异常 4：数据重复
        /// </summary>
        [DisplayName("回调返回的结果")]
        public byte CallbackResult { get; set; }

    }
}
