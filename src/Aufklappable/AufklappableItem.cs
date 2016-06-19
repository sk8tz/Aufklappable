/*
    <copyright file="AufklappableItem.cs">
        Copyright (c) 2016 stroeb
    
        Licensed under Microsoft Public License (Ms-PL)
        http://opensource.org/licenses/MS-PL
    </copyright>
    <author>stroeb</author>
*/

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace Aufklappable
{
    /// <summary>
    ///     Represents a single item in an <see cref="AufklappableContainer" />.
    /// </summary>
    [DebuggerDisplay("Tag = {Tag}, Title = {Title}")]
    public class AufklappableItem : ContentControl
    {
        /// <summary>
        ///     Identifies the <see cref="Title" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(AufklappableItem),
            new PropertyMetadata(default(string)));

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Static constructor is needed for style override")]
        static AufklappableItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AufklappableItem), new FrameworkPropertyMetadata(typeof(AufklappableItem)));
        }

        public AufklappableItem()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        ///     Gets or sets the text shown in the header of this item.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        ///     Identifies this item in the parent <see cref="AufklappableContainer" />.
        /// </summary>
        public new string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Tag == null)
            {
                throw new InvalidOperationException("Tag must be set.");
            }
        }
    }
}