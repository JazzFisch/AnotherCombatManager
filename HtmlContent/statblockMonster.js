//var text = '{"Type":"Monster","Monster":{"AbilityScores":{"Strength":12,"Constitution":19,"Dexterity":25,"Intelligence":22,"Wisdom":16,"Charisma":25},"ActionPoints":1,"Alignment":"Evil","CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4808","Defenses":{"AC":30,"Fortitude":29,"Reflex":30,"Will":30},"Description":"Death need not be an end to avarice and ambition. As living creatures, beholders must eventually fall from the air to rot on the hated earth. Yet some have the willpower and anger to float again, returning as ghost beholders.","Experience":4000,"GroupRole":"Elite","HitPoints":254,"Initiative":16,"IsLeader":false,"Items":[],"Keywords":["Undead"],"Immunities":["Disease","Poison"],"Languages":["Deep Speech"],"Level":18,"Origin":"Aberrant","Phasing":true,"Race":null,"Regeneration":0,"Role":"Controller","SavingThrows":[2],"Senses":["Darkvision"],"Size":"Large","Skills":{"Perception":17},"Speeds":["Fly : 6 (hover)"],"SourceBooks":["Monster Manual 3"],"Tactics":"A ghost beholder has died once already and is now cautious. It hides in the ground or behind ceilings and walls, using a single eye stalk to spy. Then it emerges, shooting eye rays at its surprised foes.","Type":"Magical Beast","Name":"Ghost Beholder"}}';
//var text = '{"Type":"Monster","Monster":{"AbilityScores":{"Strength":28,"Constitution":24,"Dexterity":22,"Intelligence":12,"Wisdom":12,"Charisma":20},"ActionPoints":2,"Alignment":"Unaligned","CompendiumUrl":"http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2909","Defenses":{"AC":27,"Fortitude":29,"Reflex":26,"Will":25},"Description":"","Experience":6000,"GroupRole":"Solo","HitPoints":608,"Initiative":13,"IsLeader":false,"Items":[],"Keywords":["Dragon"],"Immunities":[],"Languages":["Common","Draconic"],"Level":15,"Origin":"Natural","Phasing":false,"Race":null,"Regeneration":0,"Role":"Brute","SavingThrows":[5],"Senses":["Darkvision"],"Size":"Large","Skills":{"Perception":13,"Athletics":21,"Insight":13},"Speeds":["Fly : 8 (hover)","Overland Flight : 12"],"SourceBooks":["Monster Manual 2"],"Tactics":null,"Type":"Magical Beast","Name":"Adult Silver Dragon"}}';
//var tested = false;

function ViewModel() {
    var self = this;

    this.combineArray = function(prop) {
        return self._raw && self._raw[prop] && self._raw[prop].join(', ') || '';
    };

    this.addCommas = function(num) {
        var regex = /(\d+)(\d{3})/,
            ret = num + '';

        while (regex.test(ret)) {
            ret = ret.replace(regex, '$1' + ',' + '$2');
        }

        return ret;
    };

    // TODO: add comma seperating numbers
    this._raw = {};
    this.Type = ko.observable();
    this.Name = ko.observable();
    this.AbilityScores = ko.observable();
    this.Alignment = ko.observable();
    this.GroupRole = ko.observable();
    this.Role = ko.observable();
    this.Size = ko.observable();
    this.Origin = ko.observable();
    this.Experience = ko.observable();
    this.HitPoints = ko.observable();
    this.Initiative = ko.observable();
    this.Level = ko.observable();
    this.Bloodied = ko.computed(function () {
        return (self.HitPoints() / 2).toFixed(0);
    });
    this.Equipment = ko.computed(function () {
        return self.combineArray('Items');
    });
    this.LevelString = ko.computed(function () {
        return 'Level ' + self.Level() + ' ' + self.GroupRole() + ' ' + self.Role();
    });
    this.TraitsString = ko.computed(function () {
        var prefix = self.Size() + ' ' + self.Origin() + ' ' + self.Type() + ' ',
            keywords = self.combineArray('Keywords');

        if (keywords) {
            keywords = '(' + keywords + ')';
        }
        return prefix + keywords;
    });
}

// transform the ViewModel so that we can map our data to
// it each time renderStatBlock is called
window.vm = ko.mapping.fromJS(new ViewModel());

// externally invoked method to render a new StatBlock
function renderStatBlock(json) {
    var data = ko.utils.parseJson(json);
    if (data.Type === 'Monster') {
        window.vm._raw = data.Monster;
        ko.mapping.fromJS(data.Monster, window.vm);
        ko.applyBindings(window.vm);
    }
}

$(document).ready(function() {
    // to be removed
    //if (!tested) {
    //    tested = true;
    //    renderStatBlock(text);
    //}
});
