var debugJson = '{"CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=363","Description":"","GroupRole":"Solo","IsLeader":true,"Items":[{"Key":"Wand of Orcus","Value":1}],"Keywords":["Demon"],"Immunities":["Disease","Poison","Necrotic"],"Origin":"Elemental","OtherSpeeds":["Fly 10 (clumsy)","Teleport 6"],"Phasing":false,"Powers":[{"Action":"Standard","Flavor":null,"IsBasic":true,"Keywords":["Necrotic","Weapon"],"Name":"Wand of Orcus","Requirements":null,"Trigger":null,"Type":"Melee","Usage":"At-Will","UsageDetails":null},{"Action":"Standard","Flavor":null,"IsBasic":false,"Keywords":["Necrotic"],"Name":"Touch of Death","Requirements":null,"Trigger":null,"Type":"Melee","Usage":"recharge","UsageDetails":"6"},{"Action":"immediate interrupt","Flavor":null,"IsBasic":false,"Keywords":[],"Name":"Tail Lash","Requirements":null,"Trigger":"when an enemy moves or shifts into a square adjacent to Orcus","Type":"Melee","Usage":"at-will","UsageDetails":null},{"Action":"Standard","Flavor":null,"IsBasic":false,"Keywords":["Healing","Necrotic"],"Name":"Necrotic Burst","Requirements":null,"Trigger":null,"Type":"Close","Usage":"recharge","UsageDetails":"6"}],"Regeneration":0,"Resistances":["20 variable (3/encounter)"],"Role":"Brute","SavingThrows":5,"Senses":[{"Key":"low-light vision","Value":0},{"Key":"Darkvision","Value":0}],"SourceBook":"Monster Manual 1","SourceBooks":null,"Tactics":"Those unfortunate enough to meet Orcus rarely survive the experience. The demon lord surrounds himself with undead guards and minions, and eagerly meets any challenge to battle. He likes to crush foes with the Wand of Orcus and uses master of undeath to make dread wraiths out of those he slays. Against a particularly trouble foe, he uses touch of death. When an enemy moves into an adjacent square, the demon lord strikes with his spined tail. When surrounded by numerous foes, he spends an action point to use necrotic burst.","Traits":[{"Name":"Aura of Death","Details":"enemies that enter or start their turns in the aura take 10 necrotic damage (20 necrotic damage while Orcus is bloodied).","Keywords":["Necrotic"],"Range":20},{"Name":"The Dead Rise","Details":"enemies (including flying ones) treat the area within the aura as difficult terrain, and any dead creature within the aura at the start of Orcus’s turn (except those killed by the Wand of Orcus) rises as an abyssal ghoul myrmidon to fight at Orcus’s command.","Keywords":[],"Range":6},{"Name":"Master of Undeath","Details":"At the start of Orcus’s turn, any creature killed by the Wand of Orcus that is still dead rises as a dread wraith under Orcus’s command.","Keywords":[],"Range":0}],"Type":"Humanoid","Weaknesses":[],"AbilityScores":{"Strength":35,"Constitution":33,"Dexterity":22,"Intelligence":25,"Wisdom":25,"Charisma":30},"ActionPoints":2,"Alignment":"ChaoticEvil","Defenses":{"AC":48,"Fortitude":51,"Reflex":46,"Will":49},"Experience":155000,"Handle":"Orcus (Br33S)","HitPoints":1525,"Initiative":22,"Speed":6,"Languages":["Abyssal"],"Level":33,"Name":"Orcus","Race":null,"Skills":{"Arcana":28,"History":28,"Intimidate":31,"Religion":28,"Perception":28},"Size":"Gargantuan"}';

function ViewModel() {
    var self = this;

    this.Bound = false;

    this.Monster = ko.observable();

    this.Bloodied = function () {
        return (self.Monster().HitPoints / 2).toFixed(0);
    };
    this.Skills = function () {
        return _.omit(self.Monster().Skills, 'Perception');
    };
    this.LevelString = function () {
        return 'Level ' + self.Monster().Level + ' ' + self.Monster().GroupRole + ' ' + self.Monster().Role;
    };
    this.TraitsString = function () {
        var prefix = self.Monster().Size + ' ' + self.Monster().Origin + ' ' + self.Monster().Type + ' ',
            keywords = self.Monster().Keywords.join(', ');

        if (keywords) {
            keywords = '(' + keywords + ')';
        }
        return prefix + keywords;
    };
}

// transform the ViewModel so that we can map our data to
// it each time renderStatBlock is called
window.vm = new ViewModel();
ko.bindingProvider.instance = new StringInterpolatingBindingProvider();

// externally invoked method to render a new StatBlock
function renderStatBlock(json) {
    var data = ko.utils.parseJson(json);
    window.vm.Monster(data);
    if (!vm.Bound) {
        vm.Bound = true;
        ko.applyBindings(window.vm);
    }
}

//$(document).ready(function () {
//   renderStatBlock(debugJson);
//});
