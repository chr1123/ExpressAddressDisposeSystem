using System;
using System.ComponentModel; 

namespace EADS.Model
{
	[Serializable]
	public class Model_User_Group
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		public int ID { get; set; }

		/// <summary>
		/// 用户组名称
		/// </summary>
		[DisplayName("用户组名称")]
		public string GroupName { get; set; }

		/// <summary>
		/// 负责人
		/// </summary>
		[DisplayName("负责人")]
		public string TheHead { get; set; }

		/// <summary>
		/// 联系电话
		/// </summary>
		[DisplayName("联系电话")]
		public string Phone { get; set; }

		/// <summary>
		/// 组所在省
		/// </summary>
		[DisplayName("组所在省")]
		public int? ProID { get; set; }

		/// <summary>
		/// 组所在城市
		/// </summary>
		[DisplayName("组所在城市")]
		public int? CityID { get; set; }

		/// <summary>
		/// 组所在区
		/// </summary>
		[DisplayName("组所在区")]
		public int? AreaID { get; set; }

		/// <summary>
		/// 组详细地址
		/// </summary>
		[DisplayName("组详细地址")]
		public string Address { get; set; }

		/// <summary>
		/// 组创建时间
		/// </summary>
		[DisplayName("组创建时间")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[DisplayName("备注")]
		public string Remark { get; set; }

	}
}
