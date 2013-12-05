using System;
using System.Text;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;

namespace AnotherCM.Library.Import.Monster {
    public class Speed {
        [XmlElement]
        public string Details { get; set; }

        [XmlElement("Speed")]
        public SimpleValue Distance { get; set; }

        [XmlElement("ReferencedObject")]
        public ReferencedObject Type { get; set; }

        public string Name { get { return this.Type.Name; } }

        public int Value { get { return this.Distance.Value; } }

        public Speed () {
            this.Distance = new SimpleValue();
            this.Type = new ReferencedObject();
        }

        public override string ToString () {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}", this.Type.Name, this.Distance.FinalValue);
            if (!String.IsNullOrWhiteSpace(this.Details)) {
                sb.Append(" ");
                sb.Append(this.Details);
            }
            return sb.ToString();
        }
    }
}
