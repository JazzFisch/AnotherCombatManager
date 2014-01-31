using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnotherCM.Library.Character;
using AnotherCM.Library.Encounter;
using AnotherCM.Library.Monster;
using Caliburn.Micro;

namespace AnotherCM.WPF.Framework {
    public interface ILibrary {
        BindableCollection<Character> Characters { get; }
        BindableCollection<Monster> Monsters { get; }
        BindableCollection<Encounter> Encounters { get; }
    }
}
