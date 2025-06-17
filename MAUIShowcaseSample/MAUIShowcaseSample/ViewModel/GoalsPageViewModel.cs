using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing goals page operations including goal tracking, filtering, and management
    /// </summary>
    public class GoalsPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// List of segment titles for goal status filtering
        /// </summary>
        private List<SfSegmentItem> segmentTitle;

        /// <summary>
        /// Currently selected segment index
        /// </summary>
        private int selectedSegmentIndex;

        /// <summary>
        /// Data store service for goal operations
        /// </summary>
        private readonly DataStore _dataStore;

        /// <summary>
        /// Collection of goals for the selected segment
        /// </summary>
        private ObservableCollection<Goal> selectedSegmentData = new ObservableCollection<Goal>();

        /// <summary>
        /// Collection of summarized goal data for display
        /// </summary>
        private ObservableCollection<SummarizedGoalData> goalData = new();

        /// <summary>
        /// Monthly contribution amount
        /// </summary>
        private double monthlyContribution;

        /// <summary>
        /// Number of active goals
        /// </summary>
        private double activeGoals;

        /// <summary>
        /// Number of completed goals
        /// </summary>
        private double completedGoals;

        /// <summary>
        /// Indicates if the page is enabled for user interaction
        /// </summary>
        private bool isPageEnabled = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the segment titles for goal status filtering
        /// </summary>
        /// <value>List of SfSegmentItem objects representing filter options</value>
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

        /// <summary>
        /// Gets or sets the selected segment index for goal filtering
        /// </summary>
        /// <value>Integer representing the selected filter index (0=Active, 1=Completed)</value>
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

        /// <summary>
        /// Gets or sets the collection of goals for the selected segment
        /// </summary>
        /// <value>Observable collection of Goal objects</value>
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

        /// <summary>
        /// Gets or sets the collection of summarized goal data for display
        /// </summary>
        /// <value>Observable collection of SummarizedGoalData objects</value>
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

        /// <summary>
        /// Gets or sets the monthly contribution amount
        /// </summary>
        /// <value>Double representing the total monthly contributions</value>
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

        /// <summary>
        /// Gets or sets the number of active goals
        /// </summary>
        /// <value>Double representing the count of active goals</value>
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

        /// <summary>
        /// Gets or sets the number of completed goals
        /// </summary>
        /// <value>Double representing the count of completed goals</value>
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

        /// <summary>
        /// Gets or sets the currency symbol for display
        /// </summary>
        /// <value>String representing the user's currency symbol</value>
        public string CurrencySymbol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the page is enabled for user interaction
        /// </summary>
        /// <value>Boolean indicating if the page is enabled</value>
        public bool IsPageEnabled
        {
            get => isPageEnabled;
            set
            {
                if (isPageEnabled != value)
                {
                    isPageEnabled = value;
                    OnPropertyChanged(nameof(IsPageEnabled));
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event for the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the GoalsPageViewModel class
        /// </summary>
        /// <param name="userDataService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for goal operations</param>
        public GoalsPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _dataStore = dataStore;

            // Initialize segment titles for goal status filtering
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "Active"},
                new SfSegmentItem() {Text = "Completed"}
            };

            // Set default segment to "Active"
            SelectedSegmentIndex = 0;

            // Get user's currency symbol
            CurrencySymbol = userDataService.GetUserCurrencySymbol(userDataService.LoggedInAccount);

            // Load initial data
            SelectedSegmentData = dataStore.GetGoalsData(SegmentTitle[SelectedSegmentIndex].Text);
            GoalData = GetSummarizedGoalData(selectedSegmentData);
            UpdateCardValues(dataStore.GetGoalsData());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the goals data based on the selected segment
        /// </summary>
        /// <param name="selectedSegment">Optional segment filter, uses current selection if null</param>
        public void UpdateGoalsData(string? selectedSegment = null)
        {
            // Use current segment if none specified
            if (selectedSegment == null)
            {
                selectedSegment = SegmentTitle[SelectedSegmentIndex].Text;
            }

            // Update data collections
            SelectedSegmentData = _dataStore.GetGoalsData(selectedSegment);
            GoalData = GetSummarizedGoalData(selectedSegmentData);
        }

        /// <summary>
        /// Updates the card values for monthly contribution, active goals, and completed goals
        /// </summary>
        /// <param name="goalData">Optional goal data collection, fetches fresh data if null</param>
        public void UpdateCardValues(ObservableCollection<Goal>? goalData = null)
        {
            // Define current month date range
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
            double monthlyContribution = 0;

            // Get goal data if not provided
            if (goalData == null)
            {
                goalData = _dataStore.GetGoalsData();
            }

            // Calculate monthly contribution from all goals
            foreach (var goal in goalData)
            {
                monthlyContribution = monthlyContribution + goal.Transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).Sum(t => t.ContributionAmount);
            }

            // Update card values
            MonthlyContribution = monthlyContribution;
            ActiveGoals = goalData.Where(t => t.IsCompleted == false).Count();
            CompletedGoals = goalData.Where(t => t.IsCompleted == true).Count();
        }

        /// <summary>
        /// Opens or closes the popup for a specific goal
        /// </summary>
        /// <param name="goalId">ID of the goal to toggle popup for</param>
        public void OpenPopup(double goalId)
        {
            // Toggle popup for selected goal and close others
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

        /// <summary>
        /// Deletes the goal that has its popup open
        /// </summary>
        public async void DeleteGoal()
        {
            // Get IDs of goals with open popups
            List<double> goalIds = GoalData.Where(t => t.IsPopupOpen == true).Select(t => t.GoalId).ToList();

            // Attempt to delete goals
            if (_dataStore.DeleteTransactions(goalIds, "Goal"))
            {
                // Refresh data after successful deletion
                UpdateGoalsData();
                UpdateCardValues();
                await Application.Current.MainPage.DisplayAlert("Success", "Goal deleted successfully", "Okay");
            }
            else
            {
                // Show error message if deletion fails
                await Application.Current.MainPage.DisplayAlert("Failed", "Deleting Goal failed", "Okay");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates summarized goal data from filtered goal collection
        /// </summary>
        /// <param name="filterData">Filtered goal data collection</param>
        /// <returns>Observable collection of SummarizedGoalData objects</returns>
        private ObservableCollection<SummarizedGoalData> GetSummarizedGoalData(ObservableCollection<Goal> filterData)
        {
            var summarizedData = new ObservableCollection<SummarizedGoalData>();
            var elementCount = 1;

            // Process each goal to create summarized data
            foreach (var data in filterData)
            {
                // Get goal icon and color
                var categoryIcon = DataHelper.GetGoalIcon(data.GoalTitle);
                Color categoryColor = DataHelper.GetColorCode(elementCount);

                // Calculate remaining amount and utilization percentage
                double remainingAmount = data.GoalAmount - data.ContributionAmount;
                double utilizedPercent = Math.Round(((data.ContributionAmount / data.GoalAmount) * 100), 1, MidpointRounding.ToZero);

                // Create summarized goal data object
                summarizedData.Add(new SummarizedGoalData() 
                { 
                    GoalId = data.GoalId, 
                    GoalTitle = data.GoalTitle, 
                    GoalPriority = data.GoalPriority, 
                    GoalDate = data.GoalDate, 
                    GoalAmount = data.GoalAmount, 
                    CurrencySymbol = this.CurrencySymbol, 
                    Icon = categoryIcon, 
                    IconColor = categoryColor, 
                    ContributionAmount = data.ContributionAmount, 
                    RemainingAmount = remainingAmount, 
                    Utilization = utilizedPercent, 
                    Remarks = data.Remarks, 
                    RemainingDays = (data.GoalDate - DateTime.Today).Days, 
                    Transactions = data.Transactions 
                });
                elementCount++;
            }
            return summarizedData;
        }

        #endregion
    }

    /// <summary>
    /// Represents summarized goal data for display purposes
    /// </summary>
    public class SummarizedGoalData : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Unique goal identifier
        /// </summary>
        private double goalId;

        /// <summary>
        /// Goal title/name
        /// </summary>
        private string? goalTitle;

        /// <summary>
        /// Goal priority level
        /// </summary>
        private GoalPriority goalPriority;

        /// <summary>
        /// Target goal amount
        /// </summary>
        private double? goalAmount;

        /// <summary>
        /// Goal target date
        /// </summary>
        private DateTime goalDate;

        /// <summary>
        /// Goal remarks/notes
        /// </summary>
        private string? goalRemarks;

        /// <summary>
        /// Goal completion status
        /// </summary>
        private bool isCompleted;

        /// <summary>
        /// Currency symbol for display
        /// </summary>
        private string? currencySymbol;

        /// <summary>
        /// Current contribution amount
        /// </summary>
        private double contributionAmount;

        /// <summary>
        /// Remaining amount to reach goal
        /// </summary>
        private double remainingAmount;

        /// <summary>
        /// Remaining days to goal date
        /// </summary>
        private double remainingDays;

        /// <summary>
        /// Goal utilization percentage
        /// </summary>
        private double utilization;

        /// <summary>
        /// Collection of goal transactions
        /// </summary>
        private ObservableCollection<GoalsTransaction> transactions;

        /// <summary>
        /// Goal icon
        /// </summary>
        private string? icon;

        /// <summary>
        /// Goal icon color
        /// </summary>
        private Color? iconColor;

        /// <summary>
        /// Popup open state
        /// </summary>
        private bool isPopupOpen;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique goal identifier
        /// </summary>
        /// <value>Double representing the goal ID</value>
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

        /// <summary>
        /// Gets or sets the goal title
        /// </summary>
        /// <value>String representing the goal name</value>
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

        /// <summary>
        /// Gets or sets the goal priority
        /// </summary>
        /// <value>GoalPriority enum value representing the priority level</value>
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

        /// <summary>
        /// Gets or sets the target goal amount
        /// </summary>
        /// <value>Double representing the target amount</value>
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

        /// <summary>
        /// Gets or sets the goal target date
        /// </summary>
        /// <value>DateTime representing when the goal should be achieved</value>
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

        /// <summary>
        /// Gets or sets the goal remarks
        /// </summary>
        /// <value>String representing additional notes about the goal</value>
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

        /// <summary>
        /// Gets or sets the goal completion status
        /// </summary>
        /// <value>Boolean indicating if the goal is completed</value>
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

        /// <summary>
        /// Gets or sets the currency symbol
        /// </summary>
        /// <value>String representing the currency symbol</value>
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

        /// <summary>
        /// Gets or sets the goal icon
        /// </summary>
        /// <value>String representing the icon character or path</value>
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

        /// <summary>
        /// Gets or sets the goal icon color
        /// </summary>
        /// <value>Color object representing the icon color</value>
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

        /// <summary>
        /// Gets or sets the current contribution amount
        /// </summary>
        /// <value>Double representing the amount contributed so far</value>
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

        /// <summary>
        /// Gets or sets the remaining amount to reach the goal
        /// </summary>
        /// <value>Double representing the remaining amount needed</value>
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

        /// <summary>
        /// Gets or sets the goal utilization percentage
        /// </summary>
        /// <value>Double representing the percentage of goal achieved</value>
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

        /// <summary>
        /// Gets or sets the collection of goal transactions
        /// </summary>
        /// <value>Observable collection of GoalsTransaction objects</value>
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

        /// <summary>
        /// Gets or sets the remaining days to goal date
        /// </summary>
        /// <value>Double representing the number of days remaining</value>
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

        /// <summary>
        /// Gets or sets whether the popup is open for this goal
        /// </summary>
        /// <value>Boolean indicating if the popup is open</value>
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

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event for the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SummarizedGoalData class
        /// </summary>
        public SummarizedGoalData()
        {
            // Initialize popup state to closed
            IsPopupOpen = false;
        }

        #endregion
    }
}