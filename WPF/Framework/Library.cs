using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using AnotherCM.Library.Character;
using AnotherCM.Library.Encounter;
using AnotherCM.Library.Monster;
using Caliburn.Micro;

namespace AnotherCM.WPF.Framework {
    [Export(typeof(ILibrary))]
    public class Library : ILibrary {
        private AnotherCM.Library.Common.Library library;
        private ILog log;

        public Library () {
            // TODO: make Library use BindableCollection?
            this.Characters = new BindableCollection<Character>();
            this.Monsters = new BindableCollection<Monster>();
            this.Encounters = new BindableCollection<Encounter>();
            this.log = LogManager.GetLog(typeof(Library));

            AnotherCM.Library.Common.Library.OpenLibraryAsync().ContinueWith(task => {
                this.library = task.Result;

                this.Characters.AddRange(this.library.Characters);
                this.Monsters.AddRange(this.library.Monsters);
                this.Encounters.AddRange(this.library.Encounters);

                this.Characters.CollectionChanged += OnCollectionChanged;
                this.Monsters.CollectionChanged += OnCollectionChanged;
                this.Encounters.CollectionChanged += OnCollectionChanged;
            });
        }

        public string FilePath {
            get {
                if (this.library != null) {
                    return this.library.FileName;
                }
                return null;
            }
            set {
                if (this.library != null) {
                    this.library.SaveAs(value);
                }
            }
        }

        public BindableCollection<Character> Characters { get; private set; }

        public BindableCollection<Monster> Monsters { get; private set; }

        public BindableCollection<Encounter> Encounters { get; private set; }

        public async Task AddCharactersAsync (params string[] paths) {
            await this.library.ImportCharactersFromFileAsync(paths);
            this.Characters.IsNotifying = false;
            this.Characters.Clear();
            this.Characters.IsNotifying = true;
            this.Characters.AddRange(this.library.Characters);
        }

        public async Task AddMonstersAsync (params string[] paths) {
            await this.library.ImportMonstersFromFileAsync(paths);
            this.Monsters.IsNotifying = false;
            this.Monsters.Clear();
            this.Monsters.IsNotifying = true;
            this.Monsters.AddRange(this.library.Monsters);
        }

        public void Flush () {
            this.library.Flush();
        }

        private void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action != NotifyCollectionChangedAction.Remove) {
                return;
            }

            if (sender is BindableCollection<Monster>) {
                this.library.Monsters.RemoveAt(e.OldStartingIndex);
            }
            else if (sender is BindableCollection<Character>) {
                this.library.Characters.RemoveAt(e.OldStartingIndex);
            }
            else if (sender is BindableCollection<Encounter>) {
                this.library.Encounters.RemoveAt(e.OldStartingIndex);
            }
        }
    }
}