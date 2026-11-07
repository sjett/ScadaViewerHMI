// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public class HMIExtendedNumericDisplay1Serializer : HMIIOControlBaseSerializer
	{
		public HMIExtendedNumericDisplay1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIExtendedNumericDisplay1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIExtendedNumericDisplay1ViewModel node = target as HMIExtendedNumericDisplay1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Precision = ((int?)properties["Precision"]) ?? node.Precision;

            node.UnitSignalName = ((string?)properties["UnitSignalName"]) ?? node.UnitSignalName;

            node.Unit1 = ((string?)properties["Unit1"]) ?? node.Unit1;

            node.Unit2 = ((string?)properties["Unit2"]) ?? node.Unit2;

            node.Unit1ScaleFactor = ((double?)properties["Unit1ScaleFactor"]) ?? node.Unit1ScaleFactor;

            node.Unit2ScaleFactor = ((double?)properties["Unit2ScaleFactor"]) ?? node.Unit2ScaleFactor;

            node.HorizontalContentAlignment = properties["HorizontalContentAlignment"]?.ToObject<HorizontalAlignment?>() ?? node.HorizontalContentAlignment;

            node.VerticalContentAlignment = properties["VerticalContentAlignment"]?.ToObject<VerticalAlignment?>() ?? node.VerticalContentAlignment;

			if (((string?)properties["Foreground"]) != null)
				node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIExtendedNumericDisplay1ViewModel wFExtendedNumericDisplay1ViewModel = source as HMIExtendedNumericDisplay1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFExtendedNumericDisplay1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Precision", wFExtendedNumericDisplay1ViewModel.Precision),
				new JProperty("UnitSignalName", wFExtendedNumericDisplay1ViewModel.UnitSignalName),
				new JProperty("Unit1", wFExtendedNumericDisplay1ViewModel.Unit1),
				new JProperty("Unit2", wFExtendedNumericDisplay1ViewModel.Unit2),
				new JProperty("Unit1ScaleFactor", wFExtendedNumericDisplay1ViewModel.Unit1ScaleFactor),
				new JProperty("Unit2ScaleFactor", wFExtendedNumericDisplay1ViewModel.Unit2ScaleFactor),
				new JProperty("HorizontalContentAlignment", wFExtendedNumericDisplay1ViewModel.HorizontalContentAlignment),
				new JProperty("VerticalContentAlignment", wFExtendedNumericDisplay1ViewModel.VerticalContentAlignment),
				new JProperty("Foreground", wFExtendedNumericDisplay1ViewModel.Foreground?.ToString())
			});
		}
	}
}
