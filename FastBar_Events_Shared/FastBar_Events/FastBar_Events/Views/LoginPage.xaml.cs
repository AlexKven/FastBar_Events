using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FastBar_Events.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void CreateAccountButton_Clicked(object sender, EventArgs e)
        {
            //Device.OpenUri(new Uri("https://getfastbar.com/Account/SignUp"));
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
        }
    }
}
