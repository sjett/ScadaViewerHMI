// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
using HMI.MathExpressions.Compilation;
namespace HMI.Runtime.Shared
{
    /// <summary>

    /// </summary>
    public abstract class HMIStateConditionBase : DependencyObject, IDisposable
	{
		private IHMIConnector connector;
		private OnSignalChangedDelegate delegateSignalValueChanged;
		private ExpressionCompiler eqCompiler;
		private Dictionary<string, HMINumericSignal> dictCompilerParameters;
		private Dictionary<string, HMINumericSignal> dictSignals;
		
		public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(string), typeof(HMIStateConditionBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnConditionPropertyChanged)));
		public static readonly DependencyProperty ConditionTypeProperty = DependencyProperty.Register("ConditionType", typeof(HMIStateConditionType), typeof(HMIStateConditionBase), new PropertyMetadata(HMIStateConditionType.SignalName, new PropertyChangedCallback(OnConditionTypePropertyChanged)));
		public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register("StateName", typeof(string), typeof(HMIStateConditionBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnStateNamePropertyChanged)));
		
		public event EventHandler StateChanged;

        /// <summary>

        /// </summary>
        [Category("HMI")]
		public string Condition
		{
			get
			{
				return (string)GetValue(ConditionProperty);
			}
			set
			{
                SetValue(ConditionProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Browsable(false)]
		public string ResolvedCondition
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public List<HMIStateConditionParameter> ConditionParameters
		{
			get;
			private set;
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public HMIStateConditionType ConditionType
		{
			get
			{
				return (HMIStateConditionType)GetValue(ConditionTypeProperty);
			}
			set
			{
                SetValue(ConditionTypeProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public string StateName
		{
			get
			{
				return (string)GetValue(StateNameProperty);
			}
			set
			{
                SetValue(StateNameProperty, value);
			}
		}
		private static void OnConditionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIStateConditionBase()
		{
            dictCompilerParameters = new Dictionary<string, HMINumericSignal>(StringComparer.InvariantCultureIgnoreCase);
            dictSignals = new Dictionary<string, HMINumericSignal>(StringComparer.InvariantCultureIgnoreCase);
            ConditionParameters = new List<HMIStateConditionParameter>();
		}
		~HMIStateConditionBase()
		{
            Dispose(false);
		}
		public void Dispose()
		{
            Dispose(true);
			GC.SuppressFinalize(this);
		}
        /// <summary>

        /// </summary>
        private void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			if (dictSignals.ContainsKey(signalName))
			{
                ProcessEquationType(GetSignal(signalName, signalValue));
                OnStateChanged();
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="signalName"></param>
        /// <param name="signalValue"></param>
        /// <returns></returns>
        private HMINumericSignal GetSignal(string signalName, object signalValue)
		{
			HMINumericSignal wFNumericSignal = dictSignals[signalName];
			wFNumericSignal.Value = HMINumericSignal.ConvertSignalValueToDouble(signalValue);
			return wFNumericSignal;
		}
        /// <summary>

        /// </summary>
        /// <param name="signal"></param>
        private void ProcessEquationType(HMINumericSignal signal)
		{
			if (IsEquationType())
			{
				using (Dictionary<string, HMINumericSignal>.KeyCollection.Enumerator enumerator = dictCompilerParameters.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						signal = GetSignalAndVariable(current);
					}
				}
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        private HMINumericSignal GetSignalAndVariable(string paramName)
		{
			HMINumericSignal wFNumericSignal = dictCompilerParameters[paramName];
            eqCompiler.SetVariable(paramName, wFNumericSignal.Value);
			return wFNumericSignal;
		}
		public override string ToString()
		{
			return StateName;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && HasConnector() && delegateSignalValueChanged != null)
			{
                UnregiserSignals();
			}
		}
        /// <summary>

        /// </summary>
        private void UnregiserSignals()
		{
			using (Dictionary<string, HMINumericSignal>.KeyCollection.Enumerator enumerator = dictSignals.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
                    UnregisterSignal(string.Empty, current, delegateSignalValueChanged);
				}
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="connector"></param>
        public virtual void Initialize(IHMIConnector connector)
		{
			if (!HasConnector())
			{
                ProcessConnector(connector);
			}
		}
		private bool HasConnector()
		{
			return connector != null;
		}
        /// <summary>

        /// </summary>
        private void ProcessConnector(IHMIConnector connector)
		{
			this.connector = connector;
            TrimCondition();
			if (HasCondition())
			{
                ProcessEquation();
                ProcessSignals();
			}
		}
		private bool HasCondition()
		{
			return !string.IsNullOrEmpty(Condition);
		}
        /// <summary>

        /// </summary>
        private void ProcessEquation()
		{
			if (IsEquationType())
			{
				if (Compiled())
				{
                    ProcessConditionParameters();
					return;
				}
			}
			else
			{
                AddSignalToDictionary();
			}
		}
        /// <summary>

        /// </summary>
        private void ProcessSignals()
		{
			if (HasSignals())
			{
                AddDelegateAndRegisterSignal();
			}
		}
		private bool HasSignals()
		{
			return dictSignals.Count > 0;
		}
        /// <summary>

        /// </summary>
        private void AddDelegateAndRegisterSignal()
		{
            delegateSignalValueChanged = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
			using (Dictionary<string, HMINumericSignal>.KeyCollection.Enumerator enumerator = dictSignals.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
                    RegisterSignal(string.Empty, current, delegateSignalValueChanged);
				}
			}
		}
        /// <summary>

        /// </summary>
        private void TrimCondition()
		{
            ResolvedCondition = (ResolvedCondition ?? Condition).Trim();
		}
        /// <summary>

        /// </summary>
        /// <returns></returns>
        private bool Compiled()
		{
            eqCompiler = new ExpressionCompiler(ResolvedCondition, true);
			bool result;
			try
			{
                eqCompiler.Compile();
				result = true;
			}
			catch (Exception ex)
			{
                eqCompiler = null;
                connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", GetType().Name, ex.Message), new object[0]);
				result = false;
			}
			return result;
		}
        /// <summary>

        /// </summary>
        private void ProcessConditionParameters()
		{
			using (List<HMIStateConditionParameter>.Enumerator enumerator = ConditionParameters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HMIStateConditionParameter current = enumerator.Current;
                    CompileParameters(current);
				}
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="stateConditionParameter"></param>
        private void CompileParameters(HMIStateConditionParameter stateConditionParameter)
		{
			if (!ShouldContinue(stateConditionParameter))
			{
                dictCompilerParameters.Add(stateConditionParameter.ParameterName, SetOrCreateSignal(stateConditionParameter));
			}
		}
        /// <summary>

        /// </summary>
        private void AddSignalToDictionary()
		{
			HMINumericSignal wFNumericSignal = new HMINumericSignal(ResolvedCondition);
            dictSignals.Add(wFNumericSignal.Name, wFNumericSignal);
		}
        /// <summary>

        /// </summary>
        /// <param name="stateConditionParameter"></param>
        /// <returns></returns>
        private HMINumericSignal SetOrCreateSignal(HMIStateConditionParameter stateConditionParameter)
		{
			HMINumericSignal result;
			if (DictinaryContainsSignal(stateConditionParameter))
			{
				result = SetSignal(stateConditionParameter);
			}
			else
			{
				result = CreateSignal(stateConditionParameter);
			}
			return result;
		}
        /// <summary>

        /// </summary>
        /// <param name="stateConditionParameter"></param>
        /// <returns></returns>
        private bool ShouldContinue(HMIStateConditionParameter stateConditionParameter)
		{
			return DoesntHaveParameterOrSignal(stateConditionParameter) || IsDuplicate(stateConditionParameter);
		}
		private HMINumericSignal CreateSignal(HMIStateConditionParameter stateConditionParameter)
		{
			HMINumericSignal wFNumericSignal = new HMINumericSignal(stateConditionParameter.ResolvedSignalName);
            dictSignals.Add(wFNumericSignal.Name, wFNumericSignal);
			return wFNumericSignal;
		}
		private HMINumericSignal SetSignal(HMIStateConditionParameter stateConditionParameter)
		{
			return dictSignals[stateConditionParameter.ResolvedSignalName];
		}
		private bool DictinaryContainsSignal(HMIStateConditionParameter stateConditionParameter)
		{
			return dictSignals.ContainsKey(stateConditionParameter.ResolvedSignalName);
		}
        /// <summary>

        /// </summary>
        /// <param name="stateConditionParameter"></param>
        /// <returns></returns>
        private static bool DoesntHaveParameterOrSignal(HMIStateConditionParameter stateConditionParameter)
		{
			return string.IsNullOrEmpty(stateConditionParameter.ParameterName) || string.IsNullOrEmpty(stateConditionParameter.SignalName);
		}
        /// <summary>

        /// </summary>
        /// <param name="stateConditionParameter"></param>
        /// <returns></returns>
        private bool IsDuplicate(HMIStateConditionParameter stateConditionParameter)
		{
			return dictCompilerParameters.ContainsKey(stateConditionParameter.ParameterName);
		}
        /// <summary>



        /// </summary>
        /// <returns></returns>
        public virtual bool IsSatisfied()
		{
			bool result = false;
			double conditionResultValue = double.NaN;
            SetConditionValue(ref conditionResultValue);
            SetResultValue(ref result, conditionResultValue);
			return result;
		}
		private static void SetResultValue(ref bool resultValue, double conditionResultValue)
		{
			if (HasResultAndValue(conditionResultValue))
			{
				resultValue = true;
			}
		}
		private void SetConditionValue(ref double conditionResultValue)
		{
			if (IsEquationType())
			{
                GetConditionValue(ref conditionResultValue);
				return;
			}
            SetConditionResult(ref conditionResultValue);
		}
		private void GetConditionValue(ref double conditionResultValue)
		{
			if (eqCompiler != null)
			{
				conditionResultValue = TryToComplie();
			}
		}
        /// <summary>

        /// </summary>
        /// <returns></returns>
        private bool IsEquationType()
		{
			return ConditionType == HMIStateConditionType.Equation;
		}
        /// <summary>

        /// </summary>
        /// <returns></returns>
        private double TryToComplie()
		{
			double result;
			try
			{
				result = eqCompiler.Evaluate();
			}
			catch (Exception ex)
			{
				result = double.NaN;
                connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", GetType().Name, ex.Message), new object[0]);
			}
			return result;
		}
		private void SetConditionResult(ref double conditionResultValue)
		{
			if (HasSignals())
			{
				conditionResultValue = dictSignals.Values.First().Value;
			}
		}
		private static bool HasResultAndValue(double conditionResultValue)
		{
			return !double.IsNaN(conditionResultValue) && conditionResultValue != 0.0;
		}
        /// <summary>

        /// </summary>
        protected virtual void OnStateChanged()
		{
			EventHandler stateChanged = StateChanged;
			if (stateChanged != null)
			{
				stateChanged.Invoke(this, EventArgs.Empty);
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="signalName"></param>
        /// <param name="signalChangedDelegate"></param>
        protected void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
                connector.RegisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
                connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", GetType().Name, ex.Message), new object[0]);
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="signalName"></param>
        /// <param name="signalChangedDelegate"></param>
        protected void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
                connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
                connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", GetType().Name, ex.Message), new object[0]);
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="owner"></param>
        public void ResolvePlaceholders(HMIStatesControlBase owner)
		{
			if (ShouldResolvePlaceholders())
			{
                DoResolvePlaceholders(owner);
			}
		}
		private bool ShouldResolvePlaceholders()
		{
			return Condition != null && Condition.Trim().Length > 0;
		}
		private void DoResolvePlaceholders(HMIStatesControlBase owner)
		{
			if (ConditionType == HMIStateConditionType.Equation)
			{
                ResolveParametersPlaceholders(owner);
				return;
			}
            ResolveConditionPlaceholders(owner);
		}
		private void ResolveParametersPlaceholders(HMIStatesControlBase owner)
		{
			using (List<HMIStateConditionParameter>.Enumerator enumerator = ConditionParameters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HMIStateConditionParameter current = enumerator.Current;
					current.ResolvePlaceholders(owner);
				}
			}
		}
		private void ResolveConditionPlaceholders(HMIStatesControlBase owner)
		{
            ResolvedCondition = (owner.SignalPrefix + Condition).Trim();
            ResolvedCondition = StringPlaceholdersResolver.Resolve(ResolvedCondition, owner);
		}
	}
}
