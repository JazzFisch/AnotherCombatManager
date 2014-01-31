using System;
using System.Windows;

namespace AnotherCM.WPF.Controls {
    public interface IViewLocator {
        UIElement GetOrCreateViewType (Type viewType);
    }
}