namespace MatrixInverterMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var navPage = new NavigationPage(new MatrixInverter());
            MainPage = navPage;
        }
    }
}
