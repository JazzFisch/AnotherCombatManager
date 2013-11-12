using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Character {
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

        public string Target { get; set; }

        public string Text { get; set; }

        public string Usage { get; set; }

        public List<Weapon> Weapons { get; set; }

        public Power () {
            // TODO: construct all collections
            this.Keywords = new List<string>();
            this.Weapons = new List<Weapon>();
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
