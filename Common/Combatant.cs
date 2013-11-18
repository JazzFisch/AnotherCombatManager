using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DnD4e.LibraryHelper.Common {
    public abstract class Combatant {
        public Dictionary<AbilityScore, int> AbilityScores { get; set; }

        public int ActionPoints { get; set; }

        public Alignment Alignment { get; set; }

        public abstract CombatantType CombatantType { get; }

        public Dictionary<Defense, int> Defenses { get; set; }

        public int Experience { get; set; }

        public string Handle { get; set; }

        public int HitPoints { get; set; }

        public int Initiative { get; set; }

        public int Speed { get; set; }

        public List<string> Languages { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string Race { get; set; }

        public string Role { get; set; }

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
