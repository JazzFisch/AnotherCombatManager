using System;

namespace AnotherCM.Library.Character {
    public class SkillValue {
        public bool IsTrained { get; set; }

        public int Value { get; set; }

        public override string ToString () {
            if (!this.IsTrained) {
                return this.Value.ToString();
            }
            else {
                return this.Value.ToString() + "; Trained";
            }
        }
    }
}
