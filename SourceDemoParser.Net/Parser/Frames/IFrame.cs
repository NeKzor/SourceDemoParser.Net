using System.Threading.Tasks;

﻿namespace SourceDemoParser.Net
{
	public interface IFrame
	{
		byte[] RawData { get; set; }
		Task ParseData();
		Task<byte[]> ExportData();
		string ToString();
	}
}