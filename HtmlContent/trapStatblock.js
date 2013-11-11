function ViewModel() {
    var self = this;

    this.Bound = false;

    this.Trap = ko.observable();

    this.LevelString = function () {
        var groupRole = self.Trap().GroupRole,
            role = self.Trap().Role
            parts = [];

        parts.push('Level');
        parts.push(self.Trap().Level);
        if (role !== 'No Role') {
            parts.push(role);
        }
        return parts.join(' ');
    };
    this.XPString = function () {
        return 'XP ' + toCommaNum(self.Trap().Experience);
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
        window.vm.Trap(data);
        if (!vm.Bound) {
            vm.Bound = true;
            ko.applyBindings(window.vm);
        }
    }
    catch (e) {
        alert(e.toString());
    }
}

//$(document).ready(function () {
//   renderStatBlock(debugJson);
//});
