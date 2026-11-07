// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;

namespace HMI.ProjectDesigner.DesignControls.Charts.ViewModels;

public enum HMIRealtimeChartDenyAccessBehaviour
{
    Disable,
    Hide
}

public enum HMIRealtimeChartType
{
    Bar,
    Column,
    Line,
    Pie
}

public sealed class HMIRealtimeChartViewModel : CommonControlViewModel
{
    private HMIRealtimeChartType chartType = HMIRealtimeChartType.Bar;
    private double interval = 5;
    private double minimum;
    private double maximum = 100;
    private string serverName = string.Empty;
    private string signalsNames = string.Empty;
    private string signalsDescriptions = string.Empty;
    private string signalPrefix = string.Empty;
    private string securityAuthorizationName = string.Empty;
    private HMIRealtimeChartDenyAccessBehaviour securityDenyAccessBehaviour = HMIRealtimeChartDenyAccessBehaviour.Hide;

    public HMIRealtimeChartViewModel(
        IServiceProvider container,
        IExplorerNodeViewModel parentValue,
        Type controlType)
        : base(container, parentValue, controlType)
    {
    }

    public override System.Windows.Size DesignerDesiredSize => new(430, 250);

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public HMIRealtimeChartType ChartType
    {
        get => chartType;
        set => SetValue(ref chartType, value, () => ChartType);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(0.000001)]
    public double Interval
    {
        get => interval;
        set => SetValue(ref interval, value, () => Interval);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public double Minimum
    {
        get => minimum;
        set => SetValue(ref minimum, value, () => Minimum);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public double Maximum
    {
        get => maximum;
        set => SetValue(ref maximum, value, () => Maximum);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public string ServerName
    {
        get => serverName;
        set => SetValue(ref serverName, value, () => ServerName);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public string SignalsNames
    {
        get => signalsNames;
        set => SetValue(ref signalsNames, value, () => SignalsNames);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public string SignalsDescriptions
    {
        get => signalsDescriptions;
        set => SetValue(ref signalsDescriptions, value, () => SignalsDescriptions);
    }

    [PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
    public string SignalPrefix
    {
        get => signalPrefix;
        set => SetValue(ref signalPrefix, value, () => SignalPrefix);
    }

    [PropertyCategory("Security", typeof(StandardPropertyCategories))]
    public string SecurityAuthorizationName
    {
        get => securityAuthorizationName;
        set => SetValue(ref securityAuthorizationName, value, () => SecurityAuthorizationName);
    }

    [PropertyCategory("Security", typeof(StandardPropertyCategories))]
    public HMIRealtimeChartDenyAccessBehaviour SecurityDenyAccessBehaviour
    {
        get => securityDenyAccessBehaviour;
        set => SetValue(ref securityDenyAccessBehaviour, value, () => SecurityDenyAccessBehaviour);
    }

    public override IEnumerable<string> GetNamespaces() =>
    [
        "HMICharts=\"clr-namespace:HMI.RuntimeControls.Charts;assembly=HMI.RuntimeControls\""
    ];

    public override string GetXamlControlName() => "HMIRealtimeChart";

    protected override void AddXamlAttributes()
    {
        base.AddXamlAttributes();
        AddXamlAttribute("ChartType", ChartType);
        AddXamlAttribute("Interval", Interval.ToString(System.Globalization.CultureInfo.InvariantCulture));
        AddXamlAttribute("Minimum", Minimum.ToString(System.Globalization.CultureInfo.InvariantCulture));
        AddXamlAttribute("Maximum", Maximum.ToString(System.Globalization.CultureInfo.InvariantCulture));
        AddXamlAttribute("ServerName", ServerName);
        AddXamlAttribute("SignalsNames", SignalsNames);
        AddXamlAttribute("SignalsDescriptions", SignalsDescriptions);
        AddXamlAttribute("SignalPrefix", SignalPrefix);
        AddXamlAttribute("SecurityAuthorizationName", SecurityAuthorizationName);
        AddXamlAttribute("SecurityDenyAccessBehaviour", SecurityDenyAccessBehaviour);
    }

    protected override void ShowHideProperties()
    {
        base.ShowHideProperties();
        IsBackgroundVisible = false;
        IsForegroundVisible = false;
    }

    private void SetValue<T>(
        ref T field,
        T value,
        System.Linq.Expressions.Expression<Func<T>> property)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        RaisePropertyChanging<T>(property);
        field = value;
        RaisePropertyChanged<T>(property);
    }
}
