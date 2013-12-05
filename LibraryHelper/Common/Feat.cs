using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Character {
    public class ClassFeat : Feat {
        public int Level { get; set; }
    }

    public class Feat {
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Text { get; set; }

        public override string ToString () {
            return this.Name;
        }
    }
}
