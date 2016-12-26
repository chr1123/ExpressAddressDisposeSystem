
using System;
namespace EADS.Model
{
	/// <summary>
	/// Province:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Province
	{
		public Province()
		{}
		#region Model
		private int _id;
		private string _pname;
		private int? _pindex=0;
		private string _ptype;
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
		public string pName
		{
			set{ _pname=value;}
			get{return _pname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? pIndex
		{
			set{ _pindex=value;}
			get{return _pindex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pType
		{
			set{ _ptype=value;}
			get{return _ptype;}
		}
		#endregion Model

	}
}

