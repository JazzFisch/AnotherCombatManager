using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD4e.LibraryHelper.Monster {
    public class Power {
        public string Action { get; set; }

        //public List<Attack> Attacks { get; set; }

        public string Flavor { get; set; }

        public bool IsBasic { get; set; }

        public List<string> Keywords { get; set; }

        public string Name { get; set; }

        public string Requirements { get; set; }

        public string Trigger { get; set; }

        public string Type { get; set; }

        public string Usage { get; set; }

        public string UsageDetails { get; set; }

    }
}
