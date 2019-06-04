using System.Threading.Tasks;

using TemplateApp.ViewModels;

namespace TemplateApp.Services.Navigation
{
    public interface INavigationService
    {
        //* Interface Properties
        BaseViewModel PreviousPageViewModel { get; }

        //* Interface Methods
        Task InitialiseAsync();

        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(object parameter) 
            where TViewModel : BaseViewModel;

        Task RemoveLastFromBackStackAsync();
        Task RemoveBackStackAsync();
    }
}