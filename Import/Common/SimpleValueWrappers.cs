using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Common {
    // simple wrapper to get XML naming right
    public class AbilityScoreNumber : SimpleValue { }

    public class CalculatedNumber : SimpleValue { }

    public class DamageConstant : SimpleValue { }

    public class MonsterSavingThrow : SimpleValue { }

    public class SimpleAdjustableNumber : SimpleValue { }

    public class SkillNumber : SimpleValue {
        [XmlElement]
        public bool Trained { get; set; }

        public override string ToString () {
            return String.Format("{0} {1:+#;-#;0}", Name, Value);
        }
    }
}
