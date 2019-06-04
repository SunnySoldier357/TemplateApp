using Xamarin.Forms;

namespace TemplateApp
{
    public partial class App : Application
    {
        //* Constructors
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}