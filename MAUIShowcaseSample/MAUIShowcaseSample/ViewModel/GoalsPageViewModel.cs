using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public void UpdateGoalsData(string selectedSegment)
        {
            SelectedSegmentData = _dataStore.GetGoalsData(selectedSegment);
            GoalData = GetSummarizedGoalData(selectedSegmentData);
        }

        private ObservableCollection<SummarizedGoalData> GetSummarizedGoalData(ObservableCollection<Goal> filterData)
        {
            var summarizedData = new ObservableCollection<SummarizedGoalData>();
            var elementCount = 1;
            foreach (var data in filterData)
            {
                var categoryIcon = GetGoalIcon(data.GoalTitle);
                Color categoryColor = GetColorCode(elementCount);
                double remainingAmount = data.GoalAmount - data.SpentAmount;
                double utilizedPercent = Math.Round(((data.SpentAmount / data.GoalAmount) * 100), 1, MidpointRounding.ToZero);
                summarizedData.Add(new SummarizedGoalData() { GoalTitle = data.GoalTitle, GoalPriority = data.GoalPriority, GoalDate = data.GoalDate, GoalAmount = data.GoalAmount, CurrencySymbol = this.CurrencySymbol, Icon = categoryIcon, IconColor = categoryColor, SpentAmount = data.SpentAmount, RemainingAmount = remainingAmount, Utilization = utilizedPercent, Remarks = data.Remarks });
                elementCount++;
            }
            return summarizedData;
        }

        private void UpdateCardValues(ObservableCollection<Goal> goalData)
        {
            MonthlyContribution = 5000;
            ActiveGoals = goalData.Where(t => t.IsCompleted == false).Count();
            CompletedGoals = goalData.Where(t => t.IsCompleted == true).Count();
        }

        private string GetGoalIcon(string? title)
        {
            string iconCode = "\ue73d";

            if(title.Contains("Vacation", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue73b";
            }
            else if(title.Contains("Car", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue72a";
            }
            else if (title.Contains("Wedding", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue73c";
            }
            else if (title.Contains("Home", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue716";
            }
            return iconCode;
        }

        private Color GetColorCode(int elementCount)
        {
            List<string> colorValues = new List<string>() { "#315A74", "#2196F5", "#963C70", "#EC5C7B" };
            return Color.FromArgb(colorValues[elementCount]);
        }
    }

    public class SummarizedGoalData
    {
        private string? goalTitle;

        private GoalPriority goalPriority;

        private double? goalAmount;

        private DateTime goalDate;

        private string? goalRemarks;

        private bool isCompleted;

        private string? currencySymbol;

        private double spentAmount;

        private double remainingAmount;

        private double utilization;

        private string? icon;

        private Color? iconColor;

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

        public double SpentAmount
        {
            get
            {
                return this.spentAmount;
            }
            set
            {
                this.spentAmount = value;
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
    }
}
