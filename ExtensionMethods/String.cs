using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class StringMethods {
        public static string FixWhitespace (this string value) {
            if (value == null) {
                return value;
            }

            return value.Replace('\r', '\n').Replace("\n\n", "\n").Replace("\n", "###").Replace('\t', ' ');
        }
    }
}
