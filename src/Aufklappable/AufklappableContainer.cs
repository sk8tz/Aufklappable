/*
    <copyright file="AufklappableContainer.cs">
        Copyright (c) 2016 stroeb
    
        Licensed under Microsoft Public License (Ms-PL)
        http://opensource.org/licenses/MS-PL
    </copyright>
    <author>stroeb</author>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Aufklappable
{
    /// <summary>
    ///     Represents a container of <see cref="AufklappableItem" /> where only ever one of them can be expanded.
    /// </summary>
    public class AufklappableContainer : ItemsControl
    {
        /// <summary>
        ///     Identifies the <see cref="SelectedItem" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(string),
            typeof(AufklappableContainer),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        private bool _resizeItemsOnNextLayoutUpdate;

        public AufklappableContainer()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        ///     Gets or sets the Tag of the opened <see cref="AufklappableItem" />.
        ///     Setting this property will open the specified <see cref="AufklappableItem" />.
        /// </summary>
        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var aufklappable = dependencyObject as AufklappableContainer;
            if (aufklappable != null)
            {
                aufklappable._resizeItemsOnNextLayoutUpdate = true;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ThrowIfDuplicatedTags();

            ResizeItems();

            SizeChanged += OnSizeChanged;
            LayoutUpdated += OnLayoutUpdated;
        }

        private void ThrowIfDuplicatedTags()
        {
            if (Items.Cast<AufklappableItem>().GroupBy(n => n.Tag).Any(c => c.Count() > 1))
            {
                throw new InvalidOperationException("There must not be two items with the same tag.");
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            _resizeItemsOnNextLayoutUpdate = true;
        }

        private void OnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            if (!_resizeItemsOnNextLayoutUpdate)
            {
                return;
            }

            _resizeItemsOnNextLayoutUpdate = false;
            ResizeItems();
        }

        private void ResizeItems()
        {
            var allItems = Items.Cast<AufklappableItem>().ToList();

            var itemMaxHeight = GetItemMaxHeight(allItems);

            foreach (var item in allItems)
            {
                item.MaxHeight = itemMaxHeight < 0 ? double.PositiveInfinity : itemMaxHeight;
                if (item.Tag == SelectedItem)
                {
                    item.Height = itemMaxHeight < item.ActualHeight ? double.NaN : itemMaxHeight;
                }
            }
        }

        private double GetItemMaxHeight(IEnumerable<AufklappableItem> allItems)
        {
            /*  -------------------------
                | this.Margin.Top       |
                |-----------------------|
                | item[0].Margin.Top    |
                | item[0].ActualHeight  |
                | item[0].Margin.Bottom |
                |-----------------------|
                | item[1].Margin.Top    |
                |                       | <-- ItemMaxHeight
                | item[1].Margin.Bottom |
                |-----------------------|
                | this.Margin.Bottom    |
                ------------------------- */

            var sumOfAllMarginsAndClosedItemHeights = Margin.Top;
            foreach (var item in allItems)
            {
                // force recalculation of ActualHeight
                item.Height = double.NaN;
                item.UpdateLayout();

                sumOfAllMarginsAndClosedItemHeights += item.Margin.Top;
                if (item.Tag != SelectedItem)
                {
                    sumOfAllMarginsAndClosedItemHeights += item.ActualHeight;
                }
                sumOfAllMarginsAndClosedItemHeights += item.Margin.Bottom;
            }
            sumOfAllMarginsAndClosedItemHeights += Margin.Bottom;

            return ActualHeight - sumOfAllMarginsAndClosedItemHeights;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new AufklappableItem();
        }
    }
}