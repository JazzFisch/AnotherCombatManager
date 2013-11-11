using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Character;

namespace DnD4e.LibraryHelper.Import.Common {
    [XmlRoot]
    public class D20Rules {
        [XmlElement("RulesElement")]
        public Rules Rules { get; set; }

        public static bool TryCreateFromAppData (out D20Rules d20Rules) {
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var basePath = Path.Combine(appPath, "CBLoader");
            var rulesPath = Path.Combine(basePath, "combined.dnd40");

            d20Rules = null;
            try {
                Stopwatch timer = Stopwatch.StartNew();
                using (var fs = new FileStream(rulesPath, FileMode.Open, FileAccess.Read)) {
                    using (var xml = new XmlTextReader(fs)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(D20Rules));
                        if (serializer.CanDeserialize(xml)) {
                            d20Rules = serializer.Deserialize(xml) as D20Rules;
                            return true;
                        }
                    }
                }
                timer.Stop();
                Trace.WriteLine(String.Format("Deserializing D20Rules took {0}ms", timer.ElapsedMilliseconds));
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
                System.Diagnostics.Debugger.Break();
            }

            return false;
        }
    }
}
