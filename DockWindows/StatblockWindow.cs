using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Character;
using DnD4e.LibraryHelper.Monster;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class StatblockWindow : DockContent {
        #region Fields

        private const string WebBrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private Combatant combatant = null;

        #endregion

        #region Constructors

        public StatblockWindow () {
            // fix IE rendering modes... *sigh*
            // perform prior to any other initialization
            this.SetBrowserRenderingRegistryKeys(addKeys: true);

            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public Combatant Combatant {
            get {
                return this.combatant;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                if ((this.combatant == null) || !this.combatant.Handle.Equals(value.Handle, StringComparison.OrdinalIgnoreCase)) {
                    this.BeginRenderCombatant(value);
                    this.Text = value.Name;
                    this.combatant = value;
                }
            }
        }

        #endregion

        #region Event Handlers

        private void StatblockWindow_FormClosing (object sender, FormClosingEventArgs e) {
            // remove the browser rendering keys
            this.SetBrowserRenderingRegistryKeys(addKeys: false);
        }

        #endregion

        private void BeginRenderCombatant (Combatant combatant) {
            bool correctPage = this.combatant != null && combatant.CombatantType == this.combatant.CombatantType;
            Task<string> serializeTask = combatant.ToJsonAsync();
            if (correctPage) {
                this.InvokeJavascriptRender(serializeTask);
                return;
            }

            string html = null;
            WebBrowserDocumentCompletedEventHandler completedHandler = null;
            if (combatant is Character) {
                html = Properties.Resources.characterStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.webBrowser.AddScriptElement(Properties.Resources.characterStatblock_js);
                    this.InvokeJavascriptRender(serializeTask);
                };
            }
            else if (combatant is Monster) {
                html = Properties.Resources.monsterStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.webBrowser.AddScriptElement(Properties.Resources.monsterStatblock_js);
                    this.InvokeJavascriptRender(serializeTask);
                };
            }
            else {
                return;
            }

            this.webBrowser.AllowNavigation = true;
            this.webBrowser.DocumentText = html;
            this.webBrowser.DocumentCompleted += completedHandler;
        }

        private void InvokeJavascriptRender (Task<string> serializeTask) {
            if (serializeTask == null) {
                return;
            }

            // this will wait if the serialize is still running
            string json = serializeTask.Result;

            try {
                this.webBrowser.Document.InvokeScript(
                    "renderStatBlock",
                    new object[] { json }
                );
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }

        private void SetBrowserRenderingRegistryKeys (bool addKeys) {
            try {
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
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }

        private void StopListeningAndAddCommonHtmlElements (WebBrowserDocumentCompletedEventHandler completedHandler) {
            // ordering of the following is IMPORTANT
            // stop listening
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.DocumentCompleted -= completedHandler;

            // load our css in
            this.webBrowser.AddStyleSheet(Properties.Resources.statblock_css);

            // load our javascript in
            this.webBrowser.AddScriptElement(Properties.Resources.modernizr_2_6_2_js);
            this.webBrowser.AddScriptElement(Properties.Resources.underscore_js);
            this.webBrowser.AddScriptElement(Properties.Resources.knockout_3_0_0_debug_js);
            this.webBrowser.AddScriptElement(Properties.Resources.knockout_StringInterpolatingBindingProvider_js);
            this.webBrowser.AddScriptElement(Properties.Resources.ko_ninja_js);
            this.webBrowser.AddScriptElement(Properties.Resources.statblockHelpers_js);
            this.webBrowser.AddScriptElement(Properties.Resources.bindingHandlers_js);
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
