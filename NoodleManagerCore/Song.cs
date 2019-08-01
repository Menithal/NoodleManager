
using DynamicData;
using DynamicData.Binding;
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
        [Reactive] public string Title { get; set; }
        [Reactive] public string Artist { get; set; }
        [Reactive] public string Mapper { get; set; }
        [Reactive] public string Cover_url { get; set; }
        [Reactive] public string Duration { get; set; }
        [Reactive] public string Bpm { get; set; }
        [Reactive] public IEnumerable<string> Difficulties { get; set; }
        [Reactive] public bool IsSelected { get; set; }


        public string Filename_original { get; set; }
        public string Download_url { get; set; }
        public string Preview_url { get; set; }
        public string Created_at { get; set; }
        public string Updated_at { get; set; }
        public int Version { get; set; }
        public int Id { get; set; }

        public extern bool Enabled { [ObservableAsProperty] get; }

        public extern bool Easy { [ObservableAsProperty] get; }
        public extern bool Normal { [ObservableAsProperty] get; }
        public extern bool Hard { [ObservableAsProperty] get; }
        public extern bool Expert { [ObservableAsProperty] get; }
        public extern bool Master { [ObservableAsProperty] get; }

        public Song()
        {
            this.WhenAny(x => x.Title, x => !string.IsNullOrWhiteSpace(x.GetValue())).ToPropertyEx(this, x => x.Enabled);

            this.WhenAnyValue(x => x.Difficulties).Select(items => items != null && items.Any(i => i == "Easy")).ToPropertyEx(this, x => x.Easy);
            this.WhenAnyValue(x => x.Difficulties).Select(items => items != null && items.Any(i => i == "Normal")).ToPropertyEx(this, x => x.Normal);
            this.WhenAnyValue(x => x.Difficulties).Select(items => items != null && items.Any(i => i == "Hard")).ToPropertyEx(this, x => x.Hard);
            this.WhenAnyValue(x => x.Difficulties).Select(items => items != null && items.Any(i => i == "Expert")).ToPropertyEx(this, x => x.Expert);
            this.WhenAnyValue(x => x.Difficulties).Select(items => items != null && items.Any(i => i == "Master")).ToPropertyEx(this, x => x.Master);
        }
    }
}
