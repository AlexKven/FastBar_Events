using FastBar_Events.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FastBar_Events
{
    public class App : Application
    {
        public App()
        {
            DatabaseManager.Initialize();
            var startPage = new LoginPage(true);
            MainPage = new NavigationPage(startPage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task Navigate(Page navigateTo)
        {
            await (MainPage as NavigationPage)?.PushAsync(navigateTo);
        }

        public async Task NavigateBack()
        {
            await (MainPage as NavigationPage)?.PopAsync();
        }
    }
}
