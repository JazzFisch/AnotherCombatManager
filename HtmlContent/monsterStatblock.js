//var debugJson = '{"CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4578","Description":"","GroupRole":"Standard","IsLeader":false,"Keywords":[],"Immunities":[],"Origin":"Immortal","Phasing":false,"Regeneration":0,"Resistances":[],"Role":"Soldier","SavingThrows":0,"SourceBook":"Plane Above","SourceBooks":null,"Tactics":null,"Type":"Humanoid","Weaknesses":[{"Name":"Acid","Details":"or fire (if the battlesworn war troll defender takes acid or fire damage","Value":0},{"Name":"its","Details":"regeneration doesn’t function until the end of its next turn)","Value":0}],"AbilityScores":{"Strength":30,"Constitution":26,"Dexterity":22,"Intelligence":12,"Wisdom":22,"Charisma":14},"ActionPoints":0,"Alignment":"ChaoticEvil","Defenses":{"AC":42,"Fortitude":40,"Reflex":36,"Will":36},"Experience":9000,"Handle":"Battlesworn War Troll Defender (So26S)","HitPoints":242,"Initiative":21,"LandSpeed":8,"Languages":["Common","Giant"],"Level":26,"Name":"Battlesworn War Troll Defender","Race":null,"Skills":{"Perception":24,"Athletics":28,"Endurance":26},"Size":"Large"}';
//var debugJson = '{"CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2023","Description":"","GroupRole":"Standard","IsLeader":false,"Keywords":["Undead","Swarm"],"Immunities":["Disease","Poison"],"Origin":"Natural","Phasing":false,"Regeneration":0,"Resistances":[{"Name":"half","Details":"damage from melee and ranged attacks","Value":0}],"Role":"Soldier","SavingThrows":0,"SourceBook":"Open Grave","SourceBooks":null,"Tactics":null,"Type":"Animate","Weaknesses":[{"Name":"against","Details":"close and area attacks","Value":10}],"AbilityScores":{"Dexterity":18,"Constitution":13,"Strength":15,"Intelligence":2,"Wisdom":12,"Charisma":9},"ActionPoints":0,"Alignment":"Unaligned","Defenses":{"AC":20,"Fortitude":16,"Reflex":17,"Will":15},"Experience":175,"Handle":"Corpse Rat Swarm (So4S)","HitPoints":53,"Initiative":8,"LandSpeed":4,"Languages":[],"Level":4,"Name":"Corpse Rat Swarm","Race":null,"Skills":{"Perception":8},"Size":"Medium"}';
//var debugJson = '{"CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=363","Description":"","GroupRole":"Solo","IsLeader":true,"Keywords":["Demon"],"Immunities":["Disease","Poison","Necrotic"],"Origin":"Elemental","Phasing":false,"Regeneration":0,"Resistances":[{"Name":"variable","Details":"(3/encounter)","Value":20}],"Role":"Brute","SavingThrows":5,"SourceBook":"Monster Manual 1","SourceBooks":null,"Tactics":"Those unfortunate enough to meet Orcus rarely survive the experience. The demon lord surrounds himself with undead guards and minions, and eagerly meets any challenge to battle. He likes to crush foes with the Wand of Orcus and uses master of undeath to make dread wraiths out of those he slays. Against a particularly trouble foe, he uses touch of death. When an enemy moves into an adjacent square, the demon lord strikes with his spined tail. When surrounded by numerous foes, he spends an action point to use necrotic burst.","Type":"Humanoid","Weaknesses":[],"AbilityScores":{"Strength":35,"Constitution":33,"Dexterity":22,"Intelligence":25,"Wisdom":25,"Charisma":30},"ActionPoints":2,"Alignment":"ChaoticEvil","Defenses":{"AC":48,"Fortitude":51,"Reflex":46,"Will":49},"Experience":155000,"Handle":"Orcus (Br33S)","HitPoints":1525,"Initiative":22,"LandSpeed":6,"Languages":["Abyssal"],"Level":33,"Name":"Orcus","Race":null,"Skills":{"Arcana":28,"History":28,"Intimidate":31,"Religion":28,"Perception":28},"Size":"Gargantuan"}';
//var debugJson = '{"CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=290","Description":"","GroupRole":"Elite","IsLeader":false,"Keywords":["Undead"],"Immunities":["Disease","Poison"],"Origin":"Natural","Phasing":false,"Regeneration":0,"Resistances":[{"Name":"Necrotic","Details":"","Value":10}],"Role":"Lurker","SavingThrows":2,"SourceBook":"Monster Manual 1","SourceBooks":null,"Tactics":null,"Type":"Humanoid","Weaknesses":[{"Name":"Radiant","Details":"","Value":10}],"AbilityScores":{"Strength":16,"Constitution":13,"Dexterity":20,"Intelligence":12,"Wisdom":11,"Charisma":16},"ActionPoints":1,"Alignment":"Evil","Defenses":{"AC":29,"Fortitude":30,"Reflex":27,"Will":25},"Experience":1200,"Handle":"Vampire Lord (Human Rogue) (Lu11E)","HitPoints":186,"Initiative":12,"LandSpeed":8,"Languages":["Common"],"Level":11,"Name":"Vampire Lord (Human Rogue)","Race":null,"Skills":{"Acrobatics":15,"Athletics":18,"Bluff":13,"Intimidate":13,"Stealth":15,"Thievery":15,"Perception":10},"Size":"Medium"}';

function ViewModel() {
    var self = this;

    this.Monster = ko.observable();

    this.Bloodied = function () {
        return (self.Monster().HitPoints / 2).toFixed(0);
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

// externally invoked method to render a new StatBlock
function renderStatBlock(json) {
    var data = ko.utils.parseJson(json);
    window.vm.Monster(data);
    ko.applyBindings(window.vm);
}

//$(document).ready(function () {
//    renderStatBlock(debugJson);
//});
