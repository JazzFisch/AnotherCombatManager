ko.bindingHandlers.abilityScore = {
    update: function (element, valueAccessor) {
        var value = valueAccessor(),
            bonus = Math.floor(value / 2) - 5,
            sign = bonus > 0 ? '+' : '',
            text = value + ' (' + sign + bonus + ')';
        ko.bindingHandlers.text.update(element, function() { return text; });
    }
};

ko.bindingHandlers.stringArray = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var list = ko.utils.unwrapObservable(valueAccessor()),
            options = ko.utils.unwrapObservable(allBindingsAccessor()) || {},
            fallback = options.fallback || '',
            prefix = options.prefix || '',
            joined = list && list.length && prefix + list.join(', ') || '',
            text = joined || (fallback && prefix + fallback) || '';
        ko.bindingHandlers.text.update(element, function() { return text; });
    }
};

ko.bindingHandlers.kvpArray = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var list = ko.utils.unwrapObservable(valueAccessor()),
            options = ko.utils.unwrapObservable(allBindingsAccessor()) || {},
            min = options.min || 1,
            valuePrefix = options.valuePrefix || ' ',
            fallback = options.fallback || '',
            text = [];

            _.each(list, function (kvp) {
                var key = kvp.Key || '',
                    val = kvp.Value && kvp.Value >= min && valuePrefix + kvp.Value || '';
                if (key) {
                    text.push(key + val);
                }
            });

            ko.bindingHandlers.text.update(element, function() { return text.join(', ') });
    }
};

ko.bindingHandlers.commaNum = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var num = ko.utils.unwrapObservable(valueAccessor()),
            options = ko.utils.unwrapObservable(allBindingsAccessor()) || {},
            prefix = options.prefix || '',
            postfix = options.postfix || '',
            sign = options.signed ? (num > 0 ? '+' : '') : '',
            regex = /(\d+)(\d{3})/,
            text = num + '';

        while (regex.test(text)) {
            text = text.replace(regex, '$1' + ',' + '$2');
        }

        ko.bindingHandlers.text.update(element, function() { return prefix + sign + text + postfix; });
    }
};
