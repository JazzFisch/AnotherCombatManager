using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using AnotherCM.Library.Common;
using AnotherCM.Library.Monster;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;
using StatsView = AnotherCM.WPF.Views.Tabs.StatsView;

namespace AnotherCM.WPF.ViewModels.Tabs {
    [Export(typeof(IShellViewTab))]
    public class StatsViewModel : TabsBaseViewModel, IShellViewTab, IHandle<Renderable> {
        private const string WebBrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private const string DefaultRenderMethod = "renderStatBlock";

        private Renderable render;
        private bool injected;
        private ILog log;
        private Stream stream;
        private StatsView view;

        [ImportingConstructor]
        public StatsViewModel (IEventAggregator eventAggregator) {
            eventAggregator.Subscribe(this);
            this.DisplayName = "stats";
            this.log = LogManager.GetLog(typeof(StatsViewModel));

            // prime the current document
            this.Handle(null);
        }

        public Stream Stream {
            get {
                return this.stream;
            }
            set {
                if (this.stream != value) {
                    this.stream = value;
                    this.NotifyOfPropertyChange(() => this.Stream);
                }
            }
        }

        public void Handle (Renderable renderable) {
            this.log.Info("Handle({0})", renderable);
            if (renderable == null) {
                return;
            }

            if (this.render != renderable) {
                this.render = renderable;
            }

            if (!this.injected) {
                // TODO: determine actual display block to render
                var uri = new Uri("pack://application:,,,/Resources/Html/monsterStatblock.html", UriKind.Absolute);
                this.Stream = Application.GetResourceStream(uri).Stream;
            }
            else {
                Execute.OnUIThread(() => this.Render(renderable));
            }
        }

        public void LoadCompleted (NavigationEventArgs e) {
            this.log.Info("LoadCompleted");

            if (!e.IsNavigationInitiator) {
                return;
            }

            if (this.injected) {
                return;
            }

            this.injected = true;
            this.log.Info("Injecting styles and javascript");

            this.view.Browser.AddStyleSheet(new Uri("pack://application:,,,/Resources/Html/statblock.css", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/modernizr-2.6.2.js", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/underscore.js", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/knockout-3.0.0.debug.js", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/knockout-StringInterpolatingBindingProvider.js", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/ko.ninja.js", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/statblockHelpers.js", UriKind.Absolute));
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/bindingHandlers.js", UriKind.Absolute));

            // TODO: determine actual JS view model to use
            this.view.Browser.AddScriptElement(new Uri("pack://application:,,,/Resources/Html/monsterStatblock.js", UriKind.Absolute));

            // we're already on the UI thread
            this.Render(this.render);
        }

        protected override void OnViewLoaded (object view) {
            var statsView = view as StatsView;
            if (statsView != null) {
                this.view = statsView;
            }


            base.OnViewLoaded(view);
        }

        // this method MUST be run on the UI thread
        private void Render (Renderable renderable) {
            var json = renderable.ToJson();
            this.view.Browser.InvokeScript(DefaultRenderMethod, json);
        }
    }
}