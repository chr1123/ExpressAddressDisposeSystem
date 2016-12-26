using System;
using System.ComponentModel; 

namespace EADS.Model
{
	[Serializable]
	public class Model_Photo
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		public int ID { get; set; }

        [DisplayName("ZipFileID")]
        public int ZipFileID { get; set; }


        public string WebUrl { get; set; }

        /// <summary>
        /// 图片文件名称
        /// </summary>
        [DisplayName("图片文件名称")]
		public string FileName { get; set; }

		/// <summary>
		/// 图片大小 以kb为单位
		/// </summary>
		[DisplayName("图片大小")]
		public float FileSize { get; set; }

		/// <summary>
		/// 图片创建时间
		/// </summary>
		[DisplayName("图片创建时间")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 图片删除时间
		/// </summary>
		[DisplayName("图片删除时间")]
		public DateTime? DeleteTime { get; set; }

		/// <summary>
		/// 图片文件当前所在路径
		/// </summary>
		[DisplayName("图片文件当前所在路径")]
		public string FilePath { get; set; }

		/// <summary>
		/// 0-未生成订单，1-已生成订单，2-已删除
		/// </summary>
		[DisplayName("0-未生成订单，1-已生成订单，2-已删除")]
		public byte State { get; set; }

	}
}
