using FastBar_Events.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBar_Events.ViewModels
{
    public class EventsPageViewModel : BaseViewModel
    {
        public EventsPageViewModel(User user)
        {
            PageTitle = "Events for " + user.Email;
            Events.AddRange(DatabaseManager.RetrieveEvents());
        }

        private string _PageTitle;
        public string PageTitle
        {
            get { return _PageTitle; }
            set { SetProperty(ref _PageTitle, value); }
        }

        private ObservableRangeCollection<Event> _Events = new ObservableRangeCollection<Event>();
        public ObservableRangeCollection<Event> Events
        {
            get { return _Events; }
        }
    }
}
