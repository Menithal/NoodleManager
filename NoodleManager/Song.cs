
using DynamicData;
using DynamicData.Binding;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace NoodleManager
{
    public class Song : ReactiveObject
    {
        [JsonProperty(PropertyName = "title")]
        [Reactive] public string Title { get; set; }

        [JsonProperty(PropertyName = "artist")]
        [Reactive] public string Artist { get; set; }

        [JsonProperty(PropertyName = "mapper")]
        [Reactive] public string Mapper { get; set; }

        [JsonProperty(PropertyName = "cover_url")]
        [Reactive] public string Cover_url { get; set; }

        [JsonProperty(PropertyName = "duration")]
        [Reactive] public string Duration { get; set; }

        [JsonProperty(PropertyName = "bmp")]
        [Reactive] public string Bpm { get; set; }

        [JsonProperty(PropertyName = "difficulties")]
        public ObservableCollection<string> Difficulties { get; set; }

        [Reactive] public bool IsSelected { get; set; }

        [Reactive] public bool IsDownloaded { get; set; }


        [JsonProperty(PropertyName = "filename_original")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "download_url")]
        public string Download_url { get; set; }

        [JsonProperty(PropertyName = "preview_url")]
        public string Preview_url { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public string Created { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string Updated { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        public extern bool Enabled { [ObservableAsProperty] get; }

        public extern bool Easy { [ObservableAsProperty] get; }
        public extern bool Normal { [ObservableAsProperty] get; }
        public extern bool Hard { [ObservableAsProperty] get; }
        public extern bool Expert { [ObservableAsProperty] get; }
        public extern bool Master { [ObservableAsProperty] get; }

        public int count = 0;

        public Song()
        {
            Setup();
        }

        public void Setup()
        {
            //this.WhenAny(x => x.Title, x => !string.IsNullOrWhiteSpace(x.GetValue())).ToPropertyEx(this, x => x.Enabled);

            Difficulties = new ObservableCollection<string>();
            Difficulties.ToObservableChangeSet(x => x).ToCollection().Select(items => items.Any(i => i == "Easy")).ToPropertyEx(this, x => x.Easy);
            Difficulties.ToObservableChangeSet(x => x).ToCollection().Select(items => items.Any(i => i == "Normal")).ToPropertyEx(this, x => x.Normal);
            Difficulties.ToObservableChangeSet(x => x).ToCollection().Select(items => items.Any(i => i == "Hard")).ToPropertyEx(this, x => x.Hard);
            Difficulties.ToObservableChangeSet(x => x).ToCollection().Select(items => items.Any(i => i == "Expert")).ToPropertyEx(this, x => x.Expert);
            Difficulties.ToObservableChangeSet(x => x).ToCollection().Select(items => items.Any(i => i == "Master")).ToPropertyEx(this, x => x.Master);
        }
    }
}
