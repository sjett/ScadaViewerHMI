// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Instrumentation.Metadata;
using HMI.Infrastructure.Presentation.ViewModels;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Linq.Expressions;

namespace HMI.Infrastructure.Presentation.Services
{


    /// <summary>
    /// 信号监视器实现
    /// </summary>
    public class SignalWatcher : ISignalWatcher
    {
        private readonly ICommonControlViewModel model;
        private readonly Expression<Func<string>> resolvedSignal;
        private readonly string propertyName;
        private bool _isStarted;
        private bool _isDisposed;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="signal"></param>
        /// <param name="resolvedSignal"></param>
        public SignalWatcher(ICommonControlViewModel model, Expression<Func<string>> signal, Expression<Func<string>> resolvedSignal)
        {
            Contract.Requires<ArgumentNullException>(model != null, "model");
            Contract.Requires<ArgumentNullException>(signal != null, "signal");
            Contract.Requires<ArgumentNullException>(resolvedSignal != null, "resolvedSignal");

            this.resolvedSignal = resolvedSignal;
            this.model = model;
            this.propertyName = Reflection.GetPropertyName<string>(signal);
        }
        /// <summary>
        /// 启动监视器
        /// </summary>
        public void Start()
        {
            this.model.PropertyChanged += new PropertyChangedEventHandler(this.OnModelPropertyChanged);
            
            this.model.ParentChanged += new EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>>(this.OnParentChanged);

            if ((model.Parent as IProjectPageViewModel) != null)
            {
                (model.Parent as IProjectPageViewModel)!.PropertyChanged += new PropertyChangedEventHandler(this.OnParentPagePropertyChanged);
            }

            this._isStarted = true;
            
            this.model.RaisePropertyChanged<string>(this.resolvedSignal);
        }
        private void OnParentChanged(object? sender, ValueChangedEventArgs<IExplorerNodeViewModel> e)
        {
            IProjectPageViewModel? oldParentPage = e.OldValue as IProjectPageViewModel;
            
            if (oldParentPage != null)
            {
                oldParentPage.PropertyChanged -= new PropertyChangedEventHandler(this.OnParentPagePropertyChanged);
            }
            
            IProjectPageViewModel? newParentPage = e.NewValue as IProjectPageViewModel;
           
            if (newParentPage != null)
            {
                newParentPage.PropertyChanged += new PropertyChangedEventHandler(this.OnParentPagePropertyChanged);
            }

            this.model.RaisePropertyChanged<string>(this.resolvedSignal);
        }
        private void OnParentPagePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            string propertyName;
            if ((propertyName = e.PropertyName) != null)
            {
                if (!(propertyName == "ID") && !(propertyName == "ObjectID") && !(propertyName == "SignalPrefix") && !(propertyName == "UsePageNameAsSignalPrefix"))
                {
                    return;
                }

                this.model.RaisePropertyChanged<string>(this.resolvedSignal);
            }
        }
        private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.propertyName)
            {
                this.model.RaisePropertyChanged<string>(this.resolvedSignal);
                return;
            }
            if (e.PropertyName == "ObjectID" || e.PropertyName == "SignalPrefix")
            {
                this.model.RaisePropertyChanged<string>(this.resolvedSignal);
            }
        }
        /// <summary>
        /// 停止监视器
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Stop()
        {
            if (!this._isStarted)
            {
                throw new InvalidOperationException();
            }
            this.model.PropertyChanged -= new PropertyChangedEventHandler(this.OnModelPropertyChanged);
            
            if ((this.model.Parent as IProjectPageViewModel) != null)
            {
                (this.model.Parent as IProjectPageViewModel)!.PropertyChanged -= new PropertyChangedEventHandler(this.OnParentPagePropertyChanged);
            }
            
            this._isStarted = false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void StopIfStarted()
        {
            if (this._isStarted)
            {
                this.Stop();
            }
        }
        protected virtual void ReleaseAdditionalResources()
        {
        }
        protected virtual void Dispose(bool force)
        {
            if (!this._isDisposed)
            {
                if (force)
                {
                    this.StopIfStarted();
                }
                this.ReleaseAdditionalResources();
            }
            this._isDisposed = true;
        }
    }
}
