using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AnotherCM.WPF.Framework {
    public static class WebBrowserBehaviors {

        public static readonly DependencyProperty BindableStreamProperty = DependencyProperty.RegisterAttached(
            "BindableStream",
            typeof(Stream),
            typeof(WebBrowserBehaviors),
            new UIPropertyMetadata(null, BindableStreamPropertyChanged)
        );

        public static void BindableStreamPropertyChanged (DependencyObject o, DependencyPropertyChangedEventArgs e) {
            WebBrowser browser = o as WebBrowser;
            if (browser == null) {
                return;
            }

            browser.NavigateToStream(e.NewValue as Stream);
        }

        public static Stream GetBindableStream (DependencyObject obj) {
            return (Stream)obj.GetValue(BindableStreamProperty);
        }

        public static void SetBindableStream (DependencyObject obj, object value) {
            obj.SetValue(BindableStreamProperty, value);
        }
    }
}