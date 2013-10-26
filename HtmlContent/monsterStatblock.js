var debugJson = '{"CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=363","Description":"","GroupRole":"Solo","IsLeader":true,"Items":[{"Key":"Wand of Orcus","Value":1}],"Keywords":["Demon"],"Immunities":["Disease","Poison","Necrotic"],"Origin":"Elemental","OtherSpeeds":["Fly 10 (clumsy)","Teleport 6"],"Phasing":false,"Regeneration":0,"Resistances":["20 variable (3/encounter)"],"Role":"Brute","SavingThrows":5,"Senses":[{"Key":"low-light vision","Value":0},{"Key":"Darkvision","Value":0}],"SourceBook":"Monster Manual 1","SourceBooks":null,"Tactics":null,"Type":"Humanoid","Weaknesses":[],"AbilityScores":{"Strength":35,"Constitution":33,"Dexterity":22,"Intelligence":25,"Wisdom":25,"Charisma":30},"ActionPoints":2,"Alignment":"ChaoticEvil","Defenses":{"AC":48,"Fortitude":51,"Reflex":46,"Will":49},"Experience":155000,"Handle":"Orcus (Br33S)","HitPoints":1525,"Initiative":22,"Speed":6,"Languages":["Abyssal"],"Level":33,"Name":"Orcus","Race":null,"Skills":{"Arcana":28,"History":28,"Intimidate":31,"Religion":28,"Perception":28},"Size":"Gargantuan"}';

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
