var Combatant = ko.ViewModel.extend({
    
});

var Character = Combatant.extend({
});

var Monster = Combatant.extend({
});

var Trap = Combatant.extend({
});

function toCommaNum (num, signed) {
    var sign = signed ? (num > 0 ? '+' : '') : '',
        regex = /(\d+)(\d{3})/,
        text = num + '';

    while (regex.test(text)) {
        text = text.replace(regex, '$1' + ',' + '$2');
    }

    return sign + text;
}