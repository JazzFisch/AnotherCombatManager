using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AnotherCM.Library.Common;
using Microsoft.Win32;

namespace AnotherCM.WinForms.Controls {
    // TODO: add reference counted removal for render keys?
    public partial class StatBlockControl : WebBrowser {
        private const string WebBrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private const string DefaultRenderMethod = "renderStatBlock";
        private Renderable selectedObject;

        public StatBlockControl () {
            this.RenderMethod = DefaultRenderMethod;
        }

        public string RenderMethod { get; set; }

        public Renderable SelectedObject {
            get { return this.selectedObject; }
            set {
                if (Object.ReferenceEquals(this.selectedObject, value)) {
                    return;
                }
                else if (value == null) {
                    throw new ArgumentNullException("value");
                }
                else if (this.selectedObject == null || this.selectedObject.RenderType != value.RenderType) {
                    this.BrowseToResource(value);
                }
                else {
                    this.Render(value);
                }
            }
        }

        public void InvokeScript (string scriptName, params object[] args) {
            if (args != null) {
                this.Document.InvokeScript(scriptName, args);
            }
            else {
                this.Document.InvokeScript(scriptName);
            }
        }

        private void BrowseToResource (Renderable render) {
            WebBrowserDocumentCompletedEventHandler completedHandler = null;
            string html, script;
            this.SelectResources(render, out html, out script);

            completedHandler = (sender, e) => {
                this.StopListeningAndAddCommonHtmlElements(completedHandler);
                this.AddScriptElement(script);
                this.Render(render);
            };

            this.AllowNavigation = true;
            this.DocumentCompleted += completedHandler;
            this.DocumentText = html;
        }

        private void Render (Renderable render) {
            this.selectedObject = render;
            string json = render.ToJson();
            this.InvokeScript(this.RenderMethod, json); 
        }

        private void SelectResources (Renderable render, out string html, out string script) {
            switch (render.RenderType) {
                case RenderType.Character:
                    html = Properties.Resources.characterStatblock_html;
                    script = Properties.Resources.characterStatblock_js;
                    break;

                case RenderType.Monster:
                    html = Properties.Resources.monsterStatblock_html;
                    script = Properties.Resources.monsterStatblock_js;
                    break;

                default:
                    throw new ArgumentException("Unknown RenderType", "RenderType");
            }
        }

        private void StopListeningAndAddCommonHtmlElements (WebBrowserDocumentCompletedEventHandler completedHandler) {
            // ordering of the following is IMPORTANT
            // stop listening
            this.AllowNavigation = false;
            this.DocumentCompleted -= completedHandler;

            // load our css in
            this.AddStyleSheet(Properties.Resources.statblock_css);

            // load our javascript in
            this.AddScriptElement(Properties.Resources.modernizr_2_6_2_js);
            this.AddScriptElement(Properties.Resources.underscore_js);
            this.AddScriptElement(Properties.Resources.knockout_3_0_0_debug_js);
            this.AddScriptElement(Properties.Resources.knockout_StringInterpolatingBindingProvider_js);
            this.AddScriptElement(Properties.Resources.ko_ninja_js);
            this.AddScriptElement(Properties.Resources.statblockHelpers_js);
            this.AddScriptElement(Properties.Resources.bindingHandlers_js);
        }

        public static void SetBrowserRenderingRegistryKeys (bool addKeys) {
            var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            var key = Registry.CurrentUser.OpenSubKey(WebBrowserEmulationPath, true);

            if (key == null) {
                return;
            }

            if (addKeys) {
                key.SetValue(exeName, 8000, RegistryValueKind.DWord);
            }
            else if (!addKeys && key.GetValue(exeName) != null) {
                key.DeleteValue(exeName);
            }

#if DEBUG
            // add the vshost
            var ext = Path.GetExtension(exeName);
            exeName = Path.GetFileNameWithoutExtension(exeName);
            exeName += ".vshost" + ext;
            if (addKeys) {
                key.SetValue(exeName, 8000, RegistryValueKind.DWord);
            }
            else if (!addKeys && key.GetValue(exeName) != null) {
                key.DeleteValue(exeName);
            }
#endif
        }
    }

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
