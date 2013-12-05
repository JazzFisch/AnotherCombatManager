using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace AnotherCM.Library.Common {
    public class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged {
        #region Fields

        private Func<TItem, TKey> keyFunction;

        #endregion

        #region Constructors

        public ObservableKeyedCollection () : this(null, null) { }

        public ObservableKeyedCollection (Func<TItem, TKey> keyFunc) : this(keyFunc, null) { }

        public ObservableKeyedCollection (IEqualityComparer<TKey> comparer) : this(null, comparer) { }

        public ObservableKeyedCollection (Func<TItem, TKey> keyFunction, IEqualityComparer<TKey> comparer) 
            : base(comparer)
        {
            this.keyFunction = keyFunction;
        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged (NotifyCollectionChangedAction action) {
            var changed = this.CollectionChanged;
            if (changed != null) {
                changed(this, new NotifyCollectionChangedEventArgs(action));
            }
        }

        protected void OnCollectionChanged (NotifyCollectionChangedAction action, TItem changedItem) {
            var changed = this.CollectionChanged;
            if (changed != null) {
                changed(this, new NotifyCollectionChangedEventArgs(action, changedItem));
            }
        }

        protected void OnCollectionChanged (NotifyCollectionChangedAction action, TItem newItem, TItem oldItem) {
            var changed = this.CollectionChanged;
            if (changed != null) {
                changed(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
            }
        }
        
        #endregion

        #region KeyedCollection Overrides

        protected override void ClearItems () {            
            base.ClearItems();
            this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        protected override TKey GetKeyForItem (TItem item) {
            if (this.keyFunction == null) {
                throw new InvalidOperationException("Key function not set.");
            }
            return this.keyFunction(item);
        }

        protected override void InsertItem (int index, TItem item) {
            base.InsertItem(index, item);
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        protected override void RemoveItem (int index) {
            var item = base.Items[index];
            base.RemoveItem(index);
            this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
        }

        protected override void SetItem (int index, TItem item) {
            var old = base.Items[index];
            base.SetItem(index, item);
            this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, item, old);
        }

        #endregion

        public void Set (TItem item) {
            var key = this.GetKeyForItem(item);
            if (!base.Contains(key)) {
                base.Add(item);
            }

            base.Dictionary[key] = item;
        }

        public bool TryGetValue (TKey key, out TItem item) {
            return base.Dictionary.TryGetValue(key, out item);
        }
    }
}
