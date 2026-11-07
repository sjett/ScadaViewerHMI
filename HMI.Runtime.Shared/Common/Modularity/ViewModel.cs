// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using HMI.Runtime.Shared.Common;
    using HMI.Runtime.Shared.Common.Internationalization;

    public class ViewModel : PropertyChangedNotifier, INotifyPropertyChanged, IViewModel
    {
        protected IServiceProvider container;

        public ViewModel(IServiceProvider container)
        {
            ArgumentNullException.ThrowIfNull(container);
            this.container = container;
            this.Translations = this.GetLanguageTranslator();
        }

        protected virtual ILanguageTranslator GetLanguageTranslator()
        {
            return this.container.GetService(typeof(ILanguageTranslator)) as ILanguageTranslator
                ?? throw new InvalidOperationException(
                    $"Required runtime service '{typeof(ILanguageTranslator).FullName}' is not registered.");
        }

        public IEditableModel Model { get; set; }

        public ILanguageTranslator Translations { get; private set; }
    }
}
