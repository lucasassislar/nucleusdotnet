using Nucleus.Gaming.Package;
using Nucleus.Gaming.Windows.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Nucleus.Gaming.Coop {
    /// <summary>
    /// Manager that extracts information from handler metadata's 
    /// and stores the game's metadata
    /// This exists because Nucleus Coop does not know the name of the games, this data comes from game packages
    /// </summary>
    public class GameMetadataManager {
#if WINFORMS
        private Dictionary<string, List<Action<Bitmap>>> callbacks;
        public Dictionary<string, Bitmap> GameIcons { get; private set; }
#endif
        public Dictionary<string, string> GameNames { get; private set; }

        public GameMetadataManager() {
            GameNames = new Dictionary<string, string>();
#if WINFORMS
            GameIcons = new Dictionary<string, Bitmap>();
            callbacks = new Dictionary<string, List<Action<Bitmap>>>();
#endif
        }

        private void ThreadGetIcon(object state) {
            UserGameInfo game = (UserGameInfo)state;
#if WINFORMS
            Icon icon = Shell32Interop.GetIcon(game.ExePath, false);

            Bitmap bmp = icon.ToBitmap();
            icon.Dispose();
            game.Icon = bmp;

            lock (callbacks) {
                List<Action<Bitmap>> calls;
                if (callbacks.TryGetValue(game.GameID, out calls)) {
                    for (int i = 0; i < calls.Count; i++) {
                        calls[i](bmp);
                    }
                    callbacks.Remove(game.GameID);
                }

                GameIcons.Add(game.GameID, bmp);
            }
#endif
        }

#if WINFORMS
        public void GetIcon(UserGameInfo game, Action<Bitmap> callback) {
            Bitmap icon;
            if (GameIcons.TryGetValue(game.GameID, out icon)) {
                callback(icon);
            } else {
                // extract icon from exe file
                lock (callbacks) {
                    List<Action<Bitmap>> calls;
                    if (!callbacks.TryGetValue(game.GameID, out calls)) {
                        calls = new List<Action<Bitmap>>();
                        callbacks.Add(game.GameID, calls);

                        ThreadPool.QueueUserWorkItem(ThreadGetIcon, game);
                    }
                    calls.Add(callback);
                }

            }
    }
#endif

        public bool UpdateNaming(GameHandlerMetadata info) {
            // TODO: better logic so repositories can agree on game name
            if (GameNames.ContainsKey(info.GameID)) {
                return false;
            }
            GameNames.Add(info.GameID, info.GameTitle);
            return true;
        }

        public string GetGameName(string gameId) {
            string gameName;
            if (GameNames.TryGetValue(gameId, out gameName)) {
                return gameName;
            }
            return "Unknown";
        }
    }
}
