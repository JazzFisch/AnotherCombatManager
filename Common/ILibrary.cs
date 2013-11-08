using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combatant = DnD4e.LibraryHelper.Common.Combatant;
using ExportMonster = DnD4e.LibraryHelper.Monster.Monster;
using ExportCharacter = DnD4e.LibraryHelper.Character.Character;

namespace DnD4e.LibraryHelper.Common {
    public interface ILibrary : IDisposable {
        IQueryable<ExportCharacter> Characters { get; }
        IQueryable<Combatant> Combatants { get; }
        IQueryable<ExportMonster> Monsters { get; }

        bool Exists (Combatant combatant);
        IEnumerable<T> Query<T> () where T : Combatant;
        IEnumerable<T> QueryByName<T> (string name) where T : Combatant;
        IEnumerable<T> QueryByProperty<T> (string property, object value) where T : Combatant;

        void Open (string filename);
        void Remove (Combatant combatant);
        void Upsert (Combatant combatant);
    }
}
