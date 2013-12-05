using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Character {
    public class Item {
        public string Name { get; set; }

        public string ArmorCategory { get; set; }

        public string ArmorType { get; set; }

        public string Category { get; set; }

        public int Count { get; set; }

        public int EquippedCount { get; set; }

        public string Flavor { get; set; }

        public string ItemSlot { get; set; }

        public string Text { get; set; }

        public String Type { get; set; }

        public string Weight { get; set; }

        // TODO: magic details

        public override string ToString () {
            return this.Name;
        }
    }
}
