 
using System;
using System.Data;
using System.Collections.Generic;
using EADS.Common;
using EADS.Model;
namespace EADS.BLL
{
	/// <summary>
	/// City
	/// </summary>
	public  class BLL_Test
	{
		private   EADS.DAL.DAL_Bases dal=new EADS.DAL.DAL_Bases();

        public   bool IsDabaseConnectedOk() {
            bool result = dal.OpenConnect();
            if (result) {
                dal.CloseConnect();
            }
            return result;
        }
    }
}

