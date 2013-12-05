using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AnotherCM.Library.Import.Common {
    public sealed class ValueEnumCollection<TKey, TItem>
        where TKey : struct
        where TItem : SimpleValue 
    {
        [XmlArray("Values")]
        public EnumCollection<TKey, TItem> Values { get; set; }

        public int Count { get { return this.Values.Count; } }

        public TItem this[TKey key] {
            get { return this.Values[key]; }
        }

        public TItem this[int index] {
            get { return this.Values[index]; }
            set { this.Values[index] = value; }
        }

        public ValueEnumCollection () {
            this.Values = new EnumCollection<TKey, TItem>();
        }
    }
}
