// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public abstract class HMIAnimatedPrimitiveBase : HMIStaticStatesControlBase
{
    public new static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
        nameof(LineThickness), typeof(Thickness), typeof(HMIAnimatedPrimitiveBase), new PropertyMetadata(new Thickness(1)));
    public static readonly DependencyProperty AnimatedFillProperty = DependencyProperty.Register(
        nameof(AnimatedFill), typeof(Brush), typeof(HMIAnimatedPrimitiveBase), new PropertyMetadata(Brushes.Red));
    public static readonly DependencyProperty RangeMinProperty = DependencyProperty.Register(
        nameof(RangeMin), typeof(double), typeof(HMIAnimatedPrimitiveBase), new PropertyMetadata(0d));
    public static readonly DependencyProperty RangeMaxProperty = DependencyProperty.Register(
        nameof(RangeMax), typeof(double), typeof(HMIAnimatedPrimitiveBase), new PropertyMetadata(100d));
    public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register(
        nameof(SignalName), typeof(string), typeof(HMIAnimatedPrimitiveBase),
        new PropertyMetadata(string.Empty, OnSignalNameChanged));
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(HMIAnimatedPrimitiveBase), new PropertyMetadata(new CornerRadius()));
    public static readonly DependencyProperty FillLevelProperty = DependencyProperty.Register(
        nameof(FillLevel), typeof(double), typeof(HMIAnimatedPrimitiveBase), new PropertyMetadata(0d));

    private OnSignalChangedDelegate? _signalChanged;
    private string? _registeredSignal;

    protected HMIAnimatedPrimitiveBase()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public new Thickness LineThickness { get => (Thickness)GetValue(LineThicknessProperty); set => SetValue(LineThicknessProperty, value); }
    public Brush AnimatedFill { get => (Brush)GetValue(AnimatedFillProperty); set => SetValue(AnimatedFillProperty, value); }
    public double RangeMin { get => (double)GetValue(RangeMinProperty); set => SetValue(RangeMinProperty, value); }
    public double RangeMax { get => (double)GetValue(RangeMaxProperty); set => SetValue(RangeMaxProperty, value); }
    public string SignalName { get => (string)GetValue(SignalNameProperty); set => SetValue(SignalNameProperty, value); }
    public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
    public double FillLevel { get => (double)GetValue(FillLevelProperty); private set => SetValue(FillLevelProperty, value); }

    private void OnLoaded(object sender, RoutedEventArgs args) => RegisterSignal();

    private void OnUnloaded(object sender, RoutedEventArgs args) => UnregisterSignal();

    private static void OnSignalNameChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        HMIAnimatedPrimitiveBase control = (HMIAnimatedPrimitiveBase)target;
        if (control.IsLoaded)
        {
            control.RegisterSignal();
        }
    }

    private void RegisterSignal()
    {
        UnregisterSignal();
        if (IsInDesignMode || string.IsNullOrWhiteSpace(SignalName))
        {
            return;
        }

        _registeredSignal = StringPlaceholdersResolver.Resolve(SignalPrefix + SignalName, this);
        _signalChanged = OnSignalChanged;
        Connector.RegisterSignalChangedHandler(string.Empty, _registeredSignal, _signalChanged);
        Connector.RegisterSignal(string.Empty, _registeredSignal);
        object value = Connector.GetSignalValue(string.Empty, _registeredSignal);
        OnSignalChanged(_registeredSignal, value);
    }

    private void UnregisterSignal()
    {
        if (_registeredSignal is null || _signalChanged is null)
        {
            return;
        }

        Connector.UnregisterSignalChangedHandler(string.Empty, _registeredSignal, _signalChanged);
        Connector.UnregisterSignal(string.Empty, _registeredSignal);
        _registeredSignal = null;
        _signalChanged = null;
    }

    private void OnSignalChanged(string signalName, object value)
    {
        double numericValue;
        try
        {
            numericValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
        {
            return;
        }

        if (!double.IsFinite(numericValue) || !double.IsFinite(RangeMin) || !double.IsFinite(RangeMax) || RangeMax <= RangeMin)
        {
            FillLevel = 0;
            return;
        }

        FillLevel = Math.Clamp((numericValue - RangeMin) / (RangeMax - RangeMin), 0, 1);
    }

    protected override void DoDispose()
    {
        UnregisterSignal();
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        base.DoDispose();
    }
}

public sealed class FillLevelToMaskOffsetConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is double fillLevel ? 1 - Math.Clamp(fillLevel, 0, 1) : 1d;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
