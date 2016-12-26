 
using System;
namespace EADS.Model
{
	/// <summary>
	/// City:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class City
	{
		public City()
		{}
		#region Model
		private int _id;
		private string _cname;
		private int? _cproid=0;
		private int? _cindex=0;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cName
		{
			set{ _cname=value;}
			get{return _cname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? cProID
		{
			set{ _cproid=value;}
			get{return _cproid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? cIndex
		{
			set{ _cindex=value;}
			get{return _cindex;}
		}
		#endregion Model

	}
}

