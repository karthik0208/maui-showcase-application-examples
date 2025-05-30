using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class GoalsPageViewModel : INotifyPropertyChanged
    {
        private List<SfSegmentItem> segmentTitle;

        private int selectedSegmentIndex;

        private readonly DataStore _dataStore;

        private ObservableCollection<Goal> selectedSegmentData = new ObservableCollection<Goal>();

        private ObservableCollection<SummarizedGoalData> goalData = new();

        private double monthlyContribution;

        private double activeGoals;

        private double completedGoals;

        public List<SfSegmentItem> SegmentTitle
        {
            get
            {
                return this.segmentTitle;
            }
            set
            {
                this.segmentTitle = value;
            }
        }

        public int SelectedSegmentIndex
        {
            get
            {
                return this.selectedSegmentIndex;
            }
            set
            {
                this.selectedSegmentIndex = value;
                OnPropertyChanged(nameof(SelectedSegmentIndex));
            }
        }

        public ObservableCollection<Goal> SelectedSegmentData
        {
            get
            {
                return this.selectedSegmentData;
            }
            set
            {
                this.selectedSegmentData = value;
                OnPropertyChanged(nameof(SelectedSegmentData));
            }
        }

        public ObservableCollection<SummarizedGoalData> GoalData
        {
            get
            {
                return this.goalData;
            }
            set
            {
                this.goalData = value;
                OnPropertyChanged(nameof(GoalData));
            }
        }

        public double MonthlyContribution
        {
            get
            {
                return this.monthlyContribution;
            }
            set
            {
                this.monthlyContribution = value;
                OnPropertyChanged(nameof(MonthlyContribution));
            }
        }

        public double ActiveGoals
        {
            get
            {
                return this.activeGoals;
            }
            set
            {
                this.activeGoals = value;
                OnPropertyChanged(nameof(ActiveGoals));
            }
        }

        public double CompletedGoals
        {
            get
            {
                return this.completedGoals;
            }
            set
            {
                this.completedGoals = value;
                OnPropertyChanged(nameof(CompletedGoals));
            }
        }

        public string CurrencySymbol
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GoalsPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _dataStore = dataStore;
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "Active"},
                new SfSegmentItem() {Text = "Completed"}
            };
            SelectedSegmentIndex = 0;
            CurrencySymbol = userDataService.GetUserCurrencySymbol(userDataService.LoggedInAccount);
            SelectedSegmentData = dataStore.GetGoalsData(SegmentTitle[SelectedSegmentIndex].Text);
            GoalData = GetSummarizedGoalData(selectedSegmentData);
            UpdateCardValues(dataStore.GetGoalsData());
        }

        public void UpdateGoalsData(string? selectedSegment = null)
        {
            if (selectedSegment == null)
            {
                selectedSegment = SegmentTitle[SelectedSegmentIndex].Text;
            }
            SelectedSegmentData = _dataStore.GetGoalsData(selectedSegment);
            GoalData = GetSummarizedGoalData(selectedSegmentData);
        }

        private ObservableCollection<SummarizedGoalData> GetSummarizedGoalData(ObservableCollection<Goal> filterData)
        {
            var summarizedData = new ObservableCollection<SummarizedGoalData>();
            var elementCount = 1;
            foreach (var data in filterData)
            {
                var categoryIcon = DataHelper.GetGoalIcon(data.GoalTitle);
                Color categoryColor = DataHelper.GetColorCode(elementCount);
                double remainingAmount = data.GoalAmount - data.ContributionAmount;
                double utilizedPercent = Math.Round(((data.ContributionAmount / data.GoalAmount) * 100), 1, MidpointRounding.ToZero);
                summarizedData.Add(new SummarizedGoalData() { GoalId = data.GoalId, GoalTitle = data.GoalTitle, GoalPriority = data.GoalPriority, GoalDate = data.GoalDate, GoalAmount = data.GoalAmount, CurrencySymbol = this.CurrencySymbol, Icon = categoryIcon, IconColor = categoryColor, ContributionAmount = data.ContributionAmount, RemainingAmount = remainingAmount, Utilization = utilizedPercent, Remarks = data.Remarks, RemainingDays = (data.GoalDate - DateTime.Today).Days, Transactions = data.Transactions });
                elementCount++;
            }
            return summarizedData;
        }

        public void UpdateCardValues(ObservableCollection<Goal>? goalData = null)
        {
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
            double monthlyContribution = 0;
            if (goalData == null)
            {
                goalData = _dataStore.GetGoalsData();
            }
            foreach(var goal in goalData)
            {
                monthlyContribution = monthlyContribution + goal.Transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).Sum(t => t.ContributionAmount);
            }
            MonthlyContribution = monthlyContribution;
            ActiveGoals = goalData.Where(t => t.IsCompleted == false).Count();
            CompletedGoals = goalData.Where(t => t.IsCompleted == true).Count();
        }

       

        public void OpenPopup(double goalId)
        {
            foreach (var data in GoalData)
            {
                if (data.GoalId == goalId)
                {
                    if (data.IsPopupOpen == true)
                    {
                        data.IsPopupOpen = false;
                    }
                    else
                    {
                        data.IsPopupOpen = true;
                    }
                }
                else
                {
                    data.IsPopupOpen = false;
                }
            }
        }

        public async void DeleteGoal()
        {
            List<double> goalIds = GoalData.Where(t => t.IsPopupOpen == true).Select(t => t.GoalId).ToList();
            if (_dataStore.DeleteTransactions(goalIds, "Goal"))
            {
                UpdateGoalsData();
                UpdateCardValues();
                await Application.Current.MainPage.DisplayAlert("Success", "Goal deleted successfully", "Okay");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Deleting Goal failed", "Okay");
            }
        }
    }

    public class SummarizedGoalData : INotifyPropertyChanged
    {
        private double goalId;

        private string? goalTitle;

        private GoalPriority goalPriority;

        private double? goalAmount;

        private DateTime goalDate;

        private string? goalRemarks;

        private bool isCompleted;

        private string? currencySymbol;

        private double contributionAmount;

        private double remainingAmount;

        private double remainingDays;

        private double utilization;

        private ObservableCollection<GoalsTransaction> transactions;

        private string? icon;

        private Color? iconColor;

        private bool isPopupOpen;

        public double GoalId
        {
            get
            {
                return this.goalId;
            }
            set
            {
                this.goalId = value;
            }
        }

        public string? GoalTitle
        {
            get
            {
                return this.goalTitle;
            }
            set
            {
                this.goalTitle = value;
            }
        }

        public GoalPriority GoalPriority
        {
            get
            {
                return this.goalPriority;
            }
            set
            {
                this.goalPriority = value;
            }
        }

        public double? GoalAmount
        {
            get
            {
                return this.goalAmount;
            }
            set
            {
                this.goalAmount = value;
            }
        }

        public DateTime GoalDate
        {
            get
            {
                return this.goalDate;
            }
            set
            {
                this.goalDate = value;
            }
        }

        public string? Remarks
        {
            get
            {
                return this.goalRemarks;
            }
            set
            {
                this.goalRemarks = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this.isCompleted;
            }
            set
            {
                this.isCompleted = value;
            }
        }

        public string? CurrencySymbol
        {
            get
            {
                return this.currencySymbol;
            }
            set
            {
                this.currencySymbol = value;
            }
        }

        public string? Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
            }
        }

        public Color? IconColor
        {
            get
            {
                return this.iconColor;
            }
            set
            {
                this.iconColor = value;
            }
        }

        public double ContributionAmount
        {
            get
            {
                return this.contributionAmount;
            }
            set
            {
                this.contributionAmount = value;
            }
        }

        public double RemainingAmount
        {
            get
            {
                return this.remainingAmount;
            }
            set
            {
                this.remainingAmount = value;
            }
        }

        public double Utilization
        {
            get
            {
                return this.utilization;
            }
            set
            {
                this.utilization = value;
            }
        }

        public ObservableCollection<GoalsTransaction> Transactions
        {
            get
            {
                return this.transactions;
            }
            set
            {
                this.transactions = value;
            }
        }

        public double RemainingDays
        {
            get
            {
                return this.remainingDays;
            }
            set
            {
                this.remainingDays = value;
            }
        }

        public bool IsPopupOpen
        {
            get
            {
                return this.isPopupOpen;
            }
            set
            {
                this.isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SummarizedGoalData()
        {
            IsPopupOpen = false;
        }
    }
}
