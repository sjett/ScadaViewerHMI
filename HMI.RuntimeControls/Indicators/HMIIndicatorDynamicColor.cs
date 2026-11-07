// SPDX-License-Identifier: GPL-3.0-only
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace HMI.RuntimeControls.Indicators
{
    /// <summary>



    /// </summary>
    [
        TemplateVisualState(GroupName = "CommonStates", Name = "State7"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State6"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State3"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State8"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State1"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State5")]
	public class HMIIndicatorDynamicColor : HMIIndicatorDynamicBase
	{

        private Dictionary<string, DependencyObject> timelineTargetDict = new Dictionary<string, DependencyObject>();
		

		public HMIIndicatorDynamicColor()
		{
			base.DefaultStyleKey=(typeof(HMIIndicatorDynamicColor));
		}
		protected override void HandleDownloadCompleted(object state, string downloadData)
		{
			var stateName = state.ToString();

			string result = downloadData;

			IList visualStateGroups = VisualStateManager.GetVisualStateGroups(this.LayoutRoot);
			var vsgArray = visualStateGroups.Cast<VisualStateGroup>();
            VisualStateGroup? visualStateGroup = vsgArray.Where(vsg => vsg.Name == "CommonStates").FirstOrDefault();
			if (visualStateGroup == null)
			{
				return;
			}

            VisualState? visualState = visualStateGroup.States
										.Cast<VisualState>()
										.Where(vs => vs.Name == stateName)
										.FirstOrDefault();

			object obj = HMIIndicatorDynamicBase.GetBadXamlResource();
			try
			{
				using (Stream stream = File.OpenRead(result))
				{
					obj = (XamlReader.Load(stream) as Storyboard)!;

					visualState!.Storyboard = ((Storyboard)obj);
					this.handleTimeline(visualState);
				}
			}
			catch
			{
			}
			this.ChangeVisualState();
		}
		protected override void DownloadXamlFiles()
		{
			base.DownloadXamlFiles();

			this.HandleXamlFile("Normal", base.DefaultStateXamlPath ?? string.Empty);
			this.HandleXamlFile("State1", base.StateXamlPath1);
			this.HandleXamlFile("State2", base.StateXamlPath2);
			this.HandleXamlFile("State3", base.StateXamlPath3);
			this.HandleXamlFile("State4", base.StateXamlPath4);
			this.HandleXamlFile("State5", base.StateXamlPath5);
			this.HandleXamlFile("State6", base.StateXamlPath6);
			this.HandleXamlFile("State7", base.StateXamlPath7);
			this.HandleXamlFile("State8", base.StateXamlPath8);
		}
		private void handleTimeline(VisualState visualState)
		{
			foreach (Timeline current in visualState.Storyboard.Children)
			{
				string text = current.GetValue(Storyboard.TargetNameProperty).ToString()!;
				if (!this.timelineTargetDict.ContainsKey(text))
				{
					this.timelineTargetDict.Add(text, base.GetTemplateChild(text));
				}
				Storyboard.SetTarget(current, this.timelineTargetDict[text]);
			}
		}
		

	}
}
