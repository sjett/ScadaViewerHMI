// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Labels
{
    [TemplatePart(Name = "uxSignalName", Type = typeof(TextBlock))]
    public class HMISignalInformationLabel1 : Control, IDisposable
    {
        private const string DefaultServerName = "";
        private const string NullString = "NULL";
        private const string SignalNameTextBoxName = "uxSignalName";
        private const string InvalidSymbolicText = "HMISN_Ctrl_HMISignalInformationLabel1_Invalid";

        private TextBlock uxSignalName;
        private IHMIConnector connector;
        private HMISignalDefinition wfSignalDefinition;
        private string invalidTranslation = "HMISN_Ctrl_HMISignalInformationLabel1_Invalid";

        public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMISignalInformationLabel1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSignalNamePropertyChanged)));
        public static readonly DependencyProperty SignalInformationProperty = DependencyProperty.Register("SignalInformation", typeof(SignalInformationField), typeof(HMISignalInformationLabel1), new PropertyMetadata(SignalInformationField.Name, new PropertyChangedCallback(OnSignalInformationPropertyChanged)));
        public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMISignalInformationLabel1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMISignalInformationLabel1), new PropertyMetadata(string.Empty));
        
        public string SignalName
        {
            get
            {
                return (string)GetValue(SignalNameProperty);
            }
            set
            {
                SetValue(SignalNameProperty, value);
            }
        }
        public SignalInformationField SignalInformation
        {
            get
            {
                return (SignalInformationField)GetValue(SignalInformationProperty);
            }
            set
            {
                SetValue(SignalInformationProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalPrefix
        {
            get
            {
                return (string)GetValue(SignalPrefixProperty);
            }
            set
            {
                SetValue(SignalPrefixProperty, value);
            }
        }
        [Category("HMI")]
        public string ObjectID
        {
            get
            {
                return (string)GetValue(ObjectIDProperty);
            }
            set
            {
                SetValue(ObjectIDProperty, value);
            }
        }
        private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HMISignalInformationLabel1 wFSignalInformationLabel = d as HMISignalInformationLabel1;
            if (wFSignalInformationLabel != null && wFSignalInformationLabel.uxSignalName != null)
            {
                wFSignalInformationLabel.HandleSignalNameChange((string)e.OldValue, (string)e.NewValue, wFSignalInformationLabel.SignalInformation);
                return;
            }
        }
        private static void OnSignalInformationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HMISignalInformationLabel1 wFSignalInformationLabel = d as HMISignalInformationLabel1;
            if (wFSignalInformationLabel != null && wFSignalInformationLabel.uxSignalName != null)
            {
                wFSignalInformationLabel.HandleSignalInformationChange(wFSignalInformationLabel.SignalName, (SignalInformationField)e.NewValue);
                return;
            }
        }
        public HMISignalInformationLabel1()
        {
            DefaultStyleKey = GetType();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            uxSignalName = GetTemplateChild(SignalNameTextBoxName) as TextBlock;
            this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
            HandleSignalNameChange(SignalName, SignalName, SignalInformation);
            RegisterSymbolicTexts();
        }
        private void OnSignalInformationChangedHandler(string serverName, string[] signalNames, HMISignalDefinition[] wfSignalDefinitionProperties)
        {
            if (signalNames.First() != ResolveSignalName(PrefixSignalName(SignalName)))
            {
                return;
            }
            wfSignalDefinition = wfSignalDefinitionProperties.First();
            UpdateSignalInformation();
        }
        private void OnLanguageChangedDelegate(string[] symbolicNames, string[] translationTexts)
        {
            for (int i = 0; i < symbolicNames.Length; i++)
            {
                if (symbolicNames[i] == InvalidSymbolicText)
                {
                    invalidTranslation = translationTexts[i];
                }
            }
            UpdateSignalInformation();
        }
        private void UpdateSignalNameText(string signalName, SignalInformationField signalInformation)
        {
            uxSignalName.Text=signalName + ":" + signalInformation;
        }
        private void UpdateSignalInformation()
        {
            string text = invalidTranslation;
            if (wfSignalDefinition != null)
            {
                CultureInfo cultureInfo = CultureInfoCache.GetCultureInfo(connector.GetLanguageId());
                switch (SignalInformation)
                {
                    case SignalInformationField.SignalID:
                        text = wfSignalDefinition.SignalID.ToString();
                        break;
                    case SignalInformationField.Name:
                        text = wfSignalDefinition.AliasName;
                        break;
                    case SignalInformationField.ConnectorAddress:
                        text = wfSignalDefinition.Name;
                        break;
                    case SignalInformationField.Active:
                        text = wfSignalDefinition.Active.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Description:
                        text = wfSignalDefinition.Description;
                        break;
                    case SignalInformationField.DescriptionSymbolicText:
                        text = wfSignalDefinition.DescriptionSymbolicText;
                        break;
                    case SignalInformationField.LogUserActivity:
                        text = wfSignalDefinition.LogUserActivity.ToString(cultureInfo);
                        break;
                    case SignalInformationField.FactorX1:
                        text = wfSignalDefinition.FactorX1.ToString(cultureInfo);
                        break;
                    case SignalInformationField.FactorX2:
                        text = wfSignalDefinition.FactorX2.ToString(cultureInfo);
                        break;
                    case SignalInformationField.FactorY1:
                        text = wfSignalDefinition.FactorY1.ToString(cultureInfo);
                        break;
                    case SignalInformationField.FactorY2:
                        text = wfSignalDefinition.FactorY2.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Hysterese:
                        text = wfSignalDefinition.Hysterese.ToString(cultureInfo);
                        break;
                    case SignalInformationField.HystereseLog:
                        text = wfSignalDefinition.HystereseLog.ToString(cultureInfo);
                        break;
                    case SignalInformationField.HystereseAlarm:
                        text = wfSignalDefinition.HystereseAlarm.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Hysterese_2:
                        text = wfSignalDefinition.Hysterese_2.ToString(cultureInfo);
                        break;
                    case SignalInformationField.HystereseLog_2:
                        text = wfSignalDefinition.HystereseLog_2.ToString(cultureInfo);
                        break;
                    case SignalInformationField.HystereseAlarm_2:
                        text = wfSignalDefinition.HystereseAlarm_2.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Maximum:
                        text = wfSignalDefinition.Maximum.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Minimum:
                        text = wfSignalDefinition.Minimum.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Unit:
                        text = wfSignalDefinition.Unit;
                        break;
                    case SignalInformationField.VChannel:
                        text = wfSignalDefinition.VChannel.ToString(cultureInfo);
                        break;
                    case SignalInformationField.VChannelTypeID:
                        text = wfSignalDefinition.VChannelTypeID.ToString(cultureInfo);
                        break;
                    case SignalInformationField.VChannelInitValue:
                        text = wfSignalDefinition.VChannelInitValue.ToString(cultureInfo);
                        break;
                    case SignalInformationField.AltValue:
                        text = wfSignalDefinition.AltValue.ToString(cultureInfo);
                        break;
                    case SignalInformationField.AltValue_1:
                        text = !wfSignalDefinition.AltValue_1.HasValue ? NullString : wfSignalDefinition.AltValue_1.Value.ToString(cultureInfo);
                        break;
                    case SignalInformationField.AltValue_2:
                        text = !wfSignalDefinition.AltValue_2.HasValue ? NullString : wfSignalDefinition.AltValue_2.Value.ToString(cultureInfo);
                        break;
                    case SignalInformationField.AltValue_3:
                        text = !wfSignalDefinition.AltValue_3.HasValue ? NullString : wfSignalDefinition.AltValue_3.Value.ToString(cultureInfo);
                        break;
                    case SignalInformationField.AltValue_4:
                        text = !wfSignalDefinition.AltValue_4.HasValue ? NullString : wfSignalDefinition.AltValue_4.Value.ToString(cultureInfo);
                        break;
                    case SignalInformationField.DataTypeID:
                        text = wfSignalDefinition.DataTypeID.ToString(cultureInfo);
                        break;
                    case SignalInformationField.Status:
                        text = wfSignalDefinition.Status.ToString(cultureInfo);
                        break;
                    case SignalInformationField.OfflineValue:
                        text = wfSignalDefinition.OfflineValue;
                        break;
                    case SignalInformationField.OPCEnabled:
                        text = wfSignalDefinition.OPCEnabled.ToString(cultureInfo);
                        break;
                    case SignalInformationField.OPCQuality:
                        text = wfSignalDefinition.OPCQuality.ToString(cultureInfo);
                        break;
                    case SignalInformationField.GroupId:
                        text = wfSignalDefinition.Group == null ? NullString : wfSignalDefinition.Group.ID.ToString();
                        break;
                    case SignalInformationField.GroupName:
                        text = wfSignalDefinition.Group == null ? NullString : wfSignalDefinition.Group.Name;
                        break;
                    case SignalInformationField.GroupDescription:
                        text = wfSignalDefinition.Group == null ? NullString : wfSignalDefinition.Group.Description;
                        break;
                    case SignalInformationField.WriteGroupId:
                        text = wfSignalDefinition.WriteGroup == null ? NullString : wfSignalDefinition.WriteGroup.ID.ToString();
                        break;
                    case SignalInformationField.WriteGroupName:
                        text = wfSignalDefinition.WriteGroup == null ? NullString : wfSignalDefinition.WriteGroup.Name;
                        break;
                    case SignalInformationField.WriteGroupDescription:
                        text = wfSignalDefinition.WriteGroup == null ? NullString : wfSignalDefinition.WriteGroup.Description;
                        break;
                    case SignalInformationField.ServerId:
                        text = wfSignalDefinition.Server == null ? NullString : wfSignalDefinition.Server.ID.ToString();
                        break;
                    case SignalInformationField.ServerName:
                        text = wfSignalDefinition.Server == null ? NullString : wfSignalDefinition.Server.Name;
                        break;
                    case SignalInformationField.ServerDescription:
                        text = wfSignalDefinition.Server == null ? NullString : wfSignalDefinition.Server.Description;
                        break;
                    case SignalInformationField.ConnectorId:
                        text = wfSignalDefinition.Connector == null ? NullString : wfSignalDefinition.Connector.ID.ToString();
                        break;
                    case SignalInformationField.ConnectorName:
                        text = wfSignalDefinition.Connector == null ? NullString : wfSignalDefinition.Connector.Name;
                        break;
                    case SignalInformationField.ConnectorDescription:
                        text = wfSignalDefinition.Connector == null ? NullString : wfSignalDefinition.Connector.Description;
                        break;
                }
            }
            uxSignalName.Text=text;
        }
        private string PrefixSignalName(string signalName)
        {
            return SignalPrefix + signalName;
        }
        private string ResolveSignalName(string signalName)
        {
            return StringPlaceholdersResolver.Resolve(signalName, this);
        }
        private void HandleSignalNameChange(string oldSignalName, string newSignalName, SignalInformationField signalInformation)
        {
            if (!DesignerPropertiesExt.IsInDesignTool)
            {
                connector.UnregisterSignalInformationChangedHandler(DefaultServerName, ResolveSignalName(PrefixSignalName(oldSignalName)), new OnSignalInformationChangedDelegate(this.OnSignalInformationChangedHandler));
                connector.RegisterSignalInformationChangedHandler(DefaultServerName, ResolveSignalName(PrefixSignalName(newSignalName)), new OnSignalInformationChangedDelegate(this.OnSignalInformationChangedHandler));
                return;
            }
            UpdateSignalNameText(PrefixSignalName(newSignalName), signalInformation);
        }
        private void HandleSignalInformationChange(string signalName, SignalInformationField signalInformation)
        {
            if (!DesignerPropertiesExt.IsInDesignTool)
            {
                UpdateSignalInformation();
                return;
            }
            UpdateSignalNameText(PrefixSignalName(signalName), signalInformation);
        }
        private void RegisterSymbolicTexts()
        {
            connector.RegisterLanguageChangedHandler(new string[]
			{
				InvalidSymbolicText
			}, new OnLanguageChangedDelegate(this.OnLanguageChangedDelegate));
        }
        private void UnregisterSymbolicTexts()
        {
            connector.UnregisterLanguageChangedHandler(new string[]
			{
				InvalidSymbolicText
			}, new OnLanguageChangedDelegate(this.OnLanguageChangedDelegate));
        }
        public void Dispose()
        {
            DisposeConnector();
        }
        private void DisposeConnector()
        {
            if (connector == null)
            {
                return;
            }
            UnregisterSymbolicTexts();
            connector.UnregisterSignalInformationChangedHandler(DefaultServerName, ResolveSignalName(PrefixSignalName(SignalName)), new OnSignalInformationChangedDelegate(this.OnSignalInformationChangedHandler));
            connector.Dispose();
        }
    }
}
