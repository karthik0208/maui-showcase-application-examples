using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.View.Dashboard;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAUIShowcaseSample
{
    public partial class DashboardLayoutPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        DataStore _dataStore;

        UserDataService _userDataService;

        private string pageTitle;

        private TransactionDetails transactionDetails;

        private BudgetDetails budgetDetails;

        private SavingsDetails savingDetails;

        private GoalDetails goalDetails;

        private bool isPickerOpen;

        public string PageTitle
        {
            get
            {
                return this.pageTitle;
            }
            set
            {
                this.pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }
        public bool IsPickerOpen
        {
            get
            {
                return this.isPickerOpen;
            }
            set
            {
                this.isPickerOpen = value;
                OnPropertyChanged(nameof(IsPickerOpen));
            }
        }

        public TransactionDetails TransactionDetails
        {
            get
            {
                return this.transactionDetails;
            }
            set
            {
                this.transactionDetails = value;
            }
        }

        public BudgetDetails BudgetDetails
        {
            get
            {
                return this.budgetDetails;
            }
            set
            {
                this.budgetDetails = value;
            }
        }

        public SavingsDetails SavingDetails
        {
            get
            {
                return this.savingDetails;
            }
            set
            {
                this.savingDetails = value;
            }
        }

        public GoalDetails GoalDetails
        {
            get
            {
                return this.goalDetails;
            }
            set
            {
                this.goalDetails = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DashboardLayoutPageViewModel(UserDataService userDataService, DataStore dataStore, string pageTitle)
        {
            PageTitle = pageTitle;
            _userDataService = userDataService;
            _dataStore = dataStore;
            DashboardLogo = "dashboardlogo.png";
            ProfileAvatar = "profileavatar.png";
            IsPickerOpen = false;
            NavigationItemList = new ObservableCollection<DashboardNavigationItems>
            {
                new() { ItemName = "Dashboard", ItemIcon = "\ue716", Command = new RelayCommand(OnDashboardNavigationClicked) },
                new() { ItemName = "Transaction", ItemIcon = "\ue738", Command = new RelayCommand(OnTransactionNavigationClicked) },
                new() { ItemName = "Budget", ItemIcon = "\ue739", Command = new RelayCommand(OnBudgetNavigationClicked) },
                new() { ItemName = "Savings", ItemIcon = "\ue737", Command = new RelayCommand(OnSavingsNavigationClicked) },
                new() { ItemName = "Goal", ItemIcon = "\ue73a", Command = new RelayCommand(OnGoalNavigationClicked) }
            };

            TransactionDetails = new TransactionDetails();
            BudgetDetails = new BudgetDetails();
            SavingDetails = new SavingsDetails();
            GoalDetails = new GoalDetails();
        }

        [ObservableProperty]
        private ObservableCollection<DashboardNavigationItems> navigationItemList;

        [ObservableProperty]
        private string dashboardLogo;

        [ObservableProperty]
        private string profileAvatar;

        [RelayCommand]
        private void OnDashboardNavigationClicked() { }

        [RelayCommand]
        private void OnTransactionNavigationClicked() { }

        [RelayCommand]
        private void OnBudgetNavigationClicked() { }

        [RelayCommand]
        private void OnSavingsNavigationClicked() { }

        [RelayCommand]
        private void OnGoalNavigationClicked() { }

        [RelayCommand]
        private void OnViewProfileClicked() { }

        [RelayCommand]
        private void OnLogoutClicked() { }

        [RelayCommand]
        private void OnAvatarTapped() { }

        public bool OnAddTransactionClicked()
        {
            Transaction transaction = new Transaction()
            {
                TransactionAmount = this.TransactionDetails.TransactionAmount,
                TransactionCategory = this.TransactionDetails.TransactionCategory,
                TransactionDate = DateTime.ParseExact(this.TransactionDetails.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                TransactionDescription = this.TransactionDetails.TransactionRemarks,
                TransactionName = this.TransactionDetails.TransactionSubCategory,
                TransactionType = this.TransactionDetails.TransactionType
            };
            if(TransactionDetails.ButtonText == "Save")
            {
                transaction.TransactionId = TransactionDetails.TransactionId;
                this.TransactionDetails.Clear();
                return _dataStore.UpdateTransaction(transaction, TransactionDetails.TransactionId).Result;
            }
            this.TransactionDetails.Clear();
            return _dataStore.UpdateTransaction(transaction).Result;
        }

        public bool OnAddBudgetClicked()
        {
            Budget budget = new Budget()
            {
                BudgetTitle = this.BudgetDetails.BudgetTitle,
                BudgetAmount = Double.Parse(this.BudgetDetails.BudgetAmount),
                BudgetCategory = this.BudgetDetails.BudgetCategory,
                BudgetDate = DateTime.ParseExact(BudgetDetails.BudgetDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Remarks = BudgetDetails.BudgetRemarks,
                IsCompleted = false
            };
            if(BudgetDetails.ButtonText == "Save")
            {
                budget.BudgetId = this.BudgetDetails.BudgetId;
                this.BudgetDetails.Clear();
                return _dataStore.UpdateBudget(budget, BudgetDetails.BudgetId).Result;
            }
            this.BudgetDetails.Clear();
            return _dataStore.UpdateBudget(budget).Result;
        }

        public async Task<bool> OnAddBSavingsClicked()
        {
            Savings saving = new Savings()
            {
                SavingsDescription = this.SavingDetails.SavingDescription,
                SavingsAmount = this.SavingDetails.SavingAmount,
                SavingsType = this.SavingDetails.SavingType,
                SavingsDate = DateTime.ParseExact(SavingDetails.SavingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                SavingsRemark = SavingDetails.SavingRemarks
            };
            if (SavingDetails.ButtonText == "Save")
            {
                saving.TransactionId = SavingDetails.TransactionId;
                this.SavingDetails.Clear();
                return _dataStore.UpdateSavings(saving, SavingDetails.TransactionId).Result;
            }
            this.SavingDetails.Clear();
            return _dataStore.UpdateSavings(saving).Result;
        }

        public bool OnCreateGoalClicked()
        {
            Goal goalData = new Goal()
            {
                GoalTitle = this.GoalDetails.GoalTitle,
                GoalAmount = Double.Parse(this.GoalDetails.GoalAmount),
                GoalPriority = (GoalPriority)Enum.Parse(typeof(GoalPriority), this.GoalDetails.GoalPriority),
                GoalDate = DateTime.ParseExact(GoalDetails.DeadlineDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Remarks = GoalDetails.GoalRemarks,
                IsCompleted = false,                
                ContributionAmount = 0,
            };
            if(GoalDetails.ButtonText == "Save")
            {
                goalData.GoalId = this.GoalDetails.GoalId;
                goalData.Transactions = _dataStore.GetGoalById(goalData.GoalId).Transactions;
                this.GoalDetails.Clear();
                return _dataStore.UpdateGoal(goalData, GoalDetails.GoalId).Result;
            }
            goalData.Transactions = new ObservableCollection<GoalsTransaction>() { };
            this.GoalDetails.Clear();
            return _dataStore.UpdateGoal(goalData).Result;
        }

        public async Task<bool> TriggerEditSavings(int transactionId)
        {
            try
            {
                var savingData = _dataStore.GetSavingsByTransactionId(transactionId);
                SavingDetails.TransactionId = savingData.TransactionId;
                SavingDetails.SavingDate = savingData.SavingsDate.ToString();
                SavingDetails.SavingDescription = savingData.SavingsDescription;
                SavingDetails.SavingRemarks = savingData.SavingsRemark;
                SavingDetails.SavingType = savingData.SavingsType;
                SavingDetails.ButtonText = "Save";
                SavingDetails.SavingAmount = savingData.SavingsAmount;
                return true;
            }
            catch(Exception e)
            {
                return false;
            }            
        }

        public async Task<bool> TriggerEditTransaction(int transactionId)
        {
            try
            {
                var transactionData = _dataStore.GetTransactionById(transactionId);
                TransactionDetails.TransactionId = transactionData.TransactionId;
                TransactionDetails.TransactionDate = transactionData.TransactionDate.ToString();
                TransactionDetails.TransactionCategory = transactionData.TransactionCategory;
                TransactionDetails.TransactionRemarks = transactionData.TransactionDescription;
                TransactionDetails.TransactionType = transactionData.TransactionType;
                TransactionDetails.ButtonText = "Save";
                TransactionDetails.TransactionAmount = transactionData.TransactionAmount;
                TransactionDetails.TransactionSubCategory = transactionData.TransactionDescription;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> TriggerEditBudget(int budgetId)
        {
            try
            {
                var budgetData = _dataStore.GetBudgetById(budgetId);
                BudgetDetails.BudgetId = budgetData.BudgetId;
                BudgetDetails.BudgetTitle = budgetData.BudgetTitle;
                BudgetDetails.BudgetAmount = budgetData.BudgetAmount.ToString();
                BudgetDetails.BudgetCategory = budgetData.BudgetCategory;
                BudgetDetails.BudgetDate = budgetData.BudgetDate.ToString();
                BudgetDetails.BudgetRemarks = budgetData.Remarks;
                BudgetDetails.ButtonText = "Save";
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> TriggerEditGoal(int goalId)
        {
            try
            {
                var goalData = _dataStore.GetGoalById(goalId);
                GoalDetails.GoalId = goalData.GoalId;
                GoalDetails.GoalTitle = goalData.GoalTitle;
                GoalDetails.GoalAmount = goalData.GoalAmount.ToString();
                GoalDetails.GoalPriority = goalData.GoalPriority.ToString();
                GoalDetails.DeadlineDate = goalData.GoalDate.ToString();
                GoalDetails.GoalRemarks = goalData.Remarks;
                GoalDetails.ButtonText = "Save";
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public class DashboardNavigationItems
    {
        public string? ItemName { get; set; }
        public string? ItemIcon { get; set; }
        public ICommand? Command { get; set; }
    }

    public class TransactionDetails : INotifyPropertyChanged
    {

        private string transactionType;

        private string transactionCategory;

        private List<string> transactionCategoryOptions;

        private string transactionSubCategory;

        private List<string> transactionSubCategoryOptions;

        private string transactionAmount;

        private string transactionDate;

        private string transactionRemarks;

        public List<string> TransactionTypeOptions { get; set; }

        public List<string> TransactionCategoryOptions
        {
            get
            {
                return this.transactionCategoryOptions;
            }
            set
            {
                this.transactionCategoryOptions = value;
                OnPropertyChanged(nameof(TransactionCategoryOptions));
            }
        }

        public List<string> TransactionSubCategoryOptions
        {
            get
            {
                return this.transactionSubCategoryOptions;
            }
            set
            {
                this.transactionSubCategoryOptions = value;
                OnPropertyChanged(nameof(TransactionSubCategoryOptions));
            }
        }

        public string TransactionType
        {
            get => transactionType;
            set
            {
                if (transactionType != value)
                {
                    transactionType = value;
                    OnPropertyChanged(nameof(TransactionType));
                    TransactionCategoryOptions = GetCategoryByType(transactionType);
                }
            }
        }
        
        public string TransactionCategory
        {
            get => transactionCategory;
            set
            {
                if (transactionCategory != value)
                {
                    transactionCategory = value;
                    OnPropertyChanged(nameof(TransactionCategory));
                    TransactionSubCategoryOptions = GetSubCategoriesByCategory(transactionCategory, TransactionType);
                }
            }
        }
       
        public string TransactionSubCategory
        {
            get => transactionSubCategory;
            set
            {
                if (transactionSubCategory != value)
                {
                    transactionSubCategory = value;
                    OnPropertyChanged(nameof(TransactionSubCategory));
                }
            }
        }
        
        public string TransactionAmount
        {
            get => transactionAmount;
            set
            {
                if (transactionAmount != value)
                {
                    transactionAmount = value;
                    OnPropertyChanged(nameof(TransactionAmount));
                }
            }
        }
        
        public string TransactionDate
        {
            get => transactionDate;
            set
            {
                if (transactionDate != value)
                {
                    transactionDate = value != string.Empty ? DateTime.Parse(value).ToString("dd/MM/yyyy"): value ;
                    OnPropertyChanged(nameof(TransactionDate));
                }
            }
        }
        
        public string TransactionRemarks
        {
            get => transactionRemarks;
            set
            {
                if (transactionRemarks != value)
                {
                    transactionRemarks = value;
                    OnPropertyChanged(nameof(TransactionRemarks));
                }
            }
        }
      
        public int TransactionId { get; set; }

        public string ButtonText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TransactionDetails()
        {
            TransactionTypeOptions = new List<string>() { "Income", "Expense" };
            ButtonText = "Add";
        }

        private static List<string> GetCategoryByType(string transactionType)
        {
            if (transactionType == "Expense")
            {
                return Enum.GetNames(typeof(ExpenseCategory)).ToList();
            }
            else
            {
                return Enum.GetNames(typeof(IncomeCategory)).ToList();
            }
        }


        private static List<string> GetSubCategoriesByCategory(string category, string transactionType)
        {
            if (transactionType == "Expense")
            {
                if (Enum.TryParse<ExpenseCategory>(category, out var expenseCategory))
                {
                    return expenseCategory switch
                    {
                        ExpenseCategory.Food => Enum.GetNames(typeof(ExpenseSubCategory))
                                                    .Where(x => x.Contains("Groceries") || x.Contains("RestaurantDinner") || x.Contains("CafeteriaLunch") || x.Contains("SnacksPurchase"))
                                                    .ToList(),
                        ExpenseCategory.Transportation => Enum.GetNames(typeof(ExpenseSubCategory))
                                                               .Where(x => x.Contains("Fuel") || x.Contains("BusTicket"))
                                                               .ToList(),
                        ExpenseCategory.HealthCare => Enum.GetNames(typeof(ExpenseSubCategory))
                                                          .Where(x => x.Contains("DoctorVisit") || x.Contains("PharmacyExpenses"))
                                                          .ToList(),
                        ExpenseCategory.Bills => Enum.GetNames(typeof(ExpenseSubCategory))
                                                     .Where(x => x.Contains("BroadbandBill") || x.Contains("MobileRecharge") || x.Contains("NetflixSubscription"))
                                                     .ToList(),
                        ExpenseCategory.Utilities => Enum.GetNames(typeof(ExpenseSubCategory))
                                                         .Where(x => x.Contains("ElectricityBill") || x.Contains("WaterBill"))
                                                         .ToList(),
                        ExpenseCategory.Mortgage => Enum.GetNames(typeof(ExpenseSubCategory))
                                                        .Where(x => x.Contains("ApartmentRent") || x.Contains("HomeLoanEMI"))
                                                        .ToList(),
                        ExpenseCategory.Shopping => Enum.GetNames(typeof(ExpenseSubCategory))
                                                        .Where(x => x.Contains("ClothingPurchase") || x.Contains("ElectronicsPurchase"))
                                                        .ToList(),
                        ExpenseCategory.Insurance => Enum.GetNames(typeof(ExpenseSubCategory))
                                                         .Where(x => x.Contains("HealthInsurance"))
                                                         .ToList(),
                        ExpenseCategory.Others => Enum.GetNames(typeof(ExpenseSubCategory))
                                                      .Where(x => x.Contains("MovieTicket") || x.Contains("OnlineCourse") || x.Contains("GameSubscription") || x.Contains("ConcertTicket"))
                                                      .ToList(),
                        _ => new List<string>()
                    };
                }
            }
            else if (transactionType == "Income")
            {
                if (Enum.TryParse<IncomeCategory>(category, out var incomeCategory))
                {
                    return incomeCategory switch
                    {
                        IncomeCategory.Salary => Enum.GetNames(typeof(IncomeSubCategory))
                                                     .Where(x => x.Contains("MonthlySalary"))
                                                     .ToList(),
                        IncomeCategory.ExtraIncome => Enum.GetNames(typeof(IncomeSubCategory))
                                                          .Where(x => x.Contains("AnnualBonus"))
                                                          .ToList(),
                        IncomeCategory.Freelance => Enum.GetNames(typeof(IncomeSubCategory))
                                                        .Where(x => x.Contains("BlogWriting") || x.Contains("SideProjectPayment"))
                                                        .ToList(),
                        IncomeCategory.Interest => Enum.GetNames(typeof(IncomeSubCategory))
                                                       .Where(x => x.Contains("SavingsInterest") || x.Contains("FixedDepositInterest"))
                                                       .ToList(),
                        IncomeCategory.Dividends => Enum.GetNames(typeof(IncomeSubCategory))
                                                        .Where(x => x.Contains("StockDividends") || x.Contains("MutualFundDividend"))
                                                        .ToList(),
                        _ => new List<string>()
                    };
                }
            }

            return new List<string>(); // Return empty if unmatched
        }

        public void Clear()
        {
            TransactionType = TransactionCategory = TransactionSubCategory = TransactionAmount = TransactionDate = TransactionRemarks = string.Empty;
            TransactionCategoryOptions.Clear();
            TransactionSubCategoryOptions.Clear();
        }    
    }

    public class BudgetDetails : INotifyPropertyChanged
    {
        private string budgetTitle;

        private string? budgetCategory;

        private string budgetAmount;

        private string budgetDate;

        private string budgetRemarks;

        private List<string> budgetCategoryOptions;

        private string buttonText;

        private int budgdetId;

        public string BudgetTitle
        {
            get
            {
                return this.budgetTitle;
            }
            set
            {
                this.budgetTitle = value;
                OnPropertyChanged(nameof(BudgetTitle));
            }
        }

        public string? BudgetCategory
        {
            get
            {
                return this.budgetCategory;
            }
            set
            {
                this.budgetCategory = value;
                OnPropertyChanged(nameof(BudgetCategory));
            }
        }

        public string BudgetAmount
        {
            get
            {
                return this.budgetAmount;
            }
            set
            {
                this.budgetAmount = value;
                OnPropertyChanged(nameof(BudgetAmount));
            }
        }

        public string BudgetDate
        {
            get
            {
                return this.budgetDate;
            }
            set
            {
                if (budgetDate != value)
                {
                    budgetDate = value != string.Empty ? DateTime.Parse(value).ToString("dd/MM/yyyy") : value;
                    OnPropertyChanged(nameof(BudgetDate));
                }
            }
        }

        public string BudgetRemarks
        {
            get
            {
                return this.budgetRemarks;
            }
            set
            {
                this.budgetRemarks = value;
                OnPropertyChanged(nameof(BudgetRemarks));
            }
        }

        public List<string> BudgetCategoryOptions
        {
            get
            {
                return this.budgetCategoryOptions;
            }
            set
            {
                this.budgetCategoryOptions = value;
            }
        }

        public string ButtonText
        {
            get
            {
                return this.buttonText;
            }
            set
            {
                this.buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public int BudgetId
        {
            get
            {
                return this.budgdetId;
            }
            set
            {
                this.budgdetId = value;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BudgetDetails()
        {
            BudgetCategoryOptions = DataStore.BudgetCategory;
        }
        public void Clear()
        {
            BudgetAmount = BudgetDate = BudgetTitle = BudgetRemarks = string.Empty;
            BudgetCategory = null;
        }
    }

    public class GoalDetails : INotifyPropertyChanged
    {
        private int goalId;

        private string goalTitle;

        private string? goalPriority;

        private string goalAmount;

        private string deadlineDate;

        private string goalRemarks;

        private string buttonText;

        private List<string> goalPriorityOptions;

        public string GoalTitle
        {
            get
            {
                return this.goalTitle;
            }
            set
            {
                this.goalTitle = value;
                OnPropertyChanged(nameof(GoalTitle));
            }
        }

        public string? GoalPriority
        {
            get
            {
                return this.goalPriority;
            }
            set
            {
                this.goalPriority = value;
                OnPropertyChanged(nameof(GoalPriority));
            }
        }

        public string GoalAmount
        {
            get
            {
                return this.goalAmount;
            }
            set
            {
                this.goalAmount = value;
                OnPropertyChanged(nameof(GoalAmount));
            }
        }

        public string DeadlineDate
        {
            get
            {
                return this.deadlineDate;
            }
            set
            {
                if (deadlineDate != value)
                {
                    deadlineDate = value != string.Empty ? DateTime.Parse(value).ToString("dd/MM/yyyy") : value;
                    OnPropertyChanged(nameof(DeadlineDate));
                }
            }
        }

        public string GoalRemarks
        {
            get
            {
                return this.goalRemarks;
            }
            set
            {
                this.goalRemarks = value;
                OnPropertyChanged(nameof(GoalRemarks));
            }
        }

        public List<string> GoalPriorityOptions
        {
            get
            {
                return this.goalPriorityOptions;
            }
            set
            {
                this.goalPriorityOptions = value;
            }
        }

        public int GoalId
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

        public string ButtonText
        {
            get
            {
                return this.buttonText;
            }
            set
            {
                this.buttonText = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GoalDetails()
        {
            GoalPriorityOptions = Enum.GetNames(typeof(GoalPriority)).ToList();
        }

        public void Clear()
        {
            GoalAmount = GoalTitle = DeadlineDate = GoalRemarks = GoalPriority = string.Empty;
        }
    }


    public class SavingsDetails : INotifyPropertyChanged
    {
        private string savingDescription;

        private string? savingType;

        private string savingAmount;

        private string savingDate;

        private string savingRemarks;

        private List<string> savingTypeOptions;

        public string SavingDescription
        {
            get
            {
                return this.savingDescription;
            }
            set
            {
                this.savingDescription = value;
                OnPropertyChanged(nameof(SavingDescription));
            }
        }

        public string? SavingType
        {
            get
            {
                return this.savingType;
            }
            set
            {
                this.savingType = value;
                OnPropertyChanged(nameof(SavingType));
            }
        }

        public string SavingAmount
        {
            get
            {
                return this.savingAmount;
            }
            set
            {
                this.savingAmount = value;
                OnPropertyChanged(nameof(SavingAmount));
            }
        }

        public string SavingDate
        {
            get
            {
                return this.savingDate;
            }
            set
            {
                if (savingDate != value)
                {
                    savingDate = value != string.Empty ? DateTime.Parse(value).ToString("dd/MM/yyyy") : value;
                    OnPropertyChanged(nameof(SavingDate));
                }
            }
        }

        public string SavingRemarks
        {
            get
            {
                return this.savingRemarks;
            }
            set
            {
                this.savingRemarks = value;
                OnPropertyChanged(nameof(SavingRemarks));
            }
        }

        public List<string> SavingTypeOptions
        {
            get
            {
                return this.savingTypeOptions;
            }
            set
            {
                this.savingTypeOptions = value;
            }
        }

        public int TransactionId { get; set; }

        public string ButtonText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SavingsDetails()
        {
            ButtonText = "Add";
            SavingTypeOptions = new List<string>()
            {
                "Deposit",
                "Withdrawal"
            };
        }
        public void Clear()
        {
            ButtonText = "Add";
            SavingAmount = SavingDescription = SavingDate = SavingRemarks = SavingType = string.Empty;
        }
    }
}