 
using System;
namespace EADS.Model
{
	/// <summary>
	/// Area:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Area
	{
		public Area()
		{}
		#region Model
		private int _id;
		private string _aname;
		private int? _acityid=0;
		private int? _aindex=0;
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
		public string aName
		{
			set{ _aname=value;}
			get{return _aname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? aCityID
		{
			set{ _acityid=value;}
			get{return _acityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? aIndex
		{
			set{ _aindex=value;}
			get{return _aindex;}
		}
		#endregion Model

	}
}

