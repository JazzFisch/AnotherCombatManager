function ViewModel() {
    var self = this;

    this.combineArray = function (list) {
        return list && list.join(', ') || '';
    };

    this.addCommas = function (num) {
        var regex = /(\d+)(\d{3})/,
            ret = num + '';

        while (regex.test(ret)) {
            ret = ret.replace(regex, '$1' + ',' + '$2');
        }

        return ret;
    };

    this.addSign = function (num) {
        return (num > 0 ? '+' : '') + num;
    };

    this.Monster = ko.observable();

    this.AbilityScore = function (score) {
        var val = self.Monster().AbilityScores[score],
            bonus = Math.floor(val / 2) - 5,
            sign = bonus > 0 ? '+' : '';
        return val + ' (' + sign + bonus + ')'
    };
    this.Bloodied = function () {
        return (self.Monster().HitPoints / 2).toFixed(0);
    };
    this.Equipment = function () {
        return self.combineArray(self.Monster().Items);
    };
    this.LevelString = function () {
        return 'Level ' + self.Monster().Level + ' ' + self.Monster().GroupRole + ' ' + self.Monster().Role;
    };
    this.TraitsString = function () {
        var prefix = self.Monster().Size + ' ' + self.Monster().Origin + ' ' + self.Monster().Type + ' ',
            keywords = self.combineArray(self.Monster().Keywords);

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
