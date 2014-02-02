using System;
using MahApps.Metro.Controls;

namespace AnotherCM.WPF.Framework {

    public interface IFlyout {

        string Header { get; set; }

        bool IsOpen { get; set; }

        Position Position { get; set; }

        double Widget { get; set; }
    }
}