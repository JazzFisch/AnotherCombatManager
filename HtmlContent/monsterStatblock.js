// http://jsfiddle.net/7P8Tp/
//var debugJson = beholder;

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
    this.IterationString = function (num) {
        var num = _.isNumber(num) && num || _.isString(num) && parseInt(num, 10) || 0,
            vals = ['Zeroth', 'First', 'Second', 'Third', 'Fourth', 'Fifth', 'Sixth', 'Seventh', 'Eighth', 'Nineth'];

        return vals[num];
    };
}

// transform the ViewModel so that we can map our data to
// it each time renderStatBlock is called
window.vm = new ViewModel();
ko.bindingProvider.instance = new StringInterpolatingBindingProvider();

// externally invoked method to render a new StatBlock
function renderStatBlock(json) {
    try {
        var data = ko.utils.parseJson(json);
        window.vm.Monster(data);
        if (!vm.Bound) {
            vm.Bound = true;
            ko.applyBindings(window.vm);
        }
    }
    catch (e) {
        alert(e.description);
    }
}

//$(document).ready(function () {
//   renderStatBlock(debugJson);
//});
