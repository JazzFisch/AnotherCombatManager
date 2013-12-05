using System;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    internal static class StringMethods {
        public static string FixWhitespace (this string value) {
            if (value == null) {
                return null;
            }

            var str = value.Trim().Replace('\r', '\n').Replace("\n\n", "\n").Replace("\n", "###").Replace('\t', ' ');
            if (String.IsNullOrWhiteSpace(str)) {
                return null;
            }
            else {
                return str;
            }
        }

        public static int SafeToInt (this string value, int defaultValue = 0) {
            int output;
            if (String.IsNullOrWhiteSpace(value) || !Int32.TryParse(value.Trim(), out output)) {
                output = defaultValue;
            }
            return output;
        }
    }
}
