using System;
using System.Windows.Forms;

namespace DnD4e.CombatManager.Test.ExtensionMethods {
    internal static class WebBrowserMethods {
        public static void AddScriptElement (this WebBrowser webBrowser, string scriptText) {
            HtmlElementCollection head = webBrowser.Document.GetElementsByTagName("head");
            if (head != null) {
                dynamic script = webBrowser.Document.CreateElement("script");
                script.DomElement.type = "text/javascript";
                script.DomElement.text = scriptText;
                ((HtmlElement)head[0]).AppendChild(script);
            }
        }

        public static void AddStyleSheet (this WebBrowser webBrowser, string styleSheetText) {
            HtmlElementCollection head = webBrowser.Document.GetElementsByTagName("head");
            if (head != null) {
                dynamic style = webBrowser.Document.CreateElement("style");
                style.DomElement.type = "text/css";
                style.DomElement.media = "screen";
                style.DomElement.styleSheet.cssText = styleSheetText;
                ((HtmlElement)head[0]).AppendChild(style);
            }
        }

        public static void EvalScript (this WebBrowser webBrowser, string scriptText) {
            webBrowser.Document.InvokeScript("eval", new object[] { scriptText });
        }
    }
}
