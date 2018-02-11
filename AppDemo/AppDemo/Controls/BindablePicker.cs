using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using Xamarin.Forms;

namespace AppDemo.Controls
{
    public class BindablePicker : Picker
    {
        bool disableNestedCalls;

        public static readonly new BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BindablePicker),
                null, propertyChanged: OnItemsSourceChanged);

        public static readonly new BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem", typeof(object), typeof(BindablePicker),
                null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty SelectedValueProperty =
            BindableProperty.Create("SelectedValue", typeof(object), typeof(BindablePicker),
                null, BindingMode.TwoWay, propertyChanged: OnSelectedValueChanged);

        public string DisplayMemberPath { get; set; }

        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                if (SelectedItem != value)
                {
                    SetValue(SelectedItemProperty, value);
                    InternalSelectedItemChanged();
                }
            }
        }

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set
            {
                SetValue(SelectedValueProperty, value);
                InternalSelectedValueChanged();
            }
        }

        public string SelectedValuePath { get; set; }

        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        void InstanceOnItemsSourceChanged(object oldValue, object newValue)
        {
            disableNestedCalls = true;
            Items.Clear();

            var oldCollectionINotifyCollectionChanged = oldValue as INotifyCollectionChanged;
            if (oldCollectionINotifyCollectionChanged != null)
            {
                oldCollectionINotifyCollectionChanged.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            var newCollectionINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (newCollectionINotifyCollectionChanged != null)
            {
                newCollectionINotifyCollectionChanged.CollectionChanged += ItemsSource_CollectionChanged;
            }

            if (!Equals(newValue, null))
            {
                var hasDisplayMemberPath = !string.IsNullOrWhiteSpace(DisplayMemberPath);

                foreach (var item in (IEnumerable)newValue)
                {
                    if (hasDisplayMemberPath)
                    {
                        var type = item.GetType();
                        var prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Add(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        Items.Add(item.ToString());
                    }
                }

                SelectedIndex = -1;
                disableNestedCalls = false;

                if (SelectedItem != null)
                {
                    InternalSelectedItemChanged();
                }
                else if (hasDisplayMemberPath && SelectedValue != null)
                {
                    InternalSelectedValueChanged();
                }
            }
            else
            {
                disableNestedCalls = true;
                SelectedIndex = -1;
                SelectedItem = null;
                SelectedValue = null;
                disableNestedCalls = false;
            }
        }

        void InternalSelectedItemChanged()
        {
            if (disableNestedCalls)
            {
                return;
            }

            var selectedIndex = -1;
            object selectedValue = null;
            if (ItemsSource != null)
            {
                var index = 0;
                var hasSelectedValuePath = !string.IsNullOrWhiteSpace(SelectedValuePath);
                foreach (var item in ItemsSource)
                {
                    if (item != null && item.Equals(SelectedItem))
                    {
                        selectedIndex = index;
                        if (hasSelectedValuePath)
                        {
                            var type = item.GetType();
                            var prop = type.GetRuntimeProperty(SelectedValuePath);
                            selectedValue = prop.GetValue(item);
                        }

                        break;
                    }

                    index++;
                }
            }

            disableNestedCalls = true;
            SelectedValue = selectedValue;
            SelectedIndex = selectedIndex;
            disableNestedCalls = false;
        }

        void InternalSelectedValueChanged()
        {
            if (disableNestedCalls)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedValuePath))
            {
                return;
            }

            var selectedIndex = -1;
            object selectedItem = null;
            var hasSelectedValuePath = !string.IsNullOrWhiteSpace(SelectedValuePath);
            if (ItemsSource != null && hasSelectedValuePath)
            {
                var index = 0;
                foreach (var item in ItemsSource)
                {
                    if (item != null)
                    {
                        var type = item.GetType();
                        var prop = type.GetRuntimeProperty(SelectedValuePath);
                        if (object.Equals(prop.GetValue(item), SelectedValue))
                        {
                            selectedIndex = index;
                            selectedItem = item;
                            break;
                        }
                    }

                    index++;
                }
            }

            disableNestedCalls = true;
            SelectedItem = selectedItem;
            SelectedIndex = selectedIndex;
            disableNestedCalls = false;
        }

        void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var hasDisplayMemberPath = !string.IsNullOrWhiteSpace(DisplayMemberPath);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (hasDisplayMemberPath)
                    {
                        var type = item.GetType();
                        var prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Add(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        Items.Add(item.ToString());
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.NewItems)
                {
                    if (hasDisplayMemberPath)
                    {
                        var type = item.GetType();
                        var prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Remove(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        Items.Remove(item.ToString());
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var item in e.NewItems)
                {
                    if (hasDisplayMemberPath)
                    {
                        var type = item.GetType();
                        var prop = type.GetRuntimeProperty(DisplayMemberPath);
                        Items.Remove(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        var index = Items.IndexOf(item.ToString());
                        if (index > -1)
                        {
                            Items[index] = item.ToString();
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Items.Clear();
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (hasDisplayMemberPath)
                        {
                            var type = item.GetType();
                            var prop = type.GetRuntimeProperty(DisplayMemberPath);
                            Items.Remove(prop.GetValue(item).ToString());
                        }
                        else
                        {
                            var index = Items.IndexOf(item.ToString());
                            if (index > -1)
                            {
                                Items[index] = item.ToString();
                            }
                        }
                    }
                }
                else
                {
                    disableNestedCalls = true;
                    SelectedItem = null;
                    SelectedIndex = -1;
                    SelectedValue = null;
                    disableNestedCalls = false;
                }
            }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (Equals(newValue, null) && Equals(oldValue, null))
            {
                return;
            }

            var picker = (BindablePicker)bindable;
            picker.InstanceOnItemsSourceChanged(oldValue, newValue);
        }

        void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (disableNestedCalls)
            {
                return;
            }

            if (SelectedIndex < 0 || ItemsSource == null || !ItemsSource.GetEnumerator().MoveNext())
            {
                disableNestedCalls = true;
                if (SelectedIndex != -1)
                {
                    SelectedIndex = -1;
                }

                SelectedItem = null;
                SelectedValue = null;
                disableNestedCalls = false;
                return;
            }

            disableNestedCalls = true;

            var index = 0;
            var hasSelectedValuePath = !string.IsNullOrWhiteSpace(SelectedValuePath);
            foreach (var item in ItemsSource)
            {
                if (index == SelectedIndex)
                {
                    SelectedItem = item;
                    if (hasSelectedValuePath)
                    {
                        var type = item.GetType();
                        var prop = type.GetRuntimeProperty(SelectedValuePath);
                        SelectedValue = prop.GetValue(item);
                    }

                    break;
                }

                index++;
            }

            disableNestedCalls = false;
        }

        static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var boundPicker = (BindablePicker)bindable;
            boundPicker.ItemSelected?.Invoke(boundPicker, new SelectedItemChangedEventArgs(newValue));
            boundPicker.InternalSelectedItemChanged();
        }

        static void OnSelectedValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var boundPicker = (BindablePicker)bindable;
            boundPicker.InternalSelectedValueChanged();
        }
    }
}