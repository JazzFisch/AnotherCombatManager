using System;
using System.Collections.Generic;
using System.Diagnostics;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using HtmlAgilityPack;
using ExportTrap = DnD4e.LibraryHelper.Trap.Trap;
using AgilityNode = HtmlAgilityPack.HtmlNode;


using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Import.Trap {
    internal class Trap {
        public static bool TryCreateFromHtml (string compendiumHtml, Uri compendiumUrl, out ExportTrap trap) {
            trap = null;
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(compendiumHtml);
            foreach (var err in doc.ParseErrors) {
                Trace.TraceError(err.ToString());
                System.Diagnostics.Debugger.Break();
            }

            var root = doc.GetElementbyId("detail");
            if (root == null) {
                return false;
            }

            var title = root.SelectSingleNode("h1");
            var subHead = title.SafeGetFirstInnerText("span[@class='thSubHead']");
            var levelParts = title.SafeGetFirstInnerText("span[@class='thLevel']").Split();
            var xp = title.SafeGetFirstInnerText("span[@class='thLevel']/span[@class='thXP']");
            var init = root.SafeGetNextInnerText("span[@class='thInit']/b");
            var source = root.SelectSingleNode("p[@class='publishedIn']/a");
            var stats = GetStats(root);

            var export = new ExportTrap() {
                Name = title.SafeGetFirstInnerText(),
                Type = subHead,
                Level = levelParts[1].SafeToInt(),
                Role = levelParts[2].Trim(),
                Experience = xp.Substring(3).SafeToInt(),
                CompendiumUrl = compendiumUrl.ToString(),
                Initiative = init.SafeToInt(),
                HitPoints = stats["HP"].SafeToInt()
            };
            export.Handle = export.ToHandle();

            trap = export;
            return true;
        }

        private static Dictionary<string, string> GetStats (AgilityNode node) {
            var stats = new Dictionary<string, string>();
            var statNodes = node.SelectNodes("p[@class='thStat']");
            foreach (var statNode in statNodes) {
                var keys = statNode.SelectNodes("b");
                foreach (var key in keys) {
                    stats.Add(key.SafeGetInnerText(), key.SafeGetNextInnerText());
                }
            }

            return stats;
        }
    }
}
