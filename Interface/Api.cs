using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Interface.Common;

namespace Interface
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Api
    {
       
        [WebInvoke(Method = "POST", UriTemplate = "test", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public JsonResult<string> test(string sign)
        {
            JsonResult<string> result = new JsonResult<string>() ;
            try
            {
                result.data = sign;
                return result;
            }
            catch (Exception)
            {
                result.data = "aaaaaaaaaaaaaaaaaaaaaaaaaa";
                result.ret = 11;
                result.msg = "sadsfsdaf";
                return result;
            }
        }
    }
}
