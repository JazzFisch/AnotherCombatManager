using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace AnotherCM.WPF {
    public class AppBootstrapper : BootstrapperBase {
        private CompositionContainer container;
        private ILog logger;

        public AppBootstrapper () {
            LogManager.GetLog = type => new DebugLogger(type);
            this.logger = LogManager.GetLog(typeof(AppBootstrapper));
            Start();
        }

        protected override void BuildUp (object instance) {
            this.logger.Info("BuildUp {0}", instance);
            this.container.SatisfyImportsOnce(instance);
        }

        protected override void Configure () {
            this.logger.Info("Configure");

            var catalogs = AssemblySource.Instance.Select(a => new AssemblyCatalog(a))
                                                  .OfType<ComposablePartCatalog>();
            var catalog = new AggregateCatalog(catalogs);
            this.container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<ILibrary>(new AnotherCM.WPF.Framework.Library());
            batch.AddExportedValue(this.container);

            this.container.Compose(batch);

            this.AddElementConventions();
            this.AddFlyoutsBindingScope();
        }

        protected override IEnumerable<object> GetAllInstances (Type service) {
            this.logger.Info("GetAllInstances {0}", service);
            string contract = AttributedModelServices.GetContractName(service);
            return this.container.GetExportedValues<object>(contract);
        }

        protected override object GetInstance (Type service, string key) {
            this.logger.Info("GetInstance {0}, {1}", service, key);
            string contract = String.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;

            IEnumerable<object> exports;
            try {
                exports = container.GetExportedValues<object>(contract);
            }
            catch (System.Exception ex) {
                this.logger.Error(ex);
                throw;
            }

            if (exports.Any())
                return exports.First();

            throw new ArgumentException(String.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override void OnStartup (object sender, StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }

        private void AddElementConventions () {
            //ConventionManager.AddElementConvention<MultiSelector>(DependencyProperties.BindableSelectedItemsProperty, "SelectedItems", "SelectedItems");
        }

        private void AddFlyoutsBindingScope () {
            // find flyouts
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var oldNamer = BindingScope.GetNamedElements;
            BindingScope.GetNamedElements = obj => {
                var window = obj as MetroWindow;
                if (window == null) {
                    return oldNamer(obj);
                }

                var list = new List<FrameworkElement>(oldNamer(obj));
                var type = obj.GetType();
                var fields = type.GetFields(flags).Where(f => f.DeclaringType == type);
                var flyoutList = fields.Where(f => f.FieldType == typeof(FlyoutsControl))
                                       .Select(f => f.GetValue(obj))
                                       .Cast<FlyoutsControl>();
                list.AddRange(flyoutList);
                return list;
            };
        }
    }
}