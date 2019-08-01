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
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace NoodleManager
{
    public class MainViewModel : ReactiveObject
    {
        public const string baseurl = "https://synthriderz.com";
        public const string beatmapsurl = "/api/beatmaps";

        [Reactive] public Song SelectedSong { get; set; }
        public ReactiveCommand<Song, Unit> DeleteCommand { get; set; }
        public ReactiveCommand<Unit, Unit> DeleteSelectedCommand { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

        public MainViewModel()
        {
            DownloadSongs("https://synthriderz.com/api/beatmaps");
            DeleteCommand = ReactiveCommand.Create((Action<Song>)(x => Songs.Remove(x)));
            DeleteSelectedCommand = ReactiveCommand.Create((Action<Unit>)(x => Songs.Where(x=>x.IsSelected).ToList().ForEach(x=>Songs.Remove(x))), this.WhenAnyValue(x => x.SelectedSong, selectedSong => selectedSong != (Song)null));
        }

        private void DownloadSongs(string path)
        {
            string content;

            var fileStream = new FileStream(@"file.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                content = streamReader.ReadToEnd();
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            ObservableCollection<Song> data = JsonSerializer.Deserialize<ObservableCollection<Song>>(content, options);
            Songs = data;
        }
    }
}
