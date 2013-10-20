using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class StringBuilderMethods {
        public static StringBuilder AppendIfNotEmpty (this StringBuilder sb, object value) {
            var str = value.ToString();
            if (!String.IsNullOrWhiteSpace(str)) {
                sb.Append(str);
            }
            return sb;
        }

        public static StringBuilder AppendIfNotEmpty (this StringBuilder sb, object test, string value) {
            if (!String.IsNullOrWhiteSpace(test.ToString())) {
                sb.Append(value);
            }
            return sb;
        }

        public static StringBuilder AppendFormatIfNotEmpty (this StringBuilder sb, string format, object arg0) {
            if (arg0 == null) {
                return sb;
            }
            else if (!String.IsNullOrWhiteSpace(arg0.ToString())) {
                sb.AppendFormat(format, arg0);
            }
            return sb;
        }
    }
}
