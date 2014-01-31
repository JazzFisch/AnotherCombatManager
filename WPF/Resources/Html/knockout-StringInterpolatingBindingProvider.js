/* --- String interpolating binding provider ---
   Allows {{ expr }} syntax in DOM. Could be an official KO plugin
   loaded from a separate file. Note that preprocessNode is a fairly
   low-level API that developers are not often expected to use directly.
*/

var StringInterpolatingBindingProvider = function() {
    this.constructor = StringInterpolatingBindingProvider;

    var expressionRegex = /{{([\s\S]+?)}}/g;

    this.preprocessNode = function(node) {
        if (node.nodeType === 3 && node.nodeValue) {
            // Preprocess by replacing {{ expr }} with <!-- ko text: expr --><!-- /ko --> nodes
            var newNodes = replaceExpressionsInText(node.nodeValue, expressionRegex, function(expressionText) {
                return [
                    document.createComment("ko text:" + expressionText),
                    document.createComment("/ko")
                ];
            });

            // Insert the resulting nodes into the DOM and remove the original unpreprocessed node
            if (newNodes) {
                for (var i = 0; i < newNodes.length; i++) {
                    node.parentNode.insertBefore(newNodes[i], node);
                }
                node.parentNode.removeChild(node);
                return newNodes;
            }
        }
    };

    function replaceExpressionsInText(text, expressionRegex, callback) {
        var prevIndex = expressionRegex.lastIndex = 0,
            resultNodes = null,
            match;

        // Find each expression marker, and for each one, invoke the callback
        // to get an array of nodes that should replace that part of the text
        while (match = expressionRegex.exec(text)) {
            var leadingText = text.substring(prevIndex, match.index);
            prevIndex = expressionRegex.lastIndex;
            resultNodes = resultNodes || [];

            // Preserve leading text
            if (leadingText) {
                resultNodes.push(document.createTextNode(leadingText));
            }

            resultNodes.push.apply(resultNodes, callback(match[1]));
        }

        // Preserve trailing text
        var trailingText = text.substring(prevIndex);
        if (resultNodes && trailingText) {
            resultNodes.push(document.createTextNode(trailingText));
        }

        return resultNodes;
    }
};

StringInterpolatingBindingProvider.prototype = ko.bindingProvider.instance;