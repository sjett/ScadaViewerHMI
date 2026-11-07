// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.ProjectDesigner.DesignControls.Alarms;
using HMI.ProjectDesigner.DesignControls.Alarms.Serializers;
using HMI.ProjectDesigner.DesignControls.Buttons;
using HMI.ProjectDesigner.DesignControls.Buttons.Serializers;
using HMI.ProjectDesigner.DesignControls.Charts;
using HMI.ProjectDesigner.DesignControls.Charts.Serializers;
using HMI.ProjectDesigner.DesignControls.Gauges;
using HMI.ProjectDesigner.DesignControls.Gauges.Serializers;
using HMI.ProjectDesigner.DesignControls.General;
using HMI.ProjectDesigner.DesignControls.General.Serializers;
using HMI.ProjectDesigner.DesignControls.Html;
using HMI.ProjectDesigner.DesignControls.Html.Serializers;
using HMI.ProjectDesigner.DesignControls.Indicators;
using HMI.ProjectDesigner.DesignControls.Indicators.Serializers;
using HMI.ProjectDesigner.DesignControls.InputOutput;
using HMI.ProjectDesigner.DesignControls.InputOutput.Serializers;
using HMI.ProjectDesigner.DesignControls.Isa;
using HMI.ProjectDesigner.DesignControls.Isa.Serializers;
using HMI.ProjectDesigner.DesignControls.Labels;
using HMI.ProjectDesigner.DesignControls.Labels.Serializers;
using HMI.ProjectDesigner.DesignControls.Models;
using HMI.ProjectDesigner.DesignControls.Navigation;
using HMI.ProjectDesigner.DesignControls.Navigation.Serializers;
using HMI.ProjectDesigner.DesignControls.OpenDialog;
using HMI.ProjectDesigner.DesignControls.OpenDialog.Serializers;
using HMI.ProjectDesigner.DesignControls.OperationDiary;
using HMI.ProjectDesigner.DesignControls.OperationDiary.Serializers;
using HMI.ProjectDesigner.DesignControls.Pipes;
using HMI.ProjectDesigner.DesignControls.Pipes.Serializers;
using HMI.ProjectDesigner.DesignControls.Reporting;
using HMI.ProjectDesigner.DesignControls.Reporting.Serializers;
using HMI.ProjectDesigner.DesignControls.Serializers;
using HMI.ProjectDesigner.DesignControls.Serializers.DataTable;
using HMI.ProjectDesigner.DesignControls.Sliders;
using HMI.ProjectDesigner.DesignControls.Sliders.Serializers;
using HMI.ProjectDesigner.DesignControls.StaticSymbols;
using HMI.ProjectDesigner.DesignControls.StaticSymbols.Serializers;
using HMI.ProjectDesigner.DesignControls.UserManagement;
using HMI.ProjectDesigner.DesignControls.UserManagement.Serializers;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using HMI.ProjectDesigner.DesignControls.Views.Alarms.HMIAlarmViewer;
using HMI.ProjectDesigner.DesignControls.Views.DataTable;
using HMI.ProjectDesigner.DesignControls.Views.InputOutput;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Primitives.Serializers;
using HMI.ProjectDesigner.DesignControls.Primitives;
using HMI.ProjectDesigner.DesignControls.Views.Reporting;

namespace HMI.ProjectDesigner.DesignControls
{
	public class DesignControlsModule : PluginModule
	{
		protected override void RegisterMergeDictionaries()
		{
			base.RegisterMergeDictionaries();
			base.MergeDictionaryRegistry.AddDictionaryResource(new Uri("pack://application:,,,/HMI.ProjectDesigner.DesignControls;component/Views/DesignControls.xaml"));
		}
		private static void RegisterAlarmSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<RowColor, RowColorSerializer>();
			serializerFactory.RegisterSerializer<AlarmColumnSettings, AlarmColumnSettingsSerializer>();
			serializerFactory.RegisterSerializer<PriorityColor, PriorityColorSerializer>();
			serializerFactory.RegisterSerializer<HMIAlarmStateManager, HMIAlarmStateManagerSerializer>();
			serializerFactory.RegisterSerializer<HMIAlarmDuration1, HMIAlarmDuration1Serializer>();
			serializerFactory.RegisterSerializer<HMIAlarmFrequency1, HMIAlarmFrequency1Serializer>();
			serializerFactory.RegisterSerializer<HMIMachineAvailability1, HMIMachineAvailability1Serializer>();
			serializerFactory.RegisterSerializer<HMIAlarmOnline, HMIAlarmOnlineSerializer>();
			serializerFactory.RegisterSerializer<HMIAlarmViewer, HMIAlarmViewerSerializer>();
			serializerFactory.RegisterSerializer<SecurityPolicy, SecurityPolicySerializer>();
		}
		private static void RegisterBargraphSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIBargraph1, HMIBargraph1Serializer>();
			serializerFactory.RegisterSerializer<HMIBargraph2, HMIBargraph2Serializer>();
			serializerFactory.RegisterSerializer<HMIBargraph3, HMIBargraph3Serializer>();
			serializerFactory.RegisterSerializer<HMIBargraph4, HMIBargraph4Serializer>();
		}
		private static void RegisterButtonSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIButton1, HMIButton1Serializer>();
			serializerFactory.RegisterSerializer<HMIComboBoxItem, HMIComboBoxItemSerializer>();
			serializerFactory.RegisterSerializer<HMIComboBoxButton1, HMIComboBoxButton1Serializer>();
			serializerFactory.RegisterSerializer<HMILinkButton1, HMILinkButton1Serializer>();
			serializerFactory.RegisterSerializer<HMIStatesButton1, HMIStatesButton1Serializer>();
			serializerFactory.RegisterSerializer<HMIStatesToggleButton1, HMIStatesToggleButton1Serializer>();
			serializerFactory.RegisterSerializer<HMIToggleButton1, HMIToggleButton1Serializer>();
			serializerFactory.RegisterSerializer<HMIToggleButton2, HMIToggleButton2Serializer>();
			serializerFactory.RegisterSerializer<HMIToggleButton3, HMIToggleButton3Serializer>();
			serializerFactory.RegisterSerializer<HMIToggleButton4, HMIToggleButton4Serializer>();
		}
		private static void RegisterDataTableSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<DataTableButtonSettings, DataTableButtonSettingsSerializer>();
			serializerFactory.RegisterSerializer<HMIDataTable, HMIDataTableSerializer>();
		}
		private static void RegisterGaugeSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIGauge1, HMIGauge1Serializer>();
			serializerFactory.RegisterSerializer<HMIGauge2, HMIGauge2Serializer>();
		}
		private static void RegisterGeneralSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<Image, ImageSerializer>();
			serializerFactory.RegisterSerializer<HMIConnectorStatistics, HMIConnectorStatisticsSerializer>();
			serializerFactory.RegisterSerializer<HMILanguageSelector1, HMILanguageSelector1Serializer>();
			serializerFactory.RegisterSerializer<HMIDateTime1, HMIDateTime1Serializer>();
			serializerFactory.RegisterSerializer<HMILogFileViewer, HMILogFileViewerSerializer>();
			serializerFactory.RegisterSerializer<HMIUserDisplay1, HMIUserDisplay1Serializer>();
			serializerFactory.RegisterSerializer<HMIUserInformation1, HMIUserInformation1Serializer>();
			serializerFactory.RegisterSerializer<HMIUserLogin1, HMIUserLogin1Serializer>();
			serializerFactory.RegisterSerializer<HMIUserLogin2, HMIUserLogin2Serializer>();
			serializerFactory.RegisterSerializer<HMIWatchdog1, HMIWatchdog1Serializer>();
			serializerFactory.RegisterSerializer<LogbookSecurityPolicy, LogbookSecurityPolicySerializer>();
			serializerFactory.RegisterSerializer<HMILogbook, HMILogbookSerializer>();
            serializerFactory.RegisterSerializer<HMIWeb, HMIWebSerializer>();
        }

        private static void RegisterHtmlSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIMessengerPro, HMIMessengerProSerializer>();
			serializerFactory.RegisterSerializer<HMISchedulerPro, HMISchedulerProSerializer>();
			serializerFactory.RegisterSerializer<HMIScenarioManagerPro, HMIScenarioManagerProSerializer>();
		}
		private static void RegisterIndicatorSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIIndicator1, HMIIndicator1Serializer>();
			serializerFactory.RegisterSerializer<HMIIndicator2, HMIIndicator2Serializer>();
			serializerFactory.RegisterSerializer<HMIIndicator3, HMIIndicator3Serializer>();
			serializerFactory.RegisterSerializer<HMIIndicatorDynamic, HMIIndicatorDynamicSerializer>();
			serializerFactory.RegisterSerializer<HMIIndicatorDynamicColor, HMIIndicatorDynamicColorSerializer>();
			serializerFactory.RegisterSerializer<HMICompass1, HMICompass1Serializer>();
		}
		private static void RegisterInputOutputSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMINumericBox1, HMINumericBox1Serializer>();
			serializerFactory.RegisterSerializer<HMINumericBox2, HMINumericBox2Serializer>();
			serializerFactory.RegisterSerializer<HMIExtendedNumericDisplay1, HMIExtendedNumericDisplay1Serializer>();
			serializerFactory.RegisterSerializer<HMINumericDisplay1, HMINumericDisplay1Serializer>();
			serializerFactory.RegisterSerializer<HMITextBox1, HMITextBox1Serializer>();
			serializerFactory.RegisterSerializer<HMITextBox2, HMITextBox2Serializer>();
			serializerFactory.RegisterSerializer<HMITextDisplay1, HMITextDisplay1Serializer>();
			serializerFactory.RegisterSerializer<HMINumericDisplay2, HMINumericDisplay2Serializer>();
			serializerFactory.RegisterSerializer<HMIStateDisplay1, HMIStateDisplay1Serializer>();
		}
		private static void RegisterIsaSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIISACornerLine1, HMIISACornerLine1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACrossLine1, HMIISACrossLine1Serializer>();
			serializerFactory.RegisterSerializer<HMIISALine1, HMIISALine1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAMotor1, HMIISAMotor1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPressureIndicator1, HMIISAPressureIndicator1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPump1, HMIISAPump1Serializer>();
			serializerFactory.RegisterSerializer<HMIISARegulatorDiscPressure1, HMIISARegulatorDiscPressure1Serializer>();
			serializerFactory.RegisterSerializer<HMIISARegulatorExtPressValve1, HMIISARegulatorExtPressValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISARegulatorSelf1, HMIISARegulatorSelf1Serializer>();
			serializerFactory.RegisterSerializer<HMIISARegulatorSelf2, HMIISARegulatorSelf2Serializer>();
			serializerFactory.RegisterSerializer<HMIISARuptureDiscVacuum1, HMIISARuptureDiscVacuum1Serializer>();
			serializerFactory.RegisterSerializer<HMIISATLine1, HMIISATLine1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAULine1, HMIISAULine1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValve1, HMIISAValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValve3Way1, HMIISAValve3Way1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValve4Way1, HMIISAValve4Way1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveAngle1, HMIISAValveAngle1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveBall1, HMIISAValveBall1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveBleedPurge1, HMIISAValveBleedPurge1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveBlowDown1, HMIISAValveBlowDown1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveBlowDownAngle1, HMIISAValveBlowDownAngle1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveButterfly1, HMIISAValveButterfly1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveCheck1, HMIISAValveCheck1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveCheckStop1, HMIISAValveCheckStop1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveControlHand1, HMIISAValveControlHand1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveCylinderActuated1, HMIISAValveCylinderActuated1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveDiaphragm1, HMIISAValveDiaphragm1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValveDiaphragmA1, HMIISAValveDiaphragmA1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAVentilator1, HMIISAVentilator1Serializer>();
			serializerFactory.RegisterSerializer<HMIISA2WayValve1, HMIISA2WayValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISA2WayValve2, HMIISA2WayValve2Serializer>();
			serializerFactory.RegisterSerializer<HMIISA3WayValve2, HMIISA3WayValve2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAAirFilter1, HMIISAAirFilter1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAAxialFan1, HMIISAAxialFan1Serializer>();
			serializerFactory.RegisterSerializer<HMIISABackDraftDamper1, HMIISABackDraftDamper1Serializer>();
			serializerFactory.RegisterSerializer<HMIISABag1, HMIISABag1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACooledOrHeatedPipe1, HMIISACooledOrHeatedPipe1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACooler1, HMIISACooler1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACooling1, HMIISACooling1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACooling2, HMIISACooling2Serializer>();
			serializerFactory.RegisterSerializer<HMIISACoolingTower1, HMIISACoolingTower1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACourvedGasVent1, HMIISACourvedGasVent1Serializer>();
			serializerFactory.RegisterSerializer<HMIISACoveredGasVent1, HMIISACoveredGasVent1Serializer>();
			serializerFactory.RegisterSerializer<HMIISADiaphragmValve1, HMIISADiaphragmValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISADoublePipeHeatExchanger1, HMIISADoublePipeHeatExchanger1Serializer>();
			serializerFactory.RegisterSerializer<HMIISADryer1, HMIISADryer1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFan1, HMIISAFan1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFilter1, HMIISAFilter1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFilter2, HMIISAFilter2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFlap1, HMIISAFlap1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFlap2, HMIISAFlap2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFlap3, HMIISAFlap3Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFlap4, HMIISAFlap4Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFlexiblePipe1, HMIISAFlexiblePipe1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFunnel1, HMIISAFunnel1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAFurnace1, HMIISAFurnace1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAGasBottle1, HMIISAGasBottle1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHalfPipeMixingVessel1, HMIISAHalfPipeMixingVessel1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHeatExchanger1, HMIISAHeatExchanger1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHeatExchanger2, HMIISAHeatExchanger2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHeatExchanger3, HMIISAHeatExchanger3Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHeatExchanger4, HMIISAHeatExchanger4Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHeating2, HMIISAHeating2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAHeatingCircuit1, HMIISAHeatingCircuit1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAJacketedMixingVessel1, HMIISAJacketedMixingVessel1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAJacketedPipe1, HMIISAJacketedPipe1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAManualValve1, HMIISAManualValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISANeedleValve1, HMIISANeedleValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPackingColumn1, HMIISAPackingColumn1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPipe1, HMIISAPipe1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPlateHeatExchanger1, HMIISAPlateHeatExchanger1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPressure1, HMIISAPressure1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPressure2, HMIISAPressure2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPressureReducingValve1, HMIISAPressureReducingValve1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPressurizedHorizontalVessel1, HMIISAPressurizedHorizontalVessel1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPressurizedVerticalVessel1, HMIISAPressurizedVerticalVessel1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAPump2, HMIISAPump2Serializer>();
			serializerFactory.RegisterSerializer<HMIISARadialFan1, HMIISARadialFan1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAShellAndTubeHeatExchanger1, HMIISAShellAndTubeHeatExchanger1Serializer>();
			serializerFactory.RegisterSerializer<HMIISASpiralHeatExchanger1, HMIISASpiralHeatExchanger1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAStreamTrap1, HMIISAStreamTrap1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAThermallyInsulatedPipe1, HMIISAThermallyInsulatedPipe1Serializer>();
			serializerFactory.RegisterSerializer<HMIISATrayColumn1, HMIISATrayColumn1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAUShapedTubesHeatExchanger1, HMIISAUShapedTubesHeatExchanger1Serializer>();
			serializerFactory.RegisterSerializer<HMIISAValve2, HMIISAValve2Serializer>();
			serializerFactory.RegisterSerializer<HMIISAViewingGlass1, HMIISAViewingGlass1Serializer>();
		}
		private static void RegisterLabelSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMILabel1, HMILabel1Serializer>();
			serializerFactory.RegisterSerializer<HMISignalInformationLabel1, HMISignalInformationLabel1Serializer>();
		}
		private static void RegisterNavigationSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<NavigationItem, NavigationItemSerializer>();
			serializerFactory.RegisterSerializer<NavigationParameter, NavigationParameterSerializer>();
			serializerFactory.RegisterSerializer<HMINavigation, HMINavigationSerializer>();
			serializerFactory.RegisterSerializer<HMINavigationToggle1, HMINavigationToggle1Serializer>();
			serializerFactory.RegisterSerializer<HMINavigationTarget, HMINavigationTargetSerializer>();
			serializerFactory.RegisterSerializer<HMINavigationParameter, HMINavigationParameterSerializer>();
		}
		private static void RegisterOpenDialogSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIOpenDialog, HMIOpenDialogSerializer>();
		}
		private static void RegisterOperationDiarySerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<ColumnSettings, ColumnSettingsSerializer>();
			serializerFactory.RegisterSerializer<HMIOperationDiary, HMIOperationDiarySerializer>();
		}
		private static void RegisterPipeSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<LineControl, LineControlSerializer>();
			serializerFactory.RegisterSerializer<PipeControl, PipeControlSerializer>();
			serializerFactory.RegisterSerializer<HMICornerLine1, HMICornerLine1Serializer>();
			serializerFactory.RegisterSerializer<HMICornerPipe1, HMICornerPipe1Serializer>();
			serializerFactory.RegisterSerializer<HMICornerPipe2, HMICornerPipe2Serializer>();
			serializerFactory.RegisterSerializer<HMICornerPipe3, HMICornerPipe3Serializer>();
			serializerFactory.RegisterSerializer<HMICrossLine1, HMICrossLine1Serializer>();
			serializerFactory.RegisterSerializer<HMILine1, HMILine1Serializer>();
			serializerFactory.RegisterSerializer<HMILinePipe1, HMILinePipe1Serializer>();
			serializerFactory.RegisterSerializer<HMITLine1, HMITLine1Serializer>();
			serializerFactory.RegisterSerializer<HMITPipe1, HMITPipe1Serializer>();
			serializerFactory.RegisterSerializer<HMITPipe2, HMITPipe2Serializer>();
			serializerFactory.RegisterSerializer<HMITPipe3, HMITPipe3Serializer>();
			serializerFactory.RegisterSerializer<HMIULine1, HMIULine1Serializer>();
		}
		private static void RegisterPrimitiveSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIEllipse1, HMIEllipse1Serializer>();
			serializerFactory.RegisterSerializer<HMIRectangle1, HMIRectangle1Serializer>();
			serializerFactory.RegisterSerializer<HMITrapeze1, HMITrapeze1Serializer>();
			serializerFactory.RegisterSerializer<HMITriangle1, HMITriangle1Serializer>();
			serializerFactory.RegisterSerializer<HMIBorder1, HMIBorder1Serializer>();
			serializerFactory.RegisterSerializer<HMIAnimatedEllipse1, HMIAnimatedEllipse1Serializer>();
			serializerFactory.RegisterSerializer<HMIAnimatedRectangle1, HMIAnimatedRectangle1Serializer>();
			serializerFactory.RegisterSerializer<HMIAnimatedTrapeze1, HMIAnimatedTrapeze1Serializer>();
			serializerFactory.RegisterSerializer<HMIAnimatedTriangle1, HMIAnimatedTriangle1Serializer>();
		}
		private static void RegisterReportSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMITelerikReportViewer, HMITelerikReportViewerSerializer>();
			serializerFactory.RegisterSerializer<HMISqlReportViewer, HMISqlReportViewerSerializer>();
		}
		private static void RegisterSliderSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMISlider1, HMISlider1Serializer>();
		}
		private static void RegisterStaticSymbolSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<StaticSymbol, StaticSymbolSerializer>();
		}
		private static void RegisterTrendingSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMITrending2, HMITrending2Serializer>();
			serializerFactory.RegisterSerializer<HMITrending3, HMITrending3Serializer>();
			serializerFactory.RegisterSerializer<HMIRealtimeChart, HMIRealtimeChartSerializer>();
		}
		private static void RegisterUserManagerSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<HMIUserManager, HMIUserManagerSerializer>();
		}
		protected override void RegisterSerializers(ISerializerFactory serializerFactory)
		{
			serializerFactory.RegisterSerializer<ConditionSignal, ConditionSignalSerializer>();
			serializerFactory.RegisterSerializer<StateCondition, StateConditionSerializer>();
			serializerFactory.RegisterSerializer<AnimatedStateCondition, AnimatedStateConditionSerializer>();
			serializerFactory.RegisterSerializer<SymbolicTextParameterDefinition, ParameterDefinitionSerializer>();
			serializerFactory.RegisterSerializer<ParameterDefinition, ParameterDefinitionSerializer>();
			serializerFactory.RegisterSerializer<SignalParameterDefinition, ParameterDefinitionSerializer>();
			serializerFactory.RegisterSerializer<AlarmGroupParameterDefinition, ParameterDefinitionSerializer>();
			serializerFactory.RegisterSerializer<AlarmTypeParameterDefinition, ParameterDefinitionSerializer>();
			serializerFactory.RegisterSerializer<AlarmGroupFilter, AlarmGroupFilterSerializer>();
			serializerFactory.RegisterSerializer<AlarmTypeFilter, AlarmTypeFilterSerializer>();
			serializerFactory.RegisterSerializer<AlarmTypeDisplay, AlarmTypeDisplaySerializer>();
            RegisterAlarmSerializers(serializerFactory);
            RegisterBargraphSerializers(serializerFactory);
            RegisterButtonSerializers(serializerFactory);
            RegisterDataTableSerializers(serializerFactory);
            RegisterGaugeSerializers(serializerFactory);
            RegisterGeneralSerializers(serializerFactory);
            RegisterHtmlSerializers(serializerFactory);
            RegisterIndicatorSerializers(serializerFactory);
            RegisterInputOutputSerializers(serializerFactory);
            RegisterIsaSerializers(serializerFactory);
            RegisterLabelSerializers(serializerFactory);
            RegisterNavigationSerializers(serializerFactory);
            RegisterOpenDialogSerializers(serializerFactory);
            RegisterOperationDiarySerializers(serializerFactory);
            RegisterPipeSerializers(serializerFactory);
            RegisterPrimitiveSerializers(serializerFactory);
            RegisterReportSerializers(serializerFactory);
            RegisterSliderSerializers(serializerFactory);
            RegisterStaticSymbolSerializers(serializerFactory);
            RegisterTrendingSerializers(serializerFactory);
            RegisterUserManagerSerializers(serializerFactory);
		}
	}
}
