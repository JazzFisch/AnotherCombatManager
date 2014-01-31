using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AnotherCM.WPF.Framework {
    internal static class WebBrowserExtensionMethods {

        public static void AddScriptElement (this WebBrowser webBrowser, Uri scriptUrl) {
            dynamic document = webBrowser.Document;
            if (document == null) {
                return;
            }

            dynamic head = document.GetElementsByTagName("head");
            if (head != null) {
                string scriptText = GetResourceString(scriptUrl);

                dynamic script = document.CreateElement("script");
                script.type = "text/javascript";
                script.text = scriptText;
                head[0].appendChild(script);
            }
        }

        public static void AddStyleSheet (this WebBrowser webBrowser, Uri styleSheetUrl) {
            dynamic document = webBrowser.Document;
            if (document == null) {
                return;
            }

            dynamic head = document.GetElementsByTagName("head");
            if (head != null) {
                string styleSheetText = GetResourceString(styleSheetUrl);

                dynamic style = document.CreateElement("style");
                style.type = "text/css";
                style.media = "screen";
                style.styleSheet.cssText = styleSheetText;
                head[0].appendChild(style);
            }
        }

        public static void EvalScript (this WebBrowser webBrowser, string scriptText) {
            webBrowser.InvokeScript("eval", new object[] { scriptText });
        }

        private static string GetResourceString (Uri uri) {
            var stream = Application.GetResourceStream(uri).Stream;
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }

        }
    }
}