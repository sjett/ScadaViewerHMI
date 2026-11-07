// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    ///  参考线设置视图模型
    /// </summary>
    public class GuideSettingsViewModel : ViewModel, IGuideSettingsViewModel
	{
		internal bool _showRulerGuides = true;
		internal bool _showGridLines = true;
		internal bool _snapToGridLines = true;
		internal bool _snapToGuideLines = true;

		public event EventHandler<ValueChangedEventArgs<bool>>? ShowRulerGuidesChanged;

		public event EventHandler<ValueChangedEventArgs<bool>>? ShowGridLinesChanged;

		public event EventHandler<ValueChangedEventArgs<bool>>? SnapToGridLinesChanged;

		public event EventHandler<ValueChangedEventArgs<bool>>? SnapToGuideLinesChanged;

		public bool ShowRulerGuides
		{
			get
			{
				return _showRulerGuides;
			}
			set
			{
				if (_showRulerGuides != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_showRulerGuides, value);
                    _showRulerGuides = value;
                    ShowRulerGuidesChanged?.Invoke(this, e);
					base.RaisePropertyChanged<bool>(() => ShowRulerGuides);
                    savePreferences();
				}
			}
		}
		public bool ShowGridLines
		{
			get
			{
				return _showGridLines;
			}
			set
			{
				if (_showGridLines != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_showGridLines, value);
                    _showGridLines = value;
                    ShowGridLinesChanged?.Invoke(this, e);
					base.RaisePropertyChanged<bool>(() => ShowGridLines);
                    savePreferences();
				}
			}
		}
		public bool SnapToGridLines
		{
			get
			{
				return _snapToGridLines;
			}
			set
			{
				if (_snapToGridLines != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_snapToGridLines, value);
                    _snapToGridLines = value;
                    SnapToGridLinesChanged?.Invoke(this, e);
					base.RaisePropertyChanged<bool>(() => SnapToGridLines);
                    savePreferences();
				}
			}
		}
		public bool SnapToGuideLines
		{
			get
			{
				return _snapToGuideLines;
			}
			set
			{
				if (_snapToGuideLines != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_snapToGuideLines, value);
                    _snapToGuideLines = value;
                    SnapToGuideLinesChanged?.Invoke(this, e);
					base.RaisePropertyChanged<bool>(() => SnapToGuideLines);
                    savePreferences();
				}
			}
		}
		public GuideSettingsViewModel()
		{
			loadPreferences();
		}
        void loadPreferences()
        {
            var preferences = Preferences.GetPreferences(this);
            if (preferences != null)
            {
                _showRulerGuides = ((bool?)preferences["ShowRulerGuides"]) ?? true;
                _showGridLines = ((bool?)preferences["ShowGridLines"]) ?? true;
                _snapToGridLines = ((bool?)preferences["SnapToGridLines"]) ?? true;
                _snapToGuideLines = ((bool?)preferences["SnapToGuideLines"]) ?? true;
            }
        }
        private void savePreferences()
		{
			Preferences.SetPreferences(this, new JObject
			{
				new JProperty("ShowRulerGuides", ShowRulerGuides),
				new JProperty("ShowGridLines", ShowGridLines),
				new JProperty("SnapToGridLines", SnapToGridLines),
				new JProperty("SnapToGuideLines", SnapToGuideLines)
			});
		}
	}
}
