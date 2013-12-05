using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnotherCM.Library.Common {
    [DefaultProperty("Name")]
    public abstract class Renderable {
        [Browsable(false)]
        public string Handle { get; set; }

        [Category("General")]
        public string Name { get; set; }

        [Browsable(false)]
        public abstract RenderType RenderType { get; }

        public virtual string ToJson (bool indent = false) {
            Formatting formatting = indent ? Formatting.Indented : Formatting.None;
            var settings = new JsonSerializerSettings() {
                Converters = new List<JsonConverter>() { new StringEnumConverter() }
            };

            string json = JsonConvert.SerializeObject(this, formatting, settings);
            return json;
        }

        public virtual Task<string> ToJsonAsync (bool indent = false) {
            Formatting formatting = indent ? Formatting.Indented : Formatting.None;
            var settings = new JsonSerializerSettings() {
                Converters = new List<JsonConverter>() { new StringEnumConverter() }
            };

            return JsonConvert.SerializeObjectAsync(this, formatting, settings);
        }

        public override string ToString () {
            return this.Handle ?? this.Name;
        }
    }
}
