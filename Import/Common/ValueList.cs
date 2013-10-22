using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Common {
    internal class ValueList<TValue> {
        [XmlArray("Values")]
        public List<TValue> Values { get; set; }

        public ValueList () {
            this.Values = new List<TValue>();
        }

        public void Add (TValue item) {
            this.Values.Add(item);
        }

        public TValue this[int index] {
            get {
                return this.Values[index];
            }
        }
    }
}
