using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using ImportCharacter = DnD4e.LibraryHelper.Import.Character.Character;

namespace DnD4e.LibraryHelper.Character {
    public class Character : Combatant {

        public static bool TryCreateFromFile (string filename, out Character character) {
            character = null;
            try {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
                    using (var xml = new XmlTextReader(fs)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(ImportCharacter));
                        if (serializer.CanDeserialize(xml)) {
                            var import = serializer.Deserialize(xml) as ImportCharacter;
                            character = import.ToCharacter();
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
            }

            return false;
        }
    }
}
