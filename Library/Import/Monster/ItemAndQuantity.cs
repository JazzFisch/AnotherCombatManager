using System;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;

namespace AnotherCM.Library.Import.Monster {
    public class ItemAndQuantity {
        [XmlElement]
        public int Quantity { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper Item { get; set; }

        public override string ToString () {
            if (this.Quantity > 1) {
                return String.Format("{0} x {1}", this.Quantity, this.Item.Name);
            }
            else {
                return this.Item.Name;
            }
        }
    }
}
