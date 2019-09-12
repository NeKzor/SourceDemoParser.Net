using System;

namespace SourceDemoParser
{
    public class SourceGame
    {
        public static readonly SourceGame Default = new SourceGame();

        public int? MaxSplitscreenClients { get; private set; }
        public Type[] DefaultMessages { get; private set; }
        public Type[] DefaultNetMessages { get; private set; }

        public SourceGame()
        {
            DefaultMessages = DemoMessages.Default;
            DefaultNetMessages = NetMessages.Default;
        }

        public SourceGame WithMaxSplitscreenClients(int? maxSplitscreenClients)
        {
            MaxSplitscreenClients = maxSplitscreenClients;
            return this;
        }
        public SourceGame WithDefaultDemoMessages(Type[] defaultMessages)
        {
            DefaultMessages = defaultMessages;
            return this;
        }
        public SourceGame WithDefaultNetMessages(Type[] defaultNetMessages)
        {
            DefaultNetMessages = defaultNetMessages;
            return this;
        }
    }
}
