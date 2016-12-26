using System;
using System.ComponentModel; 

namespace EADS.Model
{
	[Serializable]
	public class Model_Order_Operate_Log
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		public int ID { get; set; }

		/// <summary>
		/// 订单ID
		/// </summary>
		[DisplayName("订单ID")]
		public int OrderID { get; set; }

		/// <summary>
		/// 操作人 0-系统  其他为用户
		/// </summary>
		[DisplayName("操作人 0-系统  其他为用户")]
		public int UserID { get; set; }

		/// <summary>
		/// 操作类型：0-未定义 1-分配定单 2-识别完订单 3-回传订单 4-回传成功(订单完成)
		/// </summary>
		[DisplayName("操作类型：0-未定义 1-分配定单 2-识别完订单 3-回传订单 4-回传成功(订单完成)")]
		public byte OperateType { get; set; }

		/// <summary>
		/// 操作描述
		/// </summary>
		[DisplayName("操作描述")]
		public string Description { get; set; }

		/// <summary>
		/// 操作时间
		/// </summary>
		[DisplayName("操作时间")]
		public DateTime OperateTime { get; set; }

	}
}
