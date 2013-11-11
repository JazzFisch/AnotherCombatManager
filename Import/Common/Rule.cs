using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Rule : NamedAttributeElement {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("internal-id")]
        public string InternalId { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("revision-date")]
        public string RevisionDate { get; set; }

        [XmlElement]
        public string Category { get; set; }

        public string[] Categories {
            get {
                if (String.IsNullOrWhiteSpace(this.Category)) {
                    return new string[0];
                }
                var parts = this.Category.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return parts;
            }
        }

        [XmlElement]
        public string Flavor { get; set; }

        [XmlElement("Prereqs")]
        public string Prerequisites { get; set; }

        [XmlElement("specific")]
        public Specifics Specifics { get; set; }

        //[XmlElement("rules")]
        //public object SubRules { get; set; }

        [XmlText]
        public string Text { get; set; }

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
