using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MAUIShowcaseSample
{
    public class DashboardLayoutPageViewModel
    {

        private ObservableCollection<DashboardNavigationItems> navigationItemList;


        /// <summary>
        /// Gets or sets the dashboard navigation item list.
        /// </summary>
        public ObservableCollection<DashboardNavigationItems> NavigationItemList { get; set; }


        /// <summary>
        /// Gets or sets the dashboard logo image path.
        /// </summary>
        public string DashboardLogo { get; set; }

        /// <summary>
        /// Gets or sets the dashboard profile image path.
        /// </summary>
        public string ProfileAvatar { get; set; }

        public DashboardLayoutPageViewModel()
        {
            this.DashboardLogo = "dashboardlogo.png";
            this.ProfileAvatar = "profileavatar.png";
            this.NavigationItemList = new ObservableCollection<DashboardNavigationItems>()
            {
                new DashboardNavigationItems() { ItemName = "Dashboard", ItemIcon = "\ue716", Command = new Command(this.OnDashboardNavigationClicked)},
                new DashboardNavigationItems() { ItemName = "Transaction", ItemIcon = "\ue738", Command = new Command(this.OnTransactionNavigationClicked)},
                new DashboardNavigationItems() { ItemName = "Budget", ItemIcon = "\ue739", Command = new Command(this.OnBudgetNavigationClicked)},
                new DashboardNavigationItems() { ItemName = "Savings", ItemIcon = "\ue737", Command = new Command(this.OnSavingsNavigationClicked)},
                new DashboardNavigationItems() { ItemName = "Goal", ItemIcon = "\ue73a",Command = new Command(this.OnGoalNavigationClicked)}
            };
        }

        public void OnViewProfileClicked(object sender, EventArgs e)
        {

        }

        public void OnLogoutClicked(object sender, EventArgs e)
        {

        }

        public void OnAvatarTapped(object sender, TappedEventArgs e)
        {

        }

        public void OnDashboardNavigationClicked()
        {

        }

        public void OnTransactionNavigationClicked()
        {

        }

        public void OnBudgetNavigationClicked()
        {

        }

        public void OnSavingsNavigationClicked()
        {

        }

        public void OnGoalNavigationClicked()
        {

        }
    }

    public class DashboardNavigationItems
    {
        private string? itemName;

        private string? itemIcon;

        private ICommand? command;

        public string? ItemName
        {
            get
            {
                return this.itemName;
            }
            set
            {
                this.itemName = value;
            }
        }

        public string? ItemIcon
        {
            get
            {
                return this.itemIcon;
            }
            set
            {
                this.itemIcon = value;
            }
        }

        public ICommand? Command
        {
            get
            {
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }
    }
}
