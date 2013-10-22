using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Common {
    internal class PercentageNumberBonus : DefaultBonus { }

    internal class AddNumberBonus : DefaultBonus { }

    [XmlInclude(typeof(AddNumberBonus))]
    [XmlInclude(typeof(PercentageNumberBonus))]
    internal class DefaultBonus : NamedValueElement {
        [XmlElement]
        public string ID { get; set; }

        [XmlElement]
        public float Value { get; set; }

        public override string ToString () {
            return this.Value.ToString();
        }
    }

    internal class SimpleValue : NamedValueElement {
        [XmlElement("DefaultBonus")]
        public DefaultBonus Bonus { get; set; }

        [XmlElement]
        public string Details { get; set; }

        [XmlAttribute]
        public float FinalValue { get; set; }

        [XmlElement]
        public string ID { get; set; }

        public int Value { get { return (int)Math.Round(this.FinalValue); } }

        public static implicit operator int (SimpleValue value) {
            return value.Value;
        }

        public override string ToString () {
            return String.Format("{0} : {1}", this.Name, this.FinalValue);
        }
    }
}
