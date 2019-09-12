namespace SourceDemoParser.Engine
{
    public static class Const
    {
        // const.h
        public const int MAX_OSPATH = 260;
        public const int MAX_EDICT_BITS = 11;
        public const int MAX_PLAYER_NAME_LENGTH = 32;
        public const int MAX_CUSTOM_FILES = 4;
        public const int SIGNED_GUID_LEN = 32;

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
