using System;
using System.Xml.Serialization;

namespace AnotherCM.Library.Import.Character {
    public class Damage {
        private string expression;

        [XmlText]
        public string Expression {
            get { return this.expression; }
            set { 
                this.expression = value.Trim();
                string[] parts = this.expression.Split(new char[] { '+', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) {
                    return;
                }

                string[] diceParts;
                string bonus;

                if (parts[0].Contains("d")) {
                    diceParts = parts[0].Split(new char[] { 'd', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    bonus = parts[1];
                }
                else if (parts[1].Contains("d")) {
                    diceParts = parts[1].Split(new char[] { 'd', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    bonus = parts[0];
                }
                else {
                    return;
                }

                if (diceParts.Length != 2) {
                    return;
                }

                this.Bonus = Int32.Parse(bonus);
                this.Dice = Int32.Parse(diceParts[0]);
                this.DiceSides = Int32.Parse(diceParts[1]);
            }
        }

        [XmlIgnore]
        public int Bonus { get; set; }

        [XmlIgnore]
        public int Dice { get; set; }

        [XmlIgnore]
        public int DiceSides { get; set; }

        public override string ToString () {
            return this.Expression;
        }
    }
}
