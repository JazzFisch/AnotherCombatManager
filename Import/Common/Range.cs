using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Import.Common {
    public class Range {
        public static readonly Range Invalid = new Range();

        public AttackType AttackType { get; set; }

        public float Distance { get; set; }

        public float MaxDistance { get; set; }

        public float Size { get; set; }

        public override string ToString () {
            return String.Format("{0} {1}", AttackType, (int)Math.Round(Distance));
        }
    }
}
