// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;
using System.Linq.Expressions;

namespace HMI.Common.Presentation.Events
{
    internal static class PropertySupport
    {
        internal static string ExtractPropertyName<T>(Expression<Func<T>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);
            return expression.Body switch
            {
                MemberExpression memberExpression => memberExpression.Member.Name,
                UnaryExpression { Operand: MemberExpression memberExpression } => memberExpression.Member.Name,
                _ => throw new ArgumentException("The expression must reference a property.", nameof(expression))
            };
        }
    }

    [Serializable]
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                throw new ArgumentNullException("propertyNames");
            }

            foreach (string propertyName in propertyNames)
            {
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            RaisePropertyChanged(propertyName);
        }
    }
    public class PropertyChangedNotifier : NotificationObject, INotifyProperty
    {
        public event PropertyChangingEventHandler? PropertyChanging;

        public void RaisePropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
        public void RaisePropertyChanging(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                throw new ArgumentNullException("propertyNames");
            }
            for (int i = 0; i < propertyNames.Length; i++)
            {
                string propertyName = propertyNames[i];
                this.RaisePropertyChanging(propertyName);
            }
        }
        public void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            string? propertyName = PropertySupport.ExtractPropertyName<T>(propertyExpression);
            this.RaisePropertyChanging(propertyName);
        }
        public new void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
        }
        public new void RaisePropertyChanged(params string[] propertyNames)
        {
            base.RaisePropertyChanged(propertyNames);
        }
        public new void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.RaisePropertyChanged<T>(propertyExpression);
        }
    }
}
