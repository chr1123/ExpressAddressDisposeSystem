using System;
using System.ComponentModel; 

namespace EADS.Model
{
	[Serializable]
	public class Model_User
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		public int ID { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[DisplayName("用户名")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码（MD5加密）
		/// </summary>
		[DisplayName("密码（MD5加密）")]
		public string Password { get; set; }

		/// <summary>
		/// 所属组
		/// </summary>
		[DisplayName("所属组")]
		public int GroupID { get; set; }

		/// <summary>
		/// 真实姓名
		/// </summary>
		[DisplayName("真实姓名")]
		public string RealName { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		[DisplayName("手机号")]
		public string Phone { get; set; }

		/// <summary>
		/// 性别： 0-女 1-男
		/// </summary>
		[DisplayName("性别： 0-女 1-男")]
		public byte Sex { get; set; }

		/// <summary>
		/// 年龄
		/// </summary>
		[DisplayName("年龄")]
		public byte Age { get; set; }

		/// <summary>
		/// 生日
		/// </summary>
		[DisplayName("生日")]
		public DateTime? Birthday { get; set; }

		/// <summary>
		/// 联系地址
		/// </summary>
		[DisplayName("联系地址")]
		public string Address { get; set; }

		/// <summary>
		/// 用户创建时间
		/// </summary>
		[DisplayName("用户创建时间")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[DisplayName("备注")]
		public string Remark { get; set; }

	}
}
