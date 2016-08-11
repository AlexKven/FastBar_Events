using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using MvvmHelpers;
//using Windows.UI.Xaml;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FastBar_Events.ViewModels
{
    class LoginPageViewModel : BaseViewModel
    {
        public LoginPageViewModel()
        {
            GetLoginCommand = new Command(async () => await Login(), () => Username?.Length > 0 && Password?.Length > 0);
            //GetCreateAccountCommand = new Command()
            
        }

        public Command GetLoginCommand { get; }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set { SetProperty(ref _Username, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }

        private async Task Login()
        {

        }
    }
}
