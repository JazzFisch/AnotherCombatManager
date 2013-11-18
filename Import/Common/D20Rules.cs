using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Character;

namespace DnD4e.LibraryHelper.Import.Common {
    [XmlRoot]
    public class D20Rules {
        [XmlElement("RulesElement")]
        public Rules Rules { get; set; }

        public static async Task<Rules> LoadFromAppDataAsync () {
            var d20Rules = new D20Rules() { Rules = new Rules() };
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var basePath = Path.Combine(appPath, "CBLoader");
            var rulesPath = Path.Combine(basePath, "combined.dnd40");

            if (!File.Exists(rulesPath)) {
                return d20Rules.Rules;
            }

            Stopwatch timer = Stopwatch.StartNew();
            try {
                d20Rules = await Task.Run<D20Rules>(() => {
                    XmlSerializer serializer = new XmlSerializer(typeof(D20Rules));
                    using (var file = new FileStream(rulesPath, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, useAsync: true)) {
                        using (var reader = new XmlTextReader(file)) {
                            if (serializer.CanDeserialize(reader)) {
                                return serializer.Deserialize(reader) as D20Rules;
                            }
                        }
                    }
                    return d20Rules;
                }).ConfigureAwait(false);
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
            finally {
                timer.Stop();
                Trace.TraceInformation("Deserializing D20Rules took {0}ms", timer.ElapsedMilliseconds);
            }

            return d20Rules.Rules;
        }
    }
}
