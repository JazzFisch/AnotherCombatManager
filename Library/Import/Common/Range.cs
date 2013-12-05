using System;
using AnotherCM.Library.Common;

namespace AnotherCM.Library.Import.Common {
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
