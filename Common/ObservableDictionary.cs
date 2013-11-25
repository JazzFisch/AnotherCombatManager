using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Common {
    public class ObservableCombatantDictionary<TCombatant> : ObservableDictionary<string, TCombatant> where TCombatant : Combatant {
        public ObservableCombatantDictionary ()
            : base(new ConcurrentDictionary<string, TCombatant>()) {

        }
    }

    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged {
        private IDictionary<TKey, TValue> dictionary;
        private bool isIDictionary = true;

        public ObservableDictionary ()
            : this(new Dictionary<TKey, TValue>()) {

        }

        public ObservableDictionary (IEqualityComparer<TKey> comparer)
            : this(new Dictionary<TKey, TValue>(comparer)) {
        }

        public ObservableDictionary (IDictionary<TKey, TValue> dictionary) {
            this.dictionary = dictionary;
            this.isIDictionary = this.dictionary is IDictionary;
            this.FireEvents = true;
        }

        public bool FireEvents { get; set; }

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDictionary<TKey, TValue>

        public void Add (TKey key, TValue value) {
            this.Upsert(key, value, true);
        }

        public bool ContainsKey (TKey key) {
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys {
            get { return this.dictionary.Keys; }
        }

        public bool Remove (TKey key) {
            TValue value;
            if (!this.dictionary.TryGetValue(key, out value)) {
                return false;
            }

            bool removed = this.dictionary.Remove(key);
            if (removed) {
                this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
            }
            return removed;
        }

        public bool TryGetValue (TKey key, out TValue value) {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values {
            get { return this.dictionary.Values; }
        }

        public TValue this[TKey key] {
            get {
                return this.dictionary[key];
            }
            set {
                this.Upsert(key, value, false);
            }
        }

        public void Add (KeyValuePair<TKey, TValue> item) {
            this.Upsert(item.Key, item.Value, true);
        }

        public void Clear () {
            KeyValuePair<TKey, TValue>[] copy = new KeyValuePair<TKey, TValue>[this.dictionary.Count];
            this.dictionary.CopyTo(copy, 0);
            this.dictionary.Clear();
            this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, copy);
        }

        public bool Contains (KeyValuePair<TKey, TValue> item) {
            return this.dictionary.Contains(item);
        }

        public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly {
            get { return this.dictionary.IsReadOnly; }
        }

        public bool Remove (KeyValuePair<TKey, TValue> item) {
            return this.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator () {
            return this.dictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
            return ((System.Collections.IEnumerable)this.dictionary).GetEnumerator();
        }

        #endregion

        #region Private Methods

        private void Upsert (TKey key, TValue value, bool add) {
            if (key == null) {
                throw new ArgumentNullException("key");
            }

            TValue old;
            if (this.dictionary.TryGetValue(key, out old) && add) {
                throw new ArgumentException("An item with the same key has already been added.");
            }

            if (Object.ReferenceEquals(value, old)) {
                return;
            }



            var action = add ? NotifyCollectionChangedAction.Add : NotifyCollectionChangedAction.Replace;
            this.dictionary[key] = value;

            if (add) {
                this.OnCollectionChanged(action, new KeyValuePair<TKey, TValue>(key, value));
            }
            else {
                this.OnCollectionChanged(action, new KeyValuePair<TKey, TValue>(key, old), new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        protected virtual void OnCollectionChanged (NotifyCollectionChangedAction action, object newItem, object oldItem) {
            if (this.CollectionChanged != null && this.FireEvents) {
                var args = new NotifyCollectionChangedEventArgs(
                    action,
                    newItem,
                    oldItem
                );
                this.CollectionChanged(this, args);
            }
        }

        protected virtual void OnCollectionChanged (NotifyCollectionChangedAction action, object changedItem) {
            if (this.CollectionChanged != null && this.FireEvents) {
                var args = new NotifyCollectionChangedEventArgs(
                    action,
                    changedItem
                );
                this.CollectionChanged(this, args);
            }
        }

        protected virtual void OnCollectionChanged (NotifyCollectionChangedAction action, IList changedItems) {
            if (this.CollectionChanged != null && this.FireEvents) {
                var args = new NotifyCollectionChangedEventArgs(
                    action,
                    changedItems
                );
                this.CollectionChanged(this, args);
            }
        }

        public virtual void RaiseCollectionChanged (NotifyCollectionChangedAction action) {
            if (this.CollectionChanged != null && this.FireEvents) {
                var args = new NotifyCollectionChangedEventArgs(action);
                this.CollectionChanged(this, args);
            }
        }

        #endregion
    }
}
