using System;
using System.ComponentModel.Composition;
using AnotherCM.Library.Character;
using AnotherCM.Library.Encounter;
using AnotherCM.Library.Monster;
using Caliburn.Micro;

namespace AnotherCM.WPF.Framework {
    [Export(typeof(ILibrary))]
    public class Library : ILibrary {
        private AnotherCM.Library.Common.Library library;

        public Library () {
            this.Characters = new BindableCollection<Character>();
            this.Monsters = new BindableCollection<Monster>();
            this.Encounters = new BindableCollection<Encounter>();

            AnotherCM.Library.Common.Library.OpenLibraryAsync().ContinueWith(task => {
                this.library = task.Result;

                this.Characters.AddRange(this.library.Characters);
                this.Monsters.AddRange(this.library.Monsters);
                this.Encounters.AddRange(this.library.Encounters);
            });
        }

        public BindableCollection<Character> Characters { get; private set; }

        public BindableCollection<Monster> Monsters { get; private set; }

        public BindableCollection<Encounter> Encounters { get; private set; }
    }
}