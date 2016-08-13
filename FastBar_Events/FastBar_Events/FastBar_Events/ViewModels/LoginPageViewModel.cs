//MVVM ViewModel for the login page.
using FastBar_Events.Models;
using FastBar_Events.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FastBar_Events.ViewModels
{
    class LoginPageViewModel : BaseViewModel
    {
        public LoginPageViewModel()
        {
            TextboxHeight = Device.OnPlatform<double>(40, 50, 35);
            GetLoginCommand = new Command(async () => await Login(), () => (Username?.Length > 0 && Password?.Length > 0) && !IsBusy);
            GetCreateAccountCommand = new Command(() => Device.OpenUri(new Uri("https://getfastbar.com/Account/SignUp")), () => !IsBusy);
            NavigateToEventsCommand = new Command(() => NavigateToEvents(), () => DatabaseManager.LoggedInUser != null);
            PropertyChanged += LoginPageViewModel_PropertyChanged;
            Reset();
        }

        private void LoginPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBusy")
            {
                GetLoginCommand.ChangeCanExecute();
                GetCreateAccountCommand.ChangeCanExecute();
            }
        }

        private void Reset()
        {
            Message = "Welcome! Please enter your credentials to see your events.";
            Username = "";
            Password = "";
        }

        public Command GetLoginCommand { get; }
        public Command GetCreateAccountCommand { get; }
        public Command NavigateToEventsCommand { get; }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                SetProperty(ref _Username, value);
                GetLoginCommand.ChangeCanExecute();
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                SetProperty(ref _Password, value);
                GetLoginCommand.ChangeCanExecute();
            }
        }

        private double _TextboxHeight;
        public double TextboxHeight
        {
            get { return _TextboxHeight; }
            set
            {
                SetProperty(ref _TextboxHeight, value);
            }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }

        private async Task Login()
        {
            User currentUser;
            try
            {
                IsBusy = true;
                Message = "Logging in...";
                string token = await APIManager.GetToken(Username, Password);
                System.Diagnostics.Debug.WriteLine(token);
                if (token == null)
                {
                    //Failure of some sort
                    Message = "Login unsuccessful. Check your email/password.";
                }
                else
                {
                    Message = "Login success!";
                }
                currentUser = new User();
                currentUser.Token = token;
                currentUser.Email = Username;
                DatabaseManager.LoggedInUser = currentUser;
                NavigateToEvents();
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                Message = $"Error connecting to server. Check your internet connection. Details: {e.Message}";
            }
            finally
            { //Always false your IsBusy in a finally
                IsBusy = false;
            }
        }

        internal async void NavigateToEvents()
        {
            try
            {
                IsBusy = true;
                Message = "Getting your events...";
                Username = DatabaseManager.LoggedInUser.Email;
                var success = await DatabaseManager.TryStoreEventsFromLoggedInUser();
                if (success)
                {
                    Reset(); //In case the user navigates back.
                    await ((App)App.Current).Navigate(new EventsPage());
                }
                else
                {
                    Message = "Error getting your events. Check your connection and credentials, and try again.";
                    Username = DatabaseManager.LoggedInUser.Email;
                }
            }
            catch (Exception e)
            {
                Message = $"Error: {e.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
