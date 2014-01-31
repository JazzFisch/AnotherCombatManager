using System;
using System.Windows;

namespace AnotherCM.WPF.Controls {
    public interface IThemeManager {
        ResourceDictionary GetThemeResources ();
    }
}