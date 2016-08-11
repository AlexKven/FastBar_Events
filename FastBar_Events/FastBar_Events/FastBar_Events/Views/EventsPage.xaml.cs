using FastBar_Events.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FastBar_Events.Views
{
    public partial class EventsPage : ContentPage
    {
        private EventsPageViewModel vm = new EventsPageViewModel(DatabaseManager.LoggedInUser);
        public EventsPage()
        {
            InitializeComponent();
            BindingContext = vm;
            ListViewEvents.ItemsSource = vm.Events;
        }
    }
}
