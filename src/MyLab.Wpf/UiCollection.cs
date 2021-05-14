using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyLab.Wpf
{
    public class UiCollection<T> : INotifyPropertyChanged, INotifyCollectionChanged, ICollection<T>
        where T : class
    {

        private readonly IVmStateUpdater _ownerStateUpdater;
        private bool _isEmpty;
        private T _selectedItem;
        private bool _hasSelected;

        private static readonly string IsEmptyName = nameof(IsEmpty);
        private static readonly string SelectedItemName = nameof(SelectedItem);
        private static readonly string HasSelectedName = nameof(HasSelected);

        public event EventHandler SelectedItemChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<T> Source { get; }

        public bool AutoSelectFirstItem { get; set; }

        public bool ExcludeSelected { get; set; }
        public bool IsEmpty
        {
            get => _isEmpty;
            private set
            {
                _isEmpty = value;
                OnPropertyChanged(IsEmptyName);
            }
        }

        public bool HasSelected
        {
            get => _hasSelected;
            private set
            {
                _hasSelected = value;
                OnPropertyChanged(HasSelectedName);
            }
        }

        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                var oldSelected = _selectedItem;

                _selectedItem = value;
                OnPropertyChanged(SelectedItemName);

                HasSelected = _selectedItem != null;
                OnSelectedItemChanged();

                if (ExcludeSelected)
                {
                    if (oldSelected != null)
                        Source.Add(oldSelected);
                    if (_selectedItem != null)
                        Source.Remove(_selectedItem);
                }

                _ownerStateUpdater?.UpdateStates();
            }
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
                Source.Add(item);
            UpdateProperties();
        }

        public UiCollection(IVmStateUpdater ownerStateUpdater)
            : this()
        {
            _ownerStateUpdater = ownerStateUpdater;
        }

        public UiCollection()
        {
            Source = new ObservableCollection<T>();
            Source.CollectionChanged += Source_CollectionChanged;

            AutoSelectFirstItem = true;

            UpdateProperties();
        }

        void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(sender, e);
        }

        protected void UpdateProperties()
        {
            bool wasEmpty = IsEmpty;

            IsEmpty = GetItemsCount(Source) == 0;

            if (!wasEmpty && IsEmpty && SelectedItem != null)
                SelectedItem = null;

            if (wasEmpty && !IsEmpty && AutoSelectFirstItem)
                SelectedItem = Source.FirstOrDefault();
        }

        protected virtual void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateProperties();

            CollectionChanged?.Invoke(sender, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual int GetItemsCount(ObservableCollection<T> source)
        {
            return source.Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            Source?.Add(item);
        }

        public void Clear()
        {
            Source?.Clear();
        }

        public bool Contains(T item)
        {
            return Source?.Contains(item) ?? false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Source?.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return Source?.Remove(item) ?? false;
        }

        public int Count => Source?.Count ?? 0;
        public bool IsReadOnly => ((ICollection<T>) Source)?.IsReadOnly ?? false;
    }
}
