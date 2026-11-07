// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;

namespace HMI.RuntimeControls.Sliders;

public sealed class HMISlider1 : Slider, IDisposable
{
    public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register(
        nameof(ServerName), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register(
        nameof(SignalPrefix), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty StateSignalNameProperty = DependencyProperty.Register(
        nameof(StateSignalName), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty, OnSignalPropertyChanged));
    public static readonly DependencyProperty WriteSignalNameProperty = DependencyProperty.Register(
        nameof(WriteSignalName), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register(
        nameof(EnableSignalName), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty, OnSignalPropertyChanged));
    public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register(
        nameof(EnableMask), typeof(int?), typeof(HMISlider1), new PropertyMetadata(null));
    public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register(
        nameof(VisibilitySignalName), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty, OnSignalPropertyChanged));
    public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register(
        nameof(VisibilityMask), typeof(int?), typeof(HMISlider1), new PropertyMetadata(null));
    public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register(
        nameof(Precision), typeof(int), typeof(HMISlider1), new PropertyMetadata(0));
    public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
        nameof(Scale), typeof(bool), typeof(HMISlider1), new PropertyMetadata(false));
    public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register(
        nameof(SecurityAuthorizationName), typeof(string), typeof(HMISlider1), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register(
        nameof(SecurityDenyAccessBehaviour), typeof(DenyAccessBehaviour), typeof(HMISlider1),
        new PropertyMetadata(DenyAccessBehaviour.Hide));

    private IHMIConnector? _connector;
    private OnSignalChangedDelegate? _stateChanged;
    private OnSignalChangedDelegate? _enableChanged;
    private OnSignalChangedDelegate? _visibilityChanged;
    private string? _registeredStateSignal;
    private string? _registeredEnableSignal;
    private string? _registeredVisibilitySignal;
    private bool _updatingFromSignal;
    private bool _disposed;
    private bool _isAuthorized = true;
    private bool _isAllowedByEnableSignal = true;
    private bool _isAllowedByVisibilitySignal = true;
    private readonly DependencyPropertyDescriptor? _signalPrefixDescriptor;

    static HMISlider1() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(HMISlider1), new FrameworkPropertyMetadata(typeof(HMISlider1)));

    public HMISlider1()
    {
        _signalPrefixDescriptor = DependencyPropertyDescriptor.FromProperty(SignalPrefixProperty, typeof(HMISlider1));
        _signalPrefixDescriptor?.AddValueChanged(this, OnSignalPrefixChanged);
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        ValueChanged += OnValueChanged;
    }

    public string ServerName { get => (string)GetValue(ServerNameProperty); set => SetValue(ServerNameProperty, value); }
    public string SignalPrefix { get => (string)GetValue(SignalPrefixProperty); set => SetValue(SignalPrefixProperty, value); }
    public string StateSignalName { get => (string)GetValue(StateSignalNameProperty); set => SetValue(StateSignalNameProperty, value); }
    public string WriteSignalName { get => (string)GetValue(WriteSignalNameProperty); set => SetValue(WriteSignalNameProperty, value); }
    public string EnableSignalName { get => (string)GetValue(EnableSignalNameProperty); set => SetValue(EnableSignalNameProperty, value); }
    public int? EnableMask { get => (int?)GetValue(EnableMaskProperty); set => SetValue(EnableMaskProperty, value); }
    public string VisibilitySignalName { get => (string)GetValue(VisibilitySignalNameProperty); set => SetValue(VisibilitySignalNameProperty, value); }
    public int? VisibilityMask { get => (int?)GetValue(VisibilityMaskProperty); set => SetValue(VisibilityMaskProperty, value); }
    public int Precision { get => (int)GetValue(PrecisionProperty); set => SetValue(PrecisionProperty, value); }
    public bool Scale { get => (bool)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }
    public string SecurityAuthorizationName { get => (string)GetValue(SecurityAuthorizationNameProperty); set => SetValue(SecurityAuthorizationNameProperty, value); }
    public DenyAccessBehaviour SecurityDenyAccessBehaviour
    {
        get => (DenyAccessBehaviour)GetValue(SecurityDenyAccessBehaviourProperty);
        set => SetValue(SecurityDenyAccessBehaviourProperty, value);
    }

    private static void OnSignalPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        HMISlider1 slider = (HMISlider1)target;
        if (slider.IsLoaded)
        {
            slider.RefreshSubscriptions();
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
        RefreshSubscriptions();
        ApplyAuthorization();
    }

    private void OnUnloaded(object sender, RoutedEventArgs args) => UnregisterSignals();
    private void OnSignalPrefixChanged(object? sender, EventArgs args)
    {
        if (IsLoaded)
        {
            RefreshSubscriptions();
        }
    }

    private void RefreshSubscriptions()
    {
        UnregisterSignals();
        _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
        Register(StateSignalName, ref _stateChanged, ref _registeredStateSignal, OnStateChanged);
        Register(EnableSignalName, ref _enableChanged, ref _registeredEnableSignal, OnEnableChanged);
        Register(VisibilitySignalName, ref _visibilityChanged, ref _registeredVisibilitySignal, OnVisibilityChanged);
    }

    private void Register(
        string signalName,
        ref OnSignalChangedDelegate? handler,
        ref string? registeredSignal,
        OnSignalChangedDelegate callback)
    {
        if (string.IsNullOrWhiteSpace(signalName))
        {
            return;
        }

        string resolvedName = StringPlaceholdersResolver.Resolve(SignalPrefix + signalName, this);
        handler = callback;
        registeredSignal = resolvedName;
        _connector!.RegisterSignalChangedHandler(ServerName, resolvedName, handler);
        _connector.RegisterSignal(ServerName, resolvedName);
        object currentValue = _connector.GetSignalValue(ServerName, resolvedName);
        if (currentValue is not null)
        {
            handler(resolvedName, currentValue);
        }
    }

    private void UnregisterSignals()
    {
        if (_connector is null)
        {
            return;
        }

        Unregister(ref _stateChanged, ref _registeredStateSignal);
        Unregister(ref _enableChanged, ref _registeredEnableSignal);
        Unregister(ref _visibilityChanged, ref _registeredVisibilitySignal);
    }

    private void Unregister(ref OnSignalChangedDelegate? handler, ref string? registeredSignal)
    {
        if (handler is null)
        {
            return;
        }

        _connector!.UnregisterSignalChangedHandler(ServerName, registeredSignal!, handler);
        _connector.UnregisterSignal(ServerName, registeredSignal!);
        handler = null;
        registeredSignal = null;
    }

    private void OnStateChanged(string signalName, object value)
    {
        if (!TryConvert(value, out double numericValue) || !double.IsFinite(numericValue))
        {
            return;
        }

        _updatingFromSignal = true;
        try
        {
            Value = Math.Clamp(Math.Round(numericValue, Math.Clamp(Precision, 0, 15)), Minimum, Maximum);
        }
        finally
        {
            _updatingFromSignal = false;
        }
    }

    private void OnEnableChanged(string signalName, object value)
    {
        _isAllowedByEnableSignal = IsSignalAllowed(value, EnableMask);
        ApplyAuthorization();
    }

    private void OnVisibilityChanged(string signalName, object value)
    {
        _isAllowedByVisibilitySignal = IsSignalAllowed(value, VisibilityMask);
        UpdateEffectiveState();
    }

    private static bool IsSignalAllowed(object value, int? mask)
    {
        if (!TryConvert(value, out double numericValue) || !double.IsFinite(numericValue) || numericValue == 0)
        {
            return false;
        }
        return !mask.HasValue || (Convert.ToInt64(numericValue) & mask.Value) != mask.Value;
    }

    private static bool TryConvert(object value, out double numericValue)
    {
        try
        {
            numericValue = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
            return true;
        }
        catch (FormatException)
        {
            numericValue = 0;
            return false;
        }
        catch (InvalidCastException)
        {
            numericValue = 0;
            return false;
        }
        catch (OverflowException)
        {
            numericValue = 0;
            return false;
        }
    }

    private void ApplyAuthorization()
    {
        _isAuthorized = string.IsNullOrWhiteSpace(SecurityAuthorizationName) ||
            _connector!.CheckProjectAuthorization(SecurityAuthorizationName);
        UpdateEffectiveState();
    }

    private void UpdateEffectiveState()
    {
        bool deniedAndHidden = !_isAuthorized && SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide;
        Visibility = _isAllowedByVisibilitySignal && !deniedAndHidden ? Visibility.Visible : Visibility.Collapsed;
        IsEnabled = _isAllowedByEnableSignal && (_isAuthorized || SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable);
    }

    private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
    {
        if (_updatingFromSignal || !IsLoaded || (!IsMouseCaptureWithin && !IsKeyboardFocusWithin) ||
            string.IsNullOrWhiteSpace(WriteSignalName) || !IsEnabled)
        {
            return;
        }

        _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
        if (!string.IsNullOrWhiteSpace(SecurityAuthorizationName) &&
            !_connector.CheckProjectAuthorization(SecurityAuthorizationName))
        {
            ApplyAuthorization();
            return;
        }

        _connector.WriteSignal(ServerName, StringPlaceholdersResolver.Resolve(SignalPrefix + WriteSignalName, this),
            Math.Round(args.NewValue, Math.Clamp(Precision, 0, 15)));
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        UnregisterSignals();
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        ValueChanged -= OnValueChanged;
        _signalPrefixDescriptor?.RemoveValueChanged(this, OnSignalPrefixChanged);
        _disposed = true;
    }
}
