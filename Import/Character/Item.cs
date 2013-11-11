using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Item {
        [XmlElement("RulesElement")]
        public Rules Rules { get; set; }

        public override string ToString () {
            if (this.Rules.Count == 1) {
                return this.Rules[0].Name;
            }
            else if (this.Rules.Count == 2) {
                if (this.Rules[1].Type.Equals("Magic Item", StringComparison.Ordinal)) {
                    var magic = this.Rules[1].Name;
                    var item = this.Rules[0].Name;
                    return magic.Replace("Weapon", item).Replace("Armor", item);
                }
                else {
                    return this.Rules[1].Name;
                }
            }
            else {
                return "Unknown item!";
            }
        }
    }
}
