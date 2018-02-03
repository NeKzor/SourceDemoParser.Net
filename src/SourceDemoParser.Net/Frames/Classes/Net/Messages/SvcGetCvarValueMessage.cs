namespace SourceDemoParser
{
	public class SvcGetCvarValueMessage : INetMessage
	{
		public int Cookie { get; set; }
		public string CvarName { get; set; }
	}
}