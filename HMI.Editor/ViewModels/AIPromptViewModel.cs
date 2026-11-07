// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using HMI.Editor.Services;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;
using Telerik.Windows.Controls.ConversationalUI;
using DelegateCommand = Telerik.Windows.Controls.DelegateCommand;

namespace HMI.Editor.ViewModels;

public class AIPromptViewModel : ViewModel
{
    private string inputText;
    private readonly System.IServiceProvider container;
    public AIPromptViewModel(System.IServiceProvider provider)
    {
        this.container = provider;

        this.OutputItems = new ObservableCollection<AIPromptOutputItemModel>();
        this.Suggestions = new List<string>()
        {
                AIService.Out_Of_Office,
                AIService.LinkedIn_Well_Being,
                AIService.LinkedIn_Mental_Health,
            };

        this.PromptRequestCommand = new DelegateCommand(OnPromptRequestCommandExecuted);
        this.OutputItemRetryCommand = new DelegateCommand(OnOutputItemRetryCommandExecuted);

        this.Commands = new ObservableCollection<AIPromptCommandBase>()
            {
                new AIPromptCommandGroup()
                {
                    Icon = "pack://application:,,,/HMI.Editor;component/Resources/Images/BrushIcon.svg",
                    Text = "改变形式",
                    Commands = new List<AIPromptCommand>()
                    {
                        new AIPromptCommand() { Text = "随意", Command = new DelegateCommand(OnCommandsViewCommandExecuted) },
                        new AIPromptCommand() { Text = "中性", Command = new DelegateCommand(OnCommandsViewCommandExecuted) },
                        new AIPromptCommand() { Text = "正式", Command = new DelegateCommand(OnCommandsViewCommandExecuted) }
                    }
                },
                new AIPromptCommandGroup()
                {
                    Icon = "pack://application:,,,/HMI.Editor;component/Resources/Images/EditToolsIcon.svg",
                    Text = "翻译",
                    Commands = new List<AIPromptCommand>()
                    {
                        new AIPromptCommand() { Text = "Chinese", Command = new DelegateCommand(OnCommandsViewCommandExecuted) },
                        new AIPromptCommand() { Text = "English", Command = new DelegateCommand(OnCommandsViewCommandExecuted) }
                    }
                },
                new AIPromptCommand()
                {
                    Icon = "pack://application:,,,/HMI.Editor;component/Resources/Images/MinWidthIcon.svg",
                    Text = "Simplify",
                    Command = new DelegateCommand(OnCommandsViewCommandExecuted)
                },
                new AIPromptCommand()
                {
                    Icon = "pack://application:,,,/HMI.Editor;component/Resources/Images/ArrowsLeftRightIcon.svg",
                    Text = "Extend",
                    Command = new DelegateCommand(OnCommandsViewCommandExecuted)
                }
            };
    }

    public string InputText
    {
        get 
        { 
            return this.inputText;
        }
        set 
        { 
            this.inputText = value; 
            this.RaisePropertyChanged(()=> InputText); 
        }
    }

    public ObservableCollection<AIPromptOutputItemModel> OutputItems { get; set; }
    public ICommand PromptRequestCommand { get; set; }
    public ICommand OutputItemRetryCommand { get; set; }
    public IList<AIPromptCommandBase> Commands { get; set; }
    public IEnumerable<string> Suggestions { get; set; }

    public RadDesktopAlertManager DesktopAlertManager { get; set; } = new RadDesktopAlertManager(AlertScreenPosition.TopCenter, 5d);

    private void OnPromptRequestCommandExecuted(object obj)
    {
        AIPromptOutputItemModel responseAIPromptOutputItemModel = new AIPromptOutputItemModel()
        {
            InputText = this.InputText,
        };

        responseAIPromptOutputItemModel.Title = AIService.IsValidInputText(responseAIPromptOutputItemModel.InputText) ? responseAIPromptOutputItemModel.InputText : "人工智能生成:";
        responseAIPromptOutputItemModel.ResponseText = AIService.GetResponse("Chinese", responseAIPromptOutputItemModel.InputText);

        this.OutputItems.Insert(0, responseAIPromptOutputItemModel);


        if(responseAIPromptOutputItemModel.InputText == AIService.Out_Of_Office)
        {
            var pro = this.container.Resolve<IProjectManagerService>();
            var pvm = pro.OpenProject("C:\\Users\\lee\\Documents\\HMI Editor Projects\\Project2\\Project2.Project");
            var pm = this.container.Resolve<IProjectManagerViewModel>();
            pm.Project = pvm;
        }

        this.DesktopAlertManager.ShowAnimation = new SlideAnimation { Direction = AnimationDirection.In, SlideMode = SlideMode.Top, SpeedRatio = 0.3d, PixelsToAnimate = 100 };
        this.DesktopAlertManager.HideAnimation = new FadeAnimation { Direction = AnimationDirection.Out, MinOpacity = 0.5d, MaxOpacity = 0.9d, SpeedRatio = 0.5d };

        var alert = new RadDesktopAlert()
        {
            Header = "操作提醒",
            Content = responseAIPromptOutputItemModel.Title,
            ShowDuration = 3000,
            CanMove = true,
            CanAutoClose = true,
            //Icon = new Image { Source = Application.Current.FindResource("DesktopAlertIcon") as ImageSource, Width = 48, Height = 48 },
            //IconColumnWidth = 48,
            //IconMargin = new Thickness(10, 0, 20, 0),
            //ShowMenuButton = true,
            ShowCloseButton = true,
            //MenuItemsSource = this.GetMenuItems(),
            //Sound = this.SelectedSound.Sound
        };
        alert.Closed += (s, a) =>
        {
        };
        //if (this.IsAttachedToWindow)
        {
            this.DesktopAlertManager.Attach(Application.Current.MainWindow);
        }
        //else
        //{
        //    this.DesktopAlertManager.Detach();
        //}

        this.DesktopAlertManager.ShowAlert(alert);
    }

    private void OnOutputItemRetryCommandExecuted(object obj)
    {
        AIPromptOutputItemModel aIPromptOutputItemModel = (AIPromptOutputItemModel)obj;

        this.OutputItems.Insert(0, aIPromptOutputItemModel);
    }

    private void OnCommandsViewCommandExecuted(object obj)
    {
        AIPromptCommand aiPromptCommand = (AIPromptCommand)obj;
        string commandName = aiPromptCommand.Text;

        if (this.OutputItems.Count <= 0)
        {
            return;
        }

        AIPromptOutputItemModel previousModel = this.OutputItems[0];

        AIPromptOutputItemModel aiPromptOutputItemModel = new AIPromptOutputItemModel()
        {
            Title = commandName,
            InputText = previousModel.InputText,
            ResponseText = AIService.GetResponse(commandName, previousModel.InputText),
        };

        this.OutputItems.Insert(0, aiPromptOutputItemModel);
    }
}