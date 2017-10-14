namespace SourceDemoParser.Extensions
{
	public static class Const
    {
		// const.h
		public const int MAX_OSPATH = 260;					//
		public const int MAX_EDICT_BITS = 11;               // How many bits to use to encode an edict. Number of bits needed to represent max edicts
		public const int MAX_PLAYER_NAME_LENGTH = 32;		// A player name may have 31 chars + 0 on the PC. The 360 only allows 15 char + 0, but stick with the larger PC size for cross-platform communication
		public const int MAX_CUSTOM_FILES = 4;              // A client can have up to 4 customization files (logo, sounds, models, txt)
		public const int SIGNED_GUID_LEN = 32;              // Hashed CD Key (32 hex alphabetic chars + 0 terminator)

		public const string INSTANCE_BASELINE_TABLENAME = "instancebaseline";
		public const string LIGHT_STYLES_TABLENAME = "lightstyles";
		public const string USER_INFO_TABLENAME = "userinfo";
		public const string SERVER_STARTUP_DATA_TABLENAME = "server_query_info";

		// DownloadListGenerator.h
		public const string DOWNLOADABLE_FILE_TABLENAME = "downloadables";

		// precache.h
		public const string MODEL_PRECACHE_TABLENAME = "modelprecache";
		public const string GENERIC_PRECACHE_TABLENAME = "genericprecache";
		public const string SOUND_PRECACHE_TABLENAME = "soundprecache";
		public const string DECAL_PRECACHE_TABLENAME = "decalprecache";
	}
}