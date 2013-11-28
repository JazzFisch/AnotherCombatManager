using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DnD4e.LibraryHelper.Common {
    [DefaultProperty("Name")]
    public abstract class Combatant {
        protected const string MissingName = "Missing";

        public Dictionary<AbilityScore, int> AbilityScores { get; set; }

        public int ActionPoints { get; set; }

        public Alignment Alignment { get; set; }

        [Browsable(false)]
        public abstract CombatantType CombatantType { get; }

        public Dictionary<Defense, int> Defenses { get; set; }

        public int Experience { get; set; }

        [Browsable(false)]
        public string Handle { get; set; }

        public int HitPoints { get; set; }

        public int Initiative { get; set; }

        public int Speed { get; set; }

        public List<string> Languages { get; set; }

        [Category("General")]
        public int Level { get; set; }

        [Category("General")]
        public string Name { get; set; }

        [Category("General")]
        public string Race { get; set; }

        [Category("General")]
        public string Role { get; set; }

        [Category("General")]
        public string Size { get; set; }

        public Combatant () {
            // TODO: construct all collections
            this.AbilityScores = new Dictionary<AbilityScore, int>();
            this.Defenses = new Dictionary<Defense, int>();
            this.Languages = new List<string>();
        }

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
