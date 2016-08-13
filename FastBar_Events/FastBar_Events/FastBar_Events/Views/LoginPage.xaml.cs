using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using FastBar_Events.ViewModels;

namespace FastBar_Events.Views
{
    public partial class LoginPage : ContentPage
    {
        private bool? IsNarrowLayout = null;
        LoginPageViewModel vm = new LoginPageViewModel();
        private bool AutoLoadEvents;

        public LoginPage(bool automaticallyTryLoadEvents = false)
        {
            InitializeComponent();
            BindingContext = vm;
            AutoLoadEvents = automaticallyTryLoadEvents;
        }

        private void This_Appearing(object sender, EventArgs e)
        {
            IntializePlatformSpecificUI();
            SetLayout();
            if (AutoLoadEvents)
            {
                if (vm.NavigateToEventsCommand.CanExecute(null))
                    vm.NavigateToEventsCommand.Execute(null);
                AutoLoadEvents = false;
            }
        }

        private void This_SizeChanged(object sender, EventArgs e)
        {
            SetLayout();
        }

        private void IntializePlatformSpecificUI()
        {
            Device.OnPlatform(Android: () =>
            {
                //For some strange reason, Android seems to be ignoring placeholder text color.
                //TODO: Investigate why placeholder text color isn't working on Android.
                PasswordBox.PlaceholderColor = UsernameBox.PlaceholderColor = Color.FromHex("DDDDDD");
                LoginButton.TextColor = CreateAccountButton.TextColor = Color.Black;
            });

            LogoImage.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("FastBar_Logo.jpg"),
            Android: ImageSource.FromFile("fastbar_logo.jpg"),
            WinPhone: ImageSource.FromFile("Assets/FastBar_Logo.png"));
        }

        private void SetLayout()
        {
            if ((!IsNarrowLayout.HasValue || !IsNarrowLayout.Value) && Width < 500)
            {
                //The page has a narrow width, and the controls are not set to a narrow layout
                //So set the controls to a narrow layout
                NarrowRow.Height = GridLength.Auto;
                WideColumn.Width = new GridLength(0);
                LoginButton.Margin = new Thickness(10, 4, 10, 4);
                CreateAccountButton.Margin = new Thickness(10, 4, 10, 4);
                LogoImage.WidthRequest = 200;
                LogoImage.HeightRequest = 55;
                DescriptionText.FontSize = 24;
                Grid.SetRow(MovingLayout, 1);
                Grid.SetColumn(MovingLayout, 0);

                IsNarrowLayout = true;
            }
            else if ((!IsNarrowLayout.HasValue || IsNarrowLayout.Value) && Width >= 500)
            {
                //The page has a wide width, and the controls are not set to a wide layout
                //So set the controls to a wide layout
                NarrowRow.Height = new GridLength(0);
                WideColumn.Width = new GridLength(210);
                LoginButton.Margin = new Thickness(0, 4, 10, 4);
                CreateAccountButton.Margin = new Thickness(0, 4, 10, 4);
                LogoImage.WidthRequest = 400;
                LogoImage.HeightRequest = 110;
                DescriptionText.FontSize = 32;
                Grid.SetRow(MovingLayout, 0);
                Grid.SetColumn(MovingLayout, 1);


                IsNarrowLayout = false;
            }
        }
    }
}
