/*
    <copyright file="ExpanderToBooleanConverter.cs">
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

using JetBrains.Annotations;

namespace Aufklappable
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via XAML")]
    [UsedImplicitly]
    internal class ExpanderToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? parameter : null;
        }
    }
}