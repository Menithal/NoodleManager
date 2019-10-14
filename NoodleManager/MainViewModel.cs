using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace NoodleManager
{
    public class MainViewModel : ReactiveObject
    {
        public const string baseurl = "https://synthriderz.com";
        public const string beatmapsurl = "/api/beatmaps";

        [Reactive] public Song SelectedSong { get; set; }
        [Reactive] public int CurrentTabIndex { get; set; }
        public extern bool SongsActive { [ObservableAsProperty] get; }
        public extern bool SettingsActive { [ObservableAsProperty] get; }
        [Reactive] public string Path { get; set; }
        public extern bool PathValid { [ObservableAsProperty] get; }

        public ReactiveCommand<Song, Unit> DeleteCommand { get; set; }
        public ReactiveCommand<Unit, Unit> DeleteSelectedCommand { get; set; }
        public ReactiveCommand<Window, Unit> CloseWindow { get; set; }
        public ReactiveCommand<Unit, Unit> SongsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SettingsCommand { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

        public MainViewModel()
        {
            DownloadSongs(baseurl + beatmapsurl);
            DeleteCommand = ReactiveCommand.Create((Action<Song>)(x => Songs.Remove(x)));
            DeleteSelectedCommand = ReactiveCommand.Create((Action<Unit>)(x => Songs.Where(x => x.IsSelected).ToList().ForEach(x => Songs.Remove(x))), this.WhenAnyValue(x => x.SelectedSong, selectedSong => selectedSong != (Song)null));
            CloseWindow = ReactiveCommand.Create((Action<Window>)(x => x.Close()));
            SongsCommand = ReactiveCommand.Create((Action<Unit>)(x => CurrentTabIndex = 0));
            SettingsCommand = ReactiveCommand.Create((Action<Unit>)(x => CurrentTabIndex = 1));
            this.WhenAny(x => x.Path, x => x!=null && Directory.Exists(x.GetValue())).ToPropertyEx(this, x => x.PathValid);
            this.WhenAny(x => x.CurrentTabIndex, x => x.GetValue() == 0).ToPropertyEx(this, x => x.SongsActive);
            this.WhenAny(x => x.CurrentTabIndex, x => x.GetValue() == 1).ToPropertyEx(this, x => x.SettingsActive);
        }
        

        private void DownloadSongs(string path)
        {
            string content;

            var fileStream = new FileStream(@"file.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                content = streamReader.ReadToEnd();
            }

            ObservableCollection<Song> data = JsonConvert.DeserializeObject<ObservableCollection<Song>>(content);
            Songs = data;
            /*
            using (var client = new WebClient())
            {
                client.DownloadStringCompleted += DownloadCompleteCallback;
                client.DownloadStringAsync(new Uri(path));
            }
            */
        }

        private void DownloadCompleteCallback(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null && e.Result != null && e.Result != "")
            {
                string content = e.Result;

                ObservableCollection<Song> data = JsonConvert.DeserializeObject<ObservableCollection<Song>>(content);
                Songs = data;
            }
        }
    }
}
