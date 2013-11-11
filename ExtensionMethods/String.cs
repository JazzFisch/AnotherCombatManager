using System;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class StringMethods {
        public static string FixWhitespace (this string value) {
            if (value == null) {
                return value;
            }

            return value.Replace('\r', '\n').Replace("\n\n", "\n").Replace("\n", "###").Replace('\t', ' ');
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
