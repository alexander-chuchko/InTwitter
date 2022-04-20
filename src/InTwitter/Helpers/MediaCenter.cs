using System.Collections.Generic;
using LibVLCSharp.Shared;

namespace InTwitter.Helpers
{
    public static class MediaCenter
    {
        private static LibVLC _lib;

        private static List<MediaPlayer> _players;

        static MediaCenter()
        {
            _players = new List<MediaPlayer>();
            Core.Initialize();
        }

        public static LibVLC Library => _lib ??= new LibVLC();

        public static MediaPlayer GetPlayer(Media media)
        {
            var player = new MediaPlayer(media) { EnableHardwareDecoding = true };

            _players.Add(player);

            return player;
        }

        public static void StopAllPlayers()
        {
            foreach (var player in _players)
            {
                player.Stop();
            }

            _players.Clear();
        }
    }
}