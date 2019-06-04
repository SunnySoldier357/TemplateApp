namespace TemplateApp.UWP
{
    public sealed partial class MainPage
    {
        //* Contructors
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new TemplateApp.App());
        }
    }
}