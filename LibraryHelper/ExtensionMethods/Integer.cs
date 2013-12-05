using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class IntegerMethods {
        public static string ToIteration (this int iteration) {
            switch (iteration) {
                case 0:
                    return "Zeroth";

                case 1:
                    return "First";

                case 2:
                    return "Second";

                case 3:
                    return "Third";

                case 4:
                    return "Fourth";

                case 5:
                    return "Fifth";

                case 6:
                    return "Sixth";

                case 7:
                    return "Seventh";

                case 8:
                    return "Eighth";

                case 9:
                    return "Nineth";

                default:
                    return String.Empty;
            }
        }
    }
}
