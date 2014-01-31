using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnotherCM.WPF.Controls {
    [Export(typeof(IThemeManager))]
    public class ThemeManager : IThemeManager {
        private readonly ResourceDictionary themeResources;

        public ThemeManager () {
            this.themeResources = new ResourceDictionary {
                Source =
                    new Uri("/Resources/Theme.xaml")
            };
        }

        public ResourceDictionary GetThemeResources () {
            return this.themeResources;
        }
    }
}
