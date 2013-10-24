using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DnD4e.LibraryHelper.Common {
    public abstract class Combatant {
        public Dictionary<AbilityScore, int> AbilityScores { get; set; }

        public int ActionPoints { get; set; }

        public Alignment Alignment { get; set; }

        public Dictionary<Defense, int> Defenses { get; set; }

        public int Experience { get; set; }

        public string Handle { get; set; }

        public int HitPoints { get; set; }

        public int Initiative { get; set; }

        public int LandSpeed { get; set; }

        public List<string> Languages { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string Race { get; set; }

        public Dictionary<Skill, int> Skills { get; set; }

        public string Size { get; set; }

        public virtual string ToJson () {
            return JsonConvert.SerializeObject(
                this,
#if DEBUG
                Formatting.Indented,
#else
                Formatting.None,
#endif
                new JsonSerializerSettings() {
                    Converters = new List<JsonConverter>() { new StringEnumConverter() }
                }
            );
        }

        public override string ToString () {
            return this.Handle ?? this.Name;
        }
    }
}
