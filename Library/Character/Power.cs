using System;
using System.Collections.Generic;

namespace AnotherCM.Library.Character {
    public class Power {
        public string Name { get; set; }

        public string Display { get; set; }

        public string ActionType { get; set; }

        public string Attack { get; set; }

        public string AttackType { get; set; }

        public string Effect { get; set; }

        public string Flavor { get; set; }

        public string Hit { get; set; }

        public List<string> Keywords { get; set; }

        public string PowerType { get; set; }

        // TODO: move Range to Common?
        public AnotherCM.Library.Import.Common.Range Range { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public string Text { get; set; }

        public string Usage { get; set; }

        public List<Weapon> Weapons { get; set; }

        public Power () {
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
