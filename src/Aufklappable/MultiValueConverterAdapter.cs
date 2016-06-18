/*
    <copyright file="MultiValueConverterAdapter.cs">
        Copyright (c) 2016 stroeb
    
        Licensed under Microsoft Public License (Ms-PL)
        http://opensource.org/licenses/MS-PL
    </copyright>
    <author>stroeb</author>
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Aufklappable
{
    /// <summary>
    ///     <see cref="IMultiValueConverter" /> which delegates conversions to the actual <see cref="IValueConverter" /> using one of the values as
    ///     <see cref="Binding.ConverterParameter" />.
    ///     <para>Used by <see cref="ConverterBindableBinding" />.</para>
    /// </summary>
    [ContentProperty("Converter")]
    internal class MultiValueConverterAdapter : IMultiValueConverter
    {
        private object _lastParameter;
        public IValueConverter Converter { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Gets only ever called from WPF")]
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter == null)
            {
                return values[0]; // Required for VS design-time
            }
            if (values.Length > 1)
            {
                _lastParameter = values[1];
            }
            return Converter.Convert(values[0], targetType, _lastParameter, culture);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Gets only ever called from WPF")]
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return Converter == null ? new[] { value } : new[] { Converter.ConvertBack(value, targetTypes[0], _lastParameter, culture) };
        }
    }
}