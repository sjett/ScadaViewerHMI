// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
using System.Globalization;
namespace HMI.RuntimeControls.Indicators.Localization
{
	public class LanguageTranslator : PropertyChangedNotifier, IDisposable, ILanguageTranslator
	{
		private readonly OnLanguageChangedDelegate onLanguageChangedDelegate;
		private readonly IHMITranslationProvider translationProvider;
		private readonly Dictionary<string, string> dictTranslations;

		public event EventHandler? LanguageChanged;

		public int CurrentLanguageId
		{
			get
			{
				return this.translationProvider.GetLanguageId();
			}
		}
		public CultureInfo Culture
		{
			get
			{
				return HMI.Runtime.Core.Helper.CultureInfoCache.GetCultureInfo(this.CurrentLanguageId);
			}
		}
		public string North
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_North"];
			}
		}
		public string NorthWest
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_NorthWest"];
			}
		}
		public string West
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_West"];
			}
		}
		public string SouthWest
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_SouthWest"];
			}
		}
		public string South
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_South"];
			}
		}
		public string SouthEast
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_SouthEast"];
			}
		}
		public string East
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_East"];
			}
		}
		public string NorthEast
		{
			get
			{
				return this.dictTranslations["HMISN_Ctrl_HMICompass1_NorthEast"];
			}
		}
		public LanguageTranslator(IHMITranslationProvider translationProvider)
		{
			ValidationUtils.CheckIsNull<IHMITranslationProvider>(() => translationProvider);

			this.translationProvider = translationProvider;
			this.dictTranslations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			this.onLanguageChangedDelegate = new OnLanguageChangedDelegate(this.onLanguageChanged);
			string[] symbolicTexts = this.getTranslationNames();
			this.RegisterSymbolicTexts(symbolicTexts);
		}
		public void RegisterSymbolicTexts(string[] symbolicTexts)
		{
			string[] symbolicNames = this.getSymbolicNamesForTrans(symbolicTexts);
			if (symbolicNames.Length == 0)
			{
				return;
			}
			this.addDictTranslations(symbolicNames);
			this.translationProvider.RegisterLanguageChangedHandler(symbolicNames, this.onLanguageChangedDelegate);
		}
		public string GetTranslation(string symbolicText)
		{
			return this.dictTranslations[symbolicText];
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}
		~LanguageTranslator()
		{
			this.Dispose(false);
		}
		private void Dispose(bool disposed)
		{
			if (disposed)
			{
				this.DisposeExplicit();
			}
		}
		protected virtual void DisposeExplicit()
		{
			if (this.onLanguageChangedDelegate != null)
			{
				this.translationProvider.UnregisterLanguageChangedHandler(null, this.onLanguageChangedDelegate);
			}
		}
		private void onLanguageChanged(string[] symbolicNames, string[] translationTexts)
		{
			this.updateTranslations(symbolicNames, translationTexts);

			this.RaiseLanguageChanged();
			base.RaisePropertyChanged<int>(() => this.CurrentLanguageId);
			base.RaisePropertyChanged<CultureInfo>(() => this.Culture);

			this.OnLanguageChanged();
		}
		private void updateTranslations(string[] symbolicNames, string[] translationTexts)
		{
			for (int i = 0; i < symbolicNames.Length; i++)
			{
				this.updateTranslation(symbolicNames[i], translationTexts[i]);
			}
		}
		private void updateTranslation(string symbolicName, string translationText)
		{
			if (this.dictTranslations.ContainsKey(symbolicName))
			{
				this.dictTranslations[symbolicName] = (translationText == null || !(translationText != "!null!")) ? symbolicName : translationText;
			}
		}
		protected virtual void OnLanguageChanged()
		{
			this.LanguageChanged?.Invoke(this, EventArgs.Empty);
		}
		private void addDictTranslations(string[] symbolicNames)
		{
			for (int i = 0; i < symbolicNames.Length; i++)
			{
				string text = symbolicNames[i];
				this.dictTranslations.Add(text, string.Empty);
			}
		}
		private string[] getSymbolicNamesForTrans(string[] symbolicTexts)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < symbolicTexts.Length; i++)
			{
				string text = symbolicTexts[i];
				if (!this.dictTranslations.ContainsKey(text) && !list.Contains(text))
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}
		private string[] getTranslationNames()
		{
			List<string> list = new List<string>();
			list.Add("HMISN_Ctrl_HMICompass1_North");
			list.Add("HMISN_Ctrl_HMICompass1_NorthWest");
			list.Add("HMISN_Ctrl_HMICompass1_West");
			list.Add("HMISN_Ctrl_HMICompass1_SouthWest");
			list.Add("HMISN_Ctrl_HMICompass1_South");
			list.Add("HMISN_Ctrl_HMICompass1_SouthEast");
			list.Add("HMISN_Ctrl_HMICompass1_East");
			list.Add("HMISN_Ctrl_HMICompass1_NorthEast");
			return list.ToArray();
		}
		private void RaiseLanguageChanged()
		{
			base.RaisePropertyChanged<string>(() => this.North);
			base.RaisePropertyChanged<string>(() => this.NorthWest);
			base.RaisePropertyChanged<string>(() => this.West);
			base.RaisePropertyChanged<string>(() => this.SouthWest);
			base.RaisePropertyChanged<string>(() => this.South);
			base.RaisePropertyChanged<string>(() => this.SouthEast);
			base.RaisePropertyChanged<string>(() => this.East);
			base.RaisePropertyChanged<string>(() => this.NorthEast);
		}
	}
}
