using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Monster {
    public sealed class ValueEnumCollection<TKey, TItem> : IProtectedDictionary
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

        #region IProtectedDictionary Members

        System.Collections.IDictionary IProtectedDictionary.GetDictionary () {
            IProtectedDictionary vals = this.Values;
            if (vals != null) {
                return vals.GetDictionary();
            }
            else {
                return null;
            }
        }

        #endregion
    }
}
