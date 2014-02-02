using System;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace AnotherCM.WPF.ViewModels {
    public abstract class FlyoutBaseViewModel : PropertyChangedBase, IFlyout {
        private string header;
        private bool isOpen;
        private Position position;
        private double widget;

        public string Header {
            get {
                return this.header;
            }

            set {
                if (value == this.header) {
                    return;
                }

                this.header = value;
                this.NotifyOfPropertyChange(() => this.Header);
            }
        }

        public bool IsOpen {
            get {
                return this.isOpen;
            }

            set {
                if (value.Equals(this.isOpen)) {
                    return;
                }

                this.isOpen = value;
                this.NotifyOfPropertyChange(() => this.IsOpen);
            }
        }

        public Position Position {
            get {
                return this.position;
            }

            set {
                if (value == this.position) {
                    return;
                }

                this.position = value;
                this.NotifyOfPropertyChange(() => this.Position);
            }
        }

        public double Widget {
            get {
                return this.widget;
            }

            set {
                if (value == this.widget) {
                    return;
                }

                this.widget = value;
                this.NotifyOfPropertyChange(() => this.Widget);
            }
        }
    }
}