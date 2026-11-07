// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace HMI.RuntimeControls.Indicators
{
    /// <summary>



    /// </summary>
    [
        TemplateVisualState(GroupName = "CommonStates", Name = "State2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State7"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State5"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State6"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State1"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State8"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State3")]
	public class HMIIndicatorDynamic : HMIIndicatorDynamicBase
	{
		private Grid? _xamlContainer;

		private int _currentState = -1;
		private FrameworkElement? _normalStateContent;
		private FrameworkElement? _state1Content;
		private FrameworkElement? _state2Content;
		private FrameworkElement? _state3Content;
		private FrameworkElement? _state4Content;
		private FrameworkElement? _state5Content;
		private FrameworkElement? _state6Content;
		private FrameworkElement? _state7Content;
		private FrameworkElement? _state8Content;

		public HMIIndicatorDynamic()
		{
			base.DefaultStyleKey=(typeof(HMIIndicatorDynamic));
		}
		public override void OnApplyTemplate()
		{
			this._xamlContainer = (base.GetTemplateChild("uxXamlContainer") as Grid);

			base.OnApplyTemplate();

			base.Dispatcher.BeginInvoke(new Action(this.ChangeVisualState));
		}
		protected override void ChangeVisualState()
		{
			if (this._xamlContainer == null)
			{
				return;
			}

            var stateMap = new (int state, Func<bool> canGo, FrameworkElement? xaml)[]
            {
				(8, base.CanGoToState8, this._state8Content),
				(7, base.CanGoToState7, this._state7Content),
				(6, base.CanGoToState6, this._state6Content),
				(5, base.CanGoToState5, this._state5Content),
				(4, base.CanGoToState4, this._state4Content),
				(3, base.CanGoToState3, this._state3Content),
				(2, base.CanGoToState2, this._state2Content),
				(1, base.CanGoToState1, this._state1Content),
				(0, () => true, this._normalStateContent)
            };


            foreach (var (state, canGo, xaml) in stateMap)
            {
                if (xaml != null && canGo())
                {
                    if (this._currentState != state)
                    {
                        this._currentState = state;
                        this.SetCurrentElement(xaml);
                    }
                    return;
                }
            }

		}

		private void SetCurrentElement(FrameworkElement stateContent)
		{
			foreach (UIElement current in this._xamlContainer!.Children)
			{
				current.StopStoryboards();
				NavigationHelper.DisposeControls(current);
			}
			this._xamlContainer.Children.Clear();
			this._xamlContainer.Children.Add(stateContent);
		}
		protected override void DownloadXamlFiles()
		{
			base.DownloadXamlFiles();

			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetNormalStateContent), base.DefaultStateXamlPath ?? string.Empty);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState1Content), base.StateXamlPath1);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState2Content), base.StateXamlPath2);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState3Content), base.StateXamlPath3);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState4Content), base.StateXamlPath4);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState5Content), base.StateXamlPath5);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState6Content), base.StateXamlPath6);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState7Content), base.StateXamlPath7);
			this.LoadStateXamlElement(new Action<FrameworkElement>(this.SetState8Content), base.StateXamlPath8);
		}
		private void LoadStateXamlElement(Action<FrameworkElement> setStateContent, string xamlFile)
		{
			base.HandleXamlFile(setStateContent, xamlFile);

			if (string.IsNullOrEmpty(xamlFile))
			{
				setStateContent.Invoke(new Grid());
            }
		}
		protected override void HandleDownloadCompleted(object state, string downloadData)
		{
			Action<FrameworkElement> setStateContent = (Action<FrameworkElement>)state;
			string downloadedFilePath = downloadData;
			FrameworkElement stateContent;
			try
			{
				using (Stream stream = File.OpenRead(downloadedFilePath))
				{
					stateContent = (FrameworkElement)XamlReader.Load(stream);
                }
			}
			catch
			{
				stateContent = HMIIndicatorDynamicBase.GetBadXamlResource();
			}
			if (setStateContent != null)
			{
				setStateContent.Invoke(stateContent);
			}

			this.ChangeVisualState();
		}
		
		private void SetNormalStateContent(FrameworkElement content)
		{
			this._normalStateContent = content;
		}
		
		private void SetState1Content(FrameworkElement content)
		{
			this._state1Content = content;
		}
		
		private void SetState2Content(FrameworkElement content)
		{
			this._state2Content = content;
		}
		
		private void SetState3Content(FrameworkElement content)
		{
			this._state3Content = content;
		}
		
		private void SetState4Content(FrameworkElement content)
		{
			this._state4Content = content;
		}
		
		private void SetState5Content(FrameworkElement content)
		{
			this._state5Content = content;
		}
		
		private void SetState6Content(FrameworkElement content)
		{
			this._state6Content = content;
		}
		
		private void SetState7Content(FrameworkElement content)
		{
			this._state7Content = content;
		}
		
		private void SetState8Content(FrameworkElement content)
		{
			this._state8Content = content;
		}
	}
}
