using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Common {
    // simple wrapper to get XML naming right
    internal class AbilityScoreNumber : SimpleValue { }

    internal class CalculatedNumber : SimpleValue { }

    internal class DamageConstant : SimpleValue { }

    internal class MonsterSavingThrow : SimpleValue { }

    internal class SimpleAdjustableNumber : SimpleValue { }

    internal class SkillNumber : SimpleValue {
        [XmlElement]
        public bool Trained { get; set; }

        public override string ToString () {
            return String.Format("{0} {1:+#;-#;0}", Name, Value);
        }
    }
}
