using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportCharacter = DnD4e.LibraryHelper.Character.Character;
using ImportCharacter = DnD4e.LibraryHelper.Import.Character.Character;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class CharacterMethods {
        public static ExportCharacter ToCharacter (this ImportCharacter import) {
            throw new NotImplementedException();
        }
    }
}
