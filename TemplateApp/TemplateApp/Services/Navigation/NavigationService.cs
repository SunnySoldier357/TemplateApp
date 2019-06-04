using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;

using TemplateApp.Services.Settings;
using TemplateApp.ViewModels;
using System.Globalization;

namespace TemplateApp.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        //* Private Properties
        private readonly ISettingsService settingsService;

        private IReadOnlyList<Page> mainNavigationStack =>
            mainPage.Navigation.NavigationStack;

        private NavigationPage mainPage
        {
            get => Application.Current.MainPage as NavigationPage;
            set => Application.Current.MainPage = value;
        }

        //* Public Properties
        public BaseViewModel PreviousPageViewModel
        {
            get
            {
                object viewModel = mainNavigationStack[mainNavigationStack.Count - 2]
                    .BindingContext;
                return viewModel as BaseViewModel;
            }
        }

        //* Constructors
        public NavigationService(ISettingsService settingsService) =>
            this.settingsService = settingsService;

        //* Interface Implementations
        public Task InitialiseAsync() => NavigateToAsync<MainViewModel>();

        public Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel =>
            navigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter)
            where TViewModel : BaseViewModel =>
            navigateToAsync(typeof(TViewModel), parameter);

        public Task RemoveLastFromBackStackAsync()
        {
            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(mainNavigationStack[
                    mainNavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            if (mainPage != null)
            {
                for (int i = 0; i < mainNavigationStack.Count - 1; i++)
                {
                    Page page = mainNavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        //* Private Methods
        private Page createPage(Type viewModelType)
        {
            Type pageType = getPageTypeForViewModel(viewModelType);
            if (pageType == null)
                throw new Exception($"Cannot locate page type for {viewModelType}");

            return Activator.CreateInstance(pageType) as Page;
        }

        private Type getPageTypeForViewModel(Type viewModelType)
        {
            string pageName = viewModelType.FullName.Replace("ViewModel", string.Empty)
                + "Page";
            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            string pageAssemblyName = string.Format(CultureInfo.InvariantCulture,
                "{0}, {1}", pageName, viewModelAssemblyName);

            return Type.GetType(pageAssemblyName);
        }

        private async Task navigateToAsync(Type viewModelType, object parameter)
        {
            Page page = createPage(viewModelType);

            if (mainPage != null)
                await mainPage.PushAsync(page);
            else
                mainPage = new NavigationPage(page);

            await (page.BindingContext as BaseViewModel).InitialiseAsync(parameter);
        }
    }
}