using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;

using TemplateApp.Services.Navigation;

namespace TemplateApp.ViewModels
{
    public abstract class BaseViewModel : BindableObject
    {
        //* Private Properties
        private bool isBusy;

        //* Protected Properties
        protected readonly INavigationService navigationService;

        //* Public Properties
        public bool IsBusy
        {
            get => isBusy;
            set => setProperty(ref isBusy, value);
        }

        //* Constructors
        public BaseViewModel() => 
            navigationService = DependencyService.Get<INavigationService>();

        //* Virtual Methods
        public virtual Task InitialiseAsync(object navigationData) =>
            Task.FromResult(false);

        //* Protected Methods
        protected bool setProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}