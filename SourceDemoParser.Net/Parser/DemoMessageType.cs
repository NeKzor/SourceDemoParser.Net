namespace SourceDemoParser
{
	public enum DemoMessageType
	{
		SignOn = 1,
		Packet,
		SyncTick,
		ConsoleCmd,
		UserCmd,
		DataTables,
		Stop,
		CustomData,     // >= 4
		StringTables    // >= 4
	}
}