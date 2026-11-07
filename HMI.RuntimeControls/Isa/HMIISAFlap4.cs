// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFlap4 : HMIISABase
{
    public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register(
        nameof(SignalName),
        typeof(string),
        typeof(HMIISAFlap4),
        new PropertyMetadata(string.Empty, OnSignalConfigurationChanged));

    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
        nameof(MinValue),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(0d, OnSignalConfigurationChanged));

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
        nameof(MaxValue),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(100d, OnSignalConfigurationChanged));

    public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register(
        nameof(MinAngle),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(0d, OnSignalConfigurationChanged));

    public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register(
        nameof(MaxAngle),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(180d, OnSignalConfigurationChanged));

    public static readonly DependencyProperty OffsetAngleProperty = DependencyProperty.Register(
        nameof(OffsetAngle),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(0d, OnOffsetAngleChanged));

    private static readonly DependencyPropertyKey RotationAnglePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(RotationAngle),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(0d, OnRotationAngleChanged));

    public static readonly DependencyProperty RotationAngleProperty = RotationAnglePropertyKey.DependencyProperty;

    public static readonly DependencyProperty ActualRotationAngleProperty = DependencyProperty.Register(
        nameof(ActualRotationAngle),
        typeof(double),
        typeof(HMIISAFlap4),
        new PropertyMetadata(0d));

    private IHMIConnector? connector;
    private OnSignalChangedDelegate? signalChangedHandler;
    private string? registeredSignalName;
    private double? lastSignalValue;
    private bool isLoaded;

    static HMIISAFlap4()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFlap4),
            new FrameworkPropertyMetadata(typeof(HMIISAFlap4)));
    }

    public HMIISAFlap4()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    [Category("IBMS")]
    public string SignalName
    {
        get => (string)GetValue(SignalNameProperty);
        set => SetValue(SignalNameProperty, value);
    }

    [Category("IBMS")]
    public double MinValue
    {
        get => (double)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    [Category("IBMS")]
    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    [Category("IBMS")]
    public double MinAngle
    {
        get => (double)GetValue(MinAngleProperty);
        set => SetValue(MinAngleProperty, value);
    }

    [Category("IBMS")]
    public double MaxAngle
    {
        get => (double)GetValue(MaxAngleProperty);
        set => SetValue(MaxAngleProperty, value);
    }

    [Category("IBMS")]
    public double OffsetAngle
    {
        get => (double)GetValue(OffsetAngleProperty);
        set => SetValue(OffsetAngleProperty, value);
    }

    [Browsable(false)]
    public double RotationAngle => (double)GetValue(RotationAngleProperty);

    [Browsable(false)]
    public double ActualRotationAngle => (double)GetValue(ActualRotationAngleProperty);

    private static void OnSignalConfigurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        HMIISAFlap4 control = (HMIISAFlap4)dependencyObject;
        if (args.Property == SignalNameProperty)
        {
            control.lastSignalValue = null;
            control.RegisterSignal();
        }

        control.UpdateRotationFromLastValue();
    }

    private static void OnOffsetAngleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) =>
        ((HMIISAFlap4)dependencyObject).UpdateActualRotationAngle();

    private static void OnRotationAngleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) =>
        ((HMIISAFlap4)dependencyObject).UpdateActualRotationAngle();

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        if (isLoaded || DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        isLoaded = true;
        RegisterSignal();
    }

    private void OnUnloaded(object sender, RoutedEventArgs args)
    {
        isLoaded = false;
        UnregisterSignal();
    }

    private void RegisterSignal()
    {
        UnregisterSignal();
        if (!isLoaded || string.IsNullOrWhiteSpace(SignalName))
        {
            return;
        }

        connector = RuntimeServices.GetRequiredService<IHMIConnector>();
        registeredSignalName = StringPlaceholdersResolver.Resolve(SignalPrefix + SignalName, this);
        signalChangedHandler ??= OnSignalChanged;
        connector.RegisterSignalChangedHandler(string.Empty, registeredSignalName, signalChangedHandler);
    }

    private void UnregisterSignal()
    {
        if (connector is not null && signalChangedHandler is not null && registeredSignalName is not null)
        {
            connector.UnregisterSignalChangedHandler(string.Empty, registeredSignalName, signalChangedHandler);
        }

        registeredSignalName = null;
        connector = null;
    }

    private void OnSignalChanged(string signalName, object value)
    {
        try
        {
            double numericValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            if (!double.IsFinite(numericValue))
            {
                return;
            }

            lastSignalValue = numericValue;
            UpdateRotationFromLastValue();
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Warning,
                "Ignoring non-numeric ISA flap value for '{0}': {1}",
                signalName,
                exception.Message);
        }
    }

    private void UpdateRotationFromLastValue()
    {
        if (lastSignalValue is not double value)
        {
            return;
        }

        if (!double.IsFinite(MinValue) || !double.IsFinite(MaxValue) ||
            !double.IsFinite(MinAngle) || !double.IsFinite(MaxAngle) ||
            MaxValue == MinValue)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Warning,
                "Ignoring ISA flap value because its value/angle range is invalid.");
            return;
        }

        double rotation = MinAngle + (MaxAngle - MinAngle) * (value - MinValue) / (MaxValue - MinValue);
        SetValue(RotationAnglePropertyKey, rotation);
    }

    private void UpdateActualRotationAngle() =>
        SetCurrentValue(ActualRotationAngleProperty, RotationAngle + OffsetAngle);
}
