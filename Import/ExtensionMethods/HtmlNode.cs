using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgilityNode = HtmlAgilityPack.HtmlNode;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class HtmlNode {
        public static string SafeGetFirstInnerText (this AgilityNode node, string selector) {
            var subNode = node.SelectSingleNode(selector);
            return subNode.SafeGetFirstInnerText();
        }

        public static string SafeGetFirstInnerText (this AgilityNode node) {
            if (node == null || !node.HasChildNodes) {
                return String.Empty;
            }

            return node.ChildNodes[0].SafeGetInnerText();
        }

        public static string SafeGetInnerText (this AgilityNode node) {
            if (node == null || String.IsNullOrWhiteSpace(node.InnerText)) {
                return String.Empty;
            }

            return node.InnerText.Trim();
        }

        public static string SafeGetNextInnerText (this AgilityNode node, string selector) {
            var subNode = node.SelectSingleNode(selector);
            return subNode.SafeGetNextInnerText();
        }

        public static string SafeGetNextInnerText (this AgilityNode node) {
            if (node == null || node.NextSibling == null) {
                return String.Empty;
            }

            return node.NextSibling.SafeGetInnerText();
        }
    }
}
