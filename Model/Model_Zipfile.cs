using System;
using System.ComponentModel; 

namespace EADS.Model
{
	[Serializable]
	public class Model_Zipfile
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		public int ID { get; set; }

		/// <summary>
		/// 压缩包名称
		/// </summary>
		[DisplayName("压缩包名称")]
		public string ZipName { get; set; }

		/// <summary>
		/// 压缩包大小
		/// </summary>
		[DisplayName("压缩包大小")]
		public float FileSize { get; set; }

		/// <summary>
		/// 是否已解压：0-未解压 1-已解压
		/// </summary>
		[DisplayName("是否已解压：0-未解压 1-已解压")]
		public byte Unziped { get; set; }

		/// <summary>
		/// 包含文件数量 解压时获取得到
		/// </summary>
		[DisplayName("包含文件数量 解压时获取得到")]
		public short FileCount { get; set; }

		/// <summary>
		/// 压缩包创建时间
		/// </summary>
		[DisplayName("压缩包创建时间")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 压缩包解压时间
		/// </summary>
		[DisplayName("压缩包解压时间")]
		public DateTime? UnzipTime { get; set; }

		/// <summary>
		/// 压缩包删除时间
		/// </summary>
		[DisplayName("压缩包删除时间")]
		public string DeleteTime { get; set; }

		/// <summary>
		/// 压缩包文件当前所在路径
		/// </summary>
		[DisplayName("压缩包文件当前所在路径")]
		public string FilePath { get; set; }

		/// <summary>
		/// 0-未解压，1-已解压，2-已删除
		/// </summary>
		[DisplayName("0-未解压，1-已解压，2-已删除")]
		public byte State { get; set; }

	}
}
