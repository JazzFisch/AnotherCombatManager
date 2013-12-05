﻿using System;
using System.Xml.Serialization;

namespace AnotherCM.Library.Import.Common {
    public class TextString : NamedAttributeElement {
        [XmlText]
        public string Value { get; set; }

        public override string ToString () {
            if (String.IsNullOrWhiteSpace(this.Value)) {
                return this.Name.Trim();
            }
            else {
                return this.Value.Trim();
            }
        }
    }
}
