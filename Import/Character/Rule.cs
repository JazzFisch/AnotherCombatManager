using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Rule : NamedAttributeElement {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("internal-id")]
        public string InternalId { get; set; }

        [XmlElement("specific")]
        public Specifics Specifics { get; set; }

        public override bool Equals (object obj) {
            Rule rule = obj as Rule;
            if (rule == null) {
                return false;
            }

            if (!String.IsNullOrWhiteSpace(this.InternalId)) {
                return this.InternalId.Equals(rule.InternalId);
            }
            else if (!String.IsNullOrWhiteSpace(this.Name)) {
                return this.Name.Equals(rule.Name);
            }
            else {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode () {
            if (!String.IsNullOrWhiteSpace(this.InternalId)) {
                return this.InternalId.GetHashCode();
            }
            else {
                return this.Name.GetHashCode();
            }
        }

        public override string ToString () {
            return String.Format("{0} : {1}", this.Type, this.Name);
        }
    }
}
