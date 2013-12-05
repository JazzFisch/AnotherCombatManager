using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AnotherCM.Library.Import.ExtensionMethods {
    internal static class StringMethods {
        public static async Task<T> DeserializeXmlAsync<T> (this string xmlString) where T : class {
            return await Task.Run<T>(() => {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xmlString)) {
                    using (var xml = new XmlTextReader(reader)) {
                        if (serializer.CanDeserialize(xml)) {
                            return serializer.Deserialize(xml) as T;
                        }
                        else {
                            return default(T);
                        }
                    }
                }
            }).ConfigureAwait(false);
        }

    }
}
