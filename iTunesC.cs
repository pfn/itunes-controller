using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using iTunesLib;

namespace pfn {
    class M {
        public static void Main(string[] args) {
            if (args.Length == 0) return;
            var itunes = new iTunesApp();
            if (itunes.PlayerState == ITPlayerState.ITPlayerStatePlaying ||
                    args[0] == "playpause") {
                var track = itunes.CurrentTrack;
                switch (args[0]) {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                    var oldrating = track.Rating;
                    var rating = Int32.Parse(args[0]) * 20;
                    if (oldrating != rating) {
                        track.Rating = rating;
                        ShowTrackInfo(track);
                    }
                    break;
                case "info":
                    ShowTrackInfo(track);
                    break;
                case "playpause": itunes.PlayPause(); break;
                case "next":
                    itunes.NextTrack();
                    ShowTrackInfo(itunes.CurrentTrack);
                    break;
                case "prev":
                    itunes.BackTrack();
                    var prevID = itunes.CurrentTrack.TrackDatabaseID;
                    var currentID = track.TrackDatabaseID;
                    if (prevID != currentID)
                        ShowTrackInfo(itunes.CurrentTrack);
                    break;
                }
            }
        }
        static void ShowTrackInfo(IITTrack track) {
            var rating = track.Rating / 20;
            var stars = rating == 1 ? " star" : " stars";
            var text = track.Artist + "\n" + track.Album +
                    "\n\n " + (rating == 0 ? "Rating not set" :
                                    "Rated " + rating + stars);
            ShowNotification(text, track.Name);
        }
        static void ShowNotification(string text, string title) {
            var container = new Container();
            var wait = new ManualResetEvent(false);
            var trayicon = new NotifyIcon(container);
            EventHandler done = null;
            done = delegate {
                wait.Set();
                trayicon.BalloonTipClosed  -= done;
                trayicon.BalloonTipClicked -= done;
            };
            trayicon.BalloonTipClicked += done;
            trayicon.BalloonTipClosed  += done;
            trayicon.BalloonTipTitle = title;
            trayicon.BalloonTipText = text;
            var exe = Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFilesX86) +
                            @"\iTunes\iTunes.exe";
            var icon = Icon.ExtractAssociatedIcon(exe);
            trayicon.Icon = new Icon(icon, 16, 16);

            trayicon.Visible = true;
            trayicon.ShowBalloonTip(5000);
            wait.WaitOne(10000);
            trayicon.Visible = false;
            trayicon.Dispose();
            icon.Dispose();
            container.Dispose();
        }
    }
}
