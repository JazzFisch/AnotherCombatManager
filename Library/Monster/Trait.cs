using System;
using System.Collections.Generic;

namespace AnotherCM.Library.Monster {
    public class Trait {
        public string Name { get; set; }

        public string Details { get; set; }

        public List<string> Keywords { get; set; }

        public int Range { get; set; }

        public override string ToString () {
            return this.Name;
        }
    }
}
