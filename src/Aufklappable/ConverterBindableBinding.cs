/*
    <copyright file="ConverterBindableBinding.cs">
        Copyright (c) 2016 stroeb
    
        Licensed under Microsoft Public License (Ms-PL)
        http://opensource.org/licenses/MS-PL
    </copyright>
    <author>stroeb</author>
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;
using System.Windows.Markup;

using JetBrains.Annotations;

namespace Aufklappable
{
    /// <summary>
    ///     Can be used in XAML like a <see cref="System.Windows.Data.Binding" /> where the <see cref="System.Windows.Data.Binding.ConverterParameter" /> is itself
    ///     bindable.
    ///     <para>Uses a <see cref="MultiBinding" /> and <see cref="MultiValueConverterAdapter" /> internally to implement this functionality.</para>
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via XAML")]
    [UsedImplicitly]
    internal class ConverterBindableBinding : MarkupExtension
    {
        public Binding Binding { get; set; }
        public IValueConverter Converter { get; set; }
        public Binding ConverterParameterBinding { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(Binding);
            multiBinding.Bindings.Add(ConverterParameterBinding);
            multiBinding.Converter = new MultiValueConverterAdapter { Converter = Converter };

            return multiBinding.ProvideValue(serviceProvider);
        }
    }
}