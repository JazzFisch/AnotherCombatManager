using System;

namespace AnotherCM.Library.Character {
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
