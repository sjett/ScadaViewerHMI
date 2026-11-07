// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Commands;
using HMI.Infrastructure.Resources;
using System.Windows;
using System.Windows.Input;

namespace HMI.Infrastructure.Commanding
{
    public class CommandSinkBinding : CommandBinding
    {
        private sealed class CommandBindingTarget
        {
            public readonly bool IsSupported;
            private readonly FrameworkContentElement contentElement;
            private readonly FrameworkElement element;
            public event RoutedEventHandler Loaded
            {
                add
                {
                    EnsureSupportedElement();
                    if (element != null)
                    {
                        element.Loaded += value;
                        return;
                    }
                    contentElement.Loaded += value;
                }
                remove
                {
                    EnsureSupportedElement();
                    if (element != null)
                    {
                        element.Loaded -= value;
                        return;
                    }
                    contentElement.Loaded -= value;
                }
            }
            public CommandBindingCollection CommandBindings
            {
                get
                {
                    EnsureSupportedElement();
                    if (element != null)
                    {
                        return element.CommandBindings;
                    }
                    return contentElement.CommandBindings;
                }
            }
            public bool IsLoaded
            {
                get
                {
                    EnsureSupportedElement();
                    if (element != null)
                    {
                        return element.IsLoaded;
                    }
                    return contentElement.IsLoaded;
                }
            }
            public CommandBindingTarget(DependencyObject dependencyObject)
            {
                element = dependencyObject as FrameworkElement;
                contentElement = dependencyObject as FrameworkContentElement;
                IsSupported = element != null || contentElement != null;
            }
            void EnsureSupportedElement()
            {
                if (!IsSupported)
                {
                    throw new InvalidOperationException(CommandingErrorTexts.CannotUseInvalidCommmonElement);
                }
            }
        }
        

        private ICommandSink _commandSink;
        public static readonly DependencyProperty CommandSinkProperty = DependencyProperty.RegisterAttached("CommandSink", typeof(ICommandSink), typeof(CommandSinkBinding), new UIPropertyMetadata(null, new PropertyChangedCallback(OnCommandSinkChanged)));
        public ICommandSink CommandSink
        {
            get
            {
                return _commandSink;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(CommandingErrorTexts.CannotSetCommandSinkToNull);
                }
                if (_commandSink != null)
                {
                    throw new InvalidOperationException(CommandingErrorTexts.CannotSetCommandSinkMoreThanOnce);
                }
                _commandSink = value;
                CanExecute += new CanExecuteRoutedEventHandler(CommandSinkBinding_CanExecute);
                Executed += new ExecutedRoutedEventHandler(CommandSinkBinding_Executed);
            }
        }
        public static ICommandSink GetCommandSink(DependencyObject obj)
        {
            return (ICommandSink)obj.GetValue(CommandSinkProperty);
        }
        public static void SetCommandSink(DependencyObject obj, ICommandSink value)
        {
            obj.SetValue(CommandSinkProperty, value);
        }
   
        private void CommandSinkBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool handled;
            e.CanExecute = _commandSink.CanExecuteCommand(e.Command, e.Parameter, out handled);
            e.Handled = handled;
        }
 
        private void CommandSinkBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bool handled;
            _commandSink.ExecuteCommand(e.Command, e.Parameter, out handled);
            e.Handled = handled;
        }

        static void OnCommandSinkChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            ICommandSink commandSink = change.NewValue as ICommandSink;
            if (!DeferBindingUntilLoaded(dependencyObject, commandSink))
            {
                AttachCommandSinkToBindings(dependencyObject, commandSink);
            }
        }

        static RoutedEventHandler? deferredLoadedHandler;
        static bool DeferBindingUntilLoaded(DependencyObject dependencyObject, ICommandSink commandSink)
        {
            var bindingTarget = new CommandBindingTarget(dependencyObject);
            if (bindingTarget.IsSupported && !bindingTarget.IsLoaded)
            {
                deferredLoadedHandler = new RoutedEventHandler((sender, args) =>
                {
                    bindingTarget.Loaded -= deferredLoadedHandler;
                    AttachCommandSinkToBindings(dependencyObject, commandSink);
                });

                bindingTarget.Loaded += deferredLoadedHandler;
                return true;
            }
            return false;
        }

        static void AttachCommandSinkToBindings(DependencyObject dependencyObject, ICommandSink commandSink)
        {
            CommandBindingCollection commandBindings = GetCommandBindings(dependencyObject);
            if (commandBindings == null)
            {
                throw new ArgumentException(CommandingErrorTexts.ElementDoesNotSupportCommandBindings);
            }
            foreach (CommandBinding commandBinding in commandBindings)
            {
                CommandSinkBinding commandSinkBinding = commandBinding as CommandSinkBinding;
                if (commandSinkBinding != null && commandSinkBinding.CommandSink == null)
                {
                    commandSinkBinding.CommandSink = commandSink;
                }
            }
        }
        static CommandBindingCollection GetCommandBindings(DependencyObject dependencyObject)
        {
            CommandBindingTarget bindingTarget = new CommandBindingTarget(dependencyObject);
            if (!bindingTarget.IsSupported)
            {
                return null;
            }
            return bindingTarget.CommandBindings;
        }
    }
}
