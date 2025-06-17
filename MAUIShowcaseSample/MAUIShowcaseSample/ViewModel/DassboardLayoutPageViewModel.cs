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
    /// <summary>
    /// ViewModel for managing dashboard layout page operations including navigation, data management, and form handling
    /// </summary>
    public partial class DashboardLayoutPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Data store service for data operations
        /// </summary>
        DataStore _dataStore;

        /// <summary>
        /// User data service for user-related operations
        /// </summary>
        UserDataService _userDataService;

        /// <summary>
        /// Current page title
        /// </summary>
        private string pageTitle;

        /// <summary>
        /// Transaction details form model
        /// </summary>
        private TransactionDetails transactionDetails;

        /// <summary>
        /// Budget details form model
        /// </summary>
        private BudgetDetails budgetDetails;

        /// <summary>
        /// Savings details form model
        /// </summary>
        private SavingsDetails savingDetails;

        /// <summary>
        /// Goal details form model
        /// </summary>
        private GoalDetails goalDetails;

        /// <summary>
        /// Goal transaction details form model
        /// </summary>
        private GoalTransactionDetails fundDetails;

        /// <summary>
        /// Indicates if picker is open
        /// </summary>
        private bool isPickerOpen;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current page title
        /// </summary>
        /// <value>String representing the page title</value>
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

        /// <summary>
        /// Gets or sets whether the picker is open
        /// </summary>
        /// <value>Boolean indicating if picker is open</value>
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

        /// <summary>
        /// Gets or sets the transaction details form model
        /// </summary>
        /// <value>TransactionDetails object containing transaction form data</value>
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

        /// <summary>
        /// Gets or sets the budget details form model
        /// </summary>
        /// <value>BudgetDetails object containing budget form data</value>
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

        /// <summary>
        /// Gets or sets the savings details form model
        /// </summary>
        /// <value>SavingsDetails object containing savings form data</value>
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

        /// <summary>
        /// Gets or sets the goal details form model
        /// </summary>
        /// <value>GoalDetails object containing goal form data</value>
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

        /// <summary>
        /// Gets or sets the goal transaction details form model
        /// </summary>
        /// <value>GoalTransactionDetails object containing goal transaction form data</value>
        public GoalTransactionDetails FundDetails
        {
            get
            {
                return this.fundDetails;
            }
            set
            {
                this.fundDetails = value;
            }
        }

        #endregion

        #region Observable Properties

        /// <summary>
        /// Gets or sets the navigation item list for dashboard menu
        /// </summary>
        /// <value>Observable collection of DashboardNavigationItems</value>
        [ObservableProperty]
        private ObservableCollection<DashboardNavigationItems> navigationItemList;

        /// <summary>
        /// Gets or sets the dashboard logo image path
        /// </summary>
        /// <value>String representing the logo image path</value>
        [ObservableProperty]
        private string dashboardLogo;

        /// <summary>
        /// Gets or sets the profile avatar image path
        /// </summary>
        /// <value>String representing the avatar image path</value>
        [ObservableProperty]
        private string profileAvatar;

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
        /// Initializes a new instance of the DashboardLayoutPageViewModel class
        /// </summary>
        /// <param name="userDataService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for data operations</param>
        /// <param name="pageTitle">Title of the current page</param>
        public DashboardLayoutPageViewModel(UserDataService userDataService, DataStore dataStore, string pageTitle)
        {
            PageTitle = pageTitle;
            _userDataService = userDataService;
            _dataStore = dataStore;
            DashboardLogo = "dashboardlogo.png";
            ProfileAvatar = "profileavatar.png";
            IsPickerOpen = false;

            // Initialize navigation menu items
            NavigationItemList = new ObservableCollection<DashboardNavigationItems>
            {
                new() { ItemName = "Dashboard", ItemIcon = "\ue716", Command = new RelayCommand(OnDashboardNavigationClicked) },
                new() { ItemName = "Transaction", ItemIcon = "\ue738", Command = new RelayCommand(OnTransactionNavigationClicked) },
                new() { ItemName = "Budget", ItemIcon = "\ue739", Command = new RelayCommand(OnBudgetNavigationClicked) },
                new() { ItemName = "Savings", ItemIcon = "\ue737", Command = new RelayCommand(OnSavingsNavigationClicked) },
                new() { ItemName = "Goal", ItemIcon = "\ue73a", Command = new RelayCommand(OnGoalNavigationClicked) }
            };

            // Initialize form models
            TransactionDetails = new TransactionDetails();
            BudgetDetails = new BudgetDetails();
            SavingDetails = new SavingsDetails();
            GoalDetails = new GoalDetails();
            FundDetails = new GoalTransactionDetails();
        }

        #endregion

        #region Navigation Command Methods

        /// <summary>
        /// Handles dashboard navigation click
        /// </summary>
        [RelayCommand]
        private void OnDashboardNavigationClicked() { }

        /// <summary>
        /// Handles transaction navigation click
        /// </summary>
        [RelayCommand]
        private void OnTransactionNavigationClicked() { }

        /// <summary>
        /// Handles budget navigation click
        /// </summary>
        [RelayCommand]
        private void OnBudgetNavigationClicked() { }

        /// <summary>
        /// Handles savings navigation click
        /// </summary>
        [RelayCommand]
        private void OnSavingsNavigationClicked() { }

        /// <summary>
        /// Handles goal navigation click
        /// </summary>
        [RelayCommand]
        private void OnGoalNavigationClicked() { }

        /// <summary>
        /// Handles view profile click
        /// </summary>
        [RelayCommand]
        private void OnViewProfileClicked() { }

        /// <summary>
        /// Handles logout click
        /// </summary>
        [RelayCommand]
        private void OnLogoutClicked() { }

        /// <summary>
        /// Handles avatar tap
        /// </summary>
        [RelayCommand]
        private void OnAvatarTapped() { }

        #endregion

        #region Data Management Methods

        /// <summary>
        /// Adds or updates a transaction based on form data
        /// </summary>
        /// <returns>True if operation is successful, false otherwise</returns>
        public bool OnAddTransactionClicked()
        {
            try
            {
                // Create transaction object from form data
                Transaction transaction = new Transaction()
                {
                    TransactionAmount = this.TransactionDetails.TransactionAmount,
                    TransactionCategory = this.TransactionDetails.TransactionCategory,
                    TransactionDate = DateTime.ParseExact(this.TransactionDetails.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    TransactionDescription = this.TransactionDetails.TransactionRemarks,
                    TransactionName = this.TransactionDetails.TransactionSubCategory,
                    TransactionType = this.TransactionDetails.TransactionType
                };

                // Update existing transaction or add new one
                if (TransactionDetails.ButtonText == "Save")
                {
                    transaction.TransactionId = TransactionDetails.TransactionId;
                    this.TransactionDetails.Clear();
                    return _dataStore.UpdateTransaction(transaction, TransactionDetails.TransactionId).Result;
                }
                this.TransactionDetails.Clear();
                return _dataStore.UpdateTransaction(transaction).Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds or updates a budget based on form data
        /// </summary>
        /// <returns>True if operation is successful, false otherwise</returns>
        public bool OnAddBudgetClicked()
        {
            try
            {
                // Create budget object from form data
                Budget budget = new Budget()
                {
                    BudgetTitle = this.BudgetDetails.BudgetTitle,
                    BudgetAmount = Double.Parse(this.BudgetDetails.BudgetAmount),
                    BudgetCategory = this.BudgetDetails.BudgetCategory,
                    BudgetDate = DateTime.ParseExact(BudgetDetails.BudgetDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Remarks = BudgetDetails.BudgetRemarks,
                    IsCompleted = false
                };

                // Update existing budget or add new one
                if (BudgetDetails.ButtonText == "Save")
                {
                    budget.BudgetId = this.BudgetDetails.BudgetId;
                    this.BudgetDetails.Clear();
                    return _dataStore.UpdateBudget(budget, BudgetDetails.BudgetId).Result;
                }
                this.BudgetDetails.Clear();
                return _dataStore.UpdateBudget(budget).Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds or updates savings based on form data
        /// </summary>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> OnAddBSavingsClicked()
        {
            try
            {
                // Create savings object from form data
                Savings saving = new Savings()
                {
                    SavingsDescription = this.SavingDetails.SavingDescription,
                    SavingsAmount = this.SavingDetails.SavingAmount,
                    SavingsType = this.SavingDetails.SavingType,
                    SavingsDate = DateTime.ParseExact(SavingDetails.SavingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    SavingsRemark = SavingDetails.SavingRemarks
                };

                // Update existing savings or add new one
                if (SavingDetails.ButtonText == "Save")
                {
                    saving.TransactionId = SavingDetails.TransactionId;
                    this.SavingDetails.Clear();
                    return _dataStore.UpdateSavings(saving, SavingDetails.TransactionId).Result;
                }
                this.SavingDetails.Clear();
                return _dataStore.UpdateSavings(saving).Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates or updates a goal based on form data
        /// </summary>
        /// <returns>True if operation is successful, false otherwise</returns>
        public bool OnCreateGoalClicked()
        {
            try
            {
                // Create goal object from form data
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

                // Update existing goal or add new one
                if (GoalDetails.ButtonText == "Save")
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
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds or updates a goal fund transaction based on form data
        /// </summary>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> OnAddFundClicked()
        {
            try
            {
                // Prepare the fund transaction from FundDetails
                GoalsTransaction fundTransaction = new GoalsTransaction
                {
                    TransactionDate = DateTime.ParseExact(FundDetails.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    TransactionDescription = FundDetails.TransactionDescription,
                    ContributionAmount = Double.Parse(FundDetails.ContributionAmount),
                    Remarks = FundDetails.Remarks
                };

                bool result;
                if (FundDetails.ButtonText == "Save")
                {
                    // Update existing transaction for the goal
                    fundTransaction.TransactionId = Double.Parse(FundDetails.TransactionId);
                    result = _dataStore.UpdateGoalTransaction(
                        fundTransaction,
                        Double.Parse(FundDetails.GoalId),
                        Double.Parse(FundDetails.TransactionId)
                    ).Result;
                }
                else
                {
                    // Add new transaction for the goal
                    result = _dataStore.UpdateGoalTransaction(
                        fundTransaction,
                        Double.Parse(FundDetails.GoalId),
                        null
                    ).Result;
                }

                this.FundDetails.Clear();
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Edit Methods

        /// <summary>
        /// Triggers edit mode for a savings transaction
        /// </summary>
        /// <param name="transactionId">ID of the savings transaction to edit</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> TriggerEditSavings(double transactionId)
        {
            try
            {
                // Load savings data for editing
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

        /// <summary>
        /// Triggers edit mode for a transaction
        /// </summary>
        /// <param name="transactionId">ID of the transaction to edit</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> TriggerEditTransaction(double transactionId)
        {
            try
            {
                // Load transaction data for editing
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

        /// <summary>
        /// Triggers edit mode for a budget
        /// </summary>
        /// <param name="budgetId">ID of the budget to edit</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> TriggerEditBudget(double budgetId)
        {
            try
            {
                // Load budget data for editing
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

        /// <summary>
        /// Triggers edit mode for a goal
        /// </summary>
        /// <param name="goalId">ID of the goal to edit</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> TriggerEditGoal(double goalId)
        {
            try
            {
                // Load goal data for editing
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

        #endregion
    }

    /// <summary>
    /// Represents a navigation item in the dashboard menu
    /// </summary>
    public class DashboardNavigationItems
    {
        #region Properties

        /// <summary>
        /// Gets or sets the navigation item name
        /// </summary>
        /// <value>String representing the item name</value>
        public string? ItemName { get; set; }

        /// <summary>
        /// Gets or sets the navigation item icon
        /// </summary>
        /// <value>String representing the icon character or path</value>
        public string? ItemIcon { get; set; }

        /// <summary>
        /// Gets or sets the command for navigation
        /// </summary>
        /// <value>ICommand for handling navigation</value>
        public ICommand? Command { get; set; }

        #endregion
    }
   /// <summary>
    /// Form model for transaction details with validation and category management
    /// </summary>
    public class TransactionDetails : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Transaction type (Income/Expense)
        /// </summary>
        private string transactionType;

        /// <summary>
        /// Transaction category
        /// </summary>
        private string transactionCategory;

        /// <summary>
        /// Available transaction category options
        /// </summary>
        private List<string> transactionCategoryOptions;

        /// <summary>
        /// Transaction subcategory
        /// </summary>
        private string transactionSubCategory;

        /// <summary>
        /// Available transaction subcategory options
        /// </summary>
        private List<string> transactionSubCategoryOptions;

        /// <summary>
        /// Transaction amount
        /// </summary>
        private string transactionAmount;

        /// <summary>
        /// Transaction date
        /// </summary>
        private string transactionDate;

        /// <summary>
        /// Transaction remarks
        /// </summary>
        private string transactionRemarks;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the available transaction type options
        /// </summary>
        /// <value>List of strings representing transaction types</value>
        public List<string> TransactionTypeOptions { get; set; }

        /// <summary>
        /// Gets or sets the available transaction category options
        /// </summary>
        /// <value>List of strings representing transaction categories</value>
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

        /// <summary>
        /// Gets or sets the available transaction subcategory options
        /// </summary>
        /// <value>List of strings representing transaction subcategories</value>
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

        /// <summary>
        /// Gets or sets the transaction type
        /// </summary>
        /// <value>String representing the transaction type</value>
        public string TransactionType
        {
            get => transactionType;
            set
            {
                if (transactionType != value)
                {
                    transactionType = value;
                    OnPropertyChanged(nameof(TransactionType));
                    // Update category options based on type
                    TransactionCategoryOptions = GetCategoryByType(transactionType);
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the transaction category
        /// </summary>
        /// <value>String representing the transaction category</value>
        public string TransactionCategory
        {
            get => transactionCategory;
            set
            {
                if (transactionCategory != value)
                {
                    transactionCategory = value;
                    OnPropertyChanged(nameof(TransactionCategory));
                    // Update subcategory options based on category
                    TransactionSubCategoryOptions = GetSubCategoriesByCategory(transactionCategory, TransactionType);
                }
            }
        }
       
        /// <summary>
        /// Gets or sets the transaction subcategory
        /// </summary>
        /// <value>String representing the transaction subcategory</value>
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
        
        /// <summary>
        /// Gets or sets the transaction amount
        /// </summary>
        /// <value>String representing the transaction amount</value>
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
        
        /// <summary>
        /// Gets or sets the transaction date
        /// </summary>
        /// <value>String representing the transaction date in dd/MM/yyyy format</value>
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
        
        /// <summary>
        /// Gets or sets the transaction remarks
        /// </summary>
        /// <value>String representing additional notes about the transaction</value>
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
      
        /// <summary>
        /// Gets or sets the transaction ID for editing
        /// </summary>
        /// <value>Double representing the transaction ID</value>
        public double TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the button text for form submission
        /// </summary>
        /// <value>String representing the button text (Add/Save)</value>
        public string ButtonText { get; set; }

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
        /// Initializes a new instance of the TransactionDetails class
        /// </summary>
        public TransactionDetails()
        {
            TransactionTypeOptions = new List<string>() { "Income", "Expense" };
            ButtonText = "Add";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets category options based on transaction type
        /// </summary>
        /// <param name="transactionType">Transaction type (Income/Expense)</param>
        /// <returns>List of category strings</returns>
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

        /// <summary>
        /// Gets subcategory options based on category and transaction type
        /// </summary>
        /// <param name="category">Selected category</param>
        /// <param name="transactionType">Transaction type</param>
        /// <returns>List of subcategory strings</returns>
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all form fields and resets to default state
        /// </summary>
        public void Clear()
        {
            TransactionType = TransactionCategory = TransactionSubCategory = TransactionAmount = TransactionDate = TransactionRemarks = string.Empty;
            TransactionCategoryOptions.Clear();
            TransactionSubCategoryOptions.Clear();
        }

        #endregion
    }

    /// <summary>
    /// Form model for budget details with validation and category management
    /// </summary>
    public class BudgetDetails : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Budget title
        /// </summary>
        private string budgetTitle;

        /// <summary>
        /// Budget category
        /// </summary>
        private string? budgetCategory;

        /// <summary>
        /// Budget amount
        /// </summary>
        private string budgetAmount;

        /// <summary>
        /// Budget date
        /// </summary>
        private string budgetDate;

        /// <summary>
        /// Budget remarks
        /// </summary>
        private string budgetRemarks;

        /// <summary>
        /// Available budget category options
        /// </summary>
        private List<string> budgetCategoryOptions;

        /// <summary>
        /// Button text for form submission
        /// </summary>
        private string buttonText;

        /// <summary>
        /// Budget ID for editing
        /// </summary>
        private double budgdetId;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the budget title
        /// </summary>
        /// <value>String representing the budget title</value>
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

        /// <summary>
        /// Gets or sets the budget category
        /// </summary>
        /// <value>String representing the budget category</value>
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

        /// <summary>
        /// Gets or sets the budget amount
        /// </summary>
        /// <value>String representing the budget amount</value>
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

        /// <summary>
        /// Gets or sets the budget date
        /// </summary>
        /// <value>String representing the budget date in dd/MM/yyyy format</value>
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

        /// <summary>
        /// Gets or sets the budget remarks
        /// </summary>
        /// <value>String representing additional notes about the budget</value>
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

        /// <summary>
        /// Gets or sets the available budget category options
        /// </summary>
        /// <value>List of strings representing budget categories</value>
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

        /// <summary>
        /// Gets or sets the button text for form submission
        /// </summary>
        /// <value>String representing the button text (Create/Save)</value>
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

        /// <summary>
        /// Gets or sets the budget ID for editing
        /// </summary>
        /// <value>Double representing the budget ID</value>
        public double BudgetId
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
        /// Initializes a new instance of the BudgetDetails class
        /// </summary>
        public BudgetDetails()
        {
            BudgetCategoryOptions = DataStore.BudgetCategory;
            ButtonText = "Create";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all form fields and resets to default state
        /// </summary>
        public void Clear()
        {
            BudgetAmount = BudgetDate = BudgetTitle = BudgetRemarks = BudgetCategory = string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// Form model for goal details with validation and priority management
    /// </summary>
    public class GoalDetails : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Goal ID for editing
        /// </summary>
        private double goalId;

        /// <summary>
        /// Goal title
        /// </summary>
        private string goalTitle;

        /// <summary>
        /// Goal priority
        /// </summary>
        private string? goalPriority;

        /// <summary>
        /// Goal amount
        /// </summary>
        private string goalAmount;

        /// <summary>
        /// Goal deadline date
        /// </summary>
        private string deadlineDate;

        /// <summary>
        /// Goal remarks
        /// </summary>
        private string goalRemarks;

        /// <summary>
        /// Button text for form submission
        /// </summary>
        private string buttonText;

        /// <summary>
        /// Available goal priority options
        /// </summary>
        private List<string> goalPriorityOptions;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the goal title
        /// </summary>
        /// <value>String representing the goal title</value>
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

        /// <summary>
        /// Gets or sets the goal priority
        /// </summary>
        /// <value>String representing the goal priority level</value>
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

        /// <summary>
        /// Gets or sets the goal amount
        /// </summary>
        /// <value>String representing the target goal amount</value>
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

        /// <summary>
        /// Gets or sets the goal deadline date
        /// </summary>
        /// <value>String representing the deadline date in dd/MM/yyyy format</value>
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

        /// <summary>
        /// Gets or sets the goal remarks
        /// </summary>
        /// <value>String representing additional notes about the goal</value>
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

        /// <summary>
        /// Gets or sets the available goal priority options
        /// </summary>
        /// <value>List of strings representing priority levels</value>
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

        /// <summary>
        /// Gets or sets the goal ID for editing
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
        /// Gets or sets the button text for form submission
        /// </summary>
        /// <value>String representing the button text (Create/Save)</value>
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
        /// Initializes a new instance of the GoalDetails class
        /// </summary>
        public GoalDetails()
        {
            GoalPriorityOptions = Enum.GetNames(typeof(GoalPriority)).ToList();
            ButtonText = "Create";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all form fields and resets to default state
        /// </summary>
        public void Clear()
        {
            GoalAmount = GoalTitle = DeadlineDate = GoalRemarks = GoalPriority = string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// Form model for savings details with validation and type management
    /// </summary>
    public class SavingsDetails : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Savings description
        /// </summary>
        private string savingDescription;

        /// <summary>
        /// Savings type (Deposit/Withdrawal)
        /// </summary>
        private string? savingType;

        /// <summary>
        /// Savings amount
        /// </summary>
        private string savingAmount;

        /// <summary>
        /// Savings date
        /// </summary>
        private string savingDate;

        /// <summary>
        /// Savings remarks
        /// </summary>
        private string savingRemarks;

        /// <summary>
        /// Available savings type options
        /// </summary>
        private List<string> savingTypeOptions;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the savings description
        /// </summary>
        /// <value>String representing the savings description</value>
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

        /// <summary>
        /// Gets or sets the savings type
        /// </summary>
        /// <value>String representing the savings type (Deposit/Withdrawal)</value>
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

        /// <summary>
        /// Gets or sets the savings amount
        /// </summary>
        /// <value>String representing the savings amount</value>
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

        /// <summary>
        /// Gets or sets the savings date
        /// </summary>
        /// <value>String representing the savings date in dd/MM/yyyy format</value>
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

        /// <summary>
        /// Gets or sets the savings remarks
        /// </summary>
        /// <value>String representing additional notes about the savings</value>
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

        /// <summary>
        /// Gets or sets the available savings type options
        /// </summary>
        /// <value>List of strings representing savings types</value>
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

        /// <summary>
        /// Gets or sets the transaction ID for editing
        /// </summary>
        /// <value>Double representing the transaction ID</value>
        public double TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the button text for form submission
        /// </summary>
        /// <value>String representing the button text (Add/Save)</value>
        public string ButtonText { get; set; }

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
        /// Initializes a new instance of the SavingsDetails class
        /// </summary>
        public SavingsDetails()
        {
            ButtonText = "Add";
            SavingTypeOptions = new List<string>()
            {
                "Deposit",
                "Withdrawal"
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all form fields and resets to default state
        /// </summary>
        public void Clear()
        {
            ButtonText = "Add";
            SavingAmount = SavingDescription = SavingDate = SavingRemarks = SavingType = string.Empty;
        }

        #endregion
    }

       /// <summary>
    /// Form model for goal transaction details with validation
    /// </summary>
    public class GoalTransactionDetails : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Goal ID
        /// </summary>
        private string goalId;

        /// <summary>
        /// Transaction ID for editing
        /// </summary>
        private string transactionId;

        /// <summary>
        /// Transaction date
        /// </summary>
        private string transactionDate;

        /// <summary>
        /// Transaction description
        /// </summary>
        private string transactionDescription;

        /// <summary>
        /// Contribution amount
        /// </summary>
        private string contributionAmount;

        /// <summary>
        /// Transaction remarks
        /// </summary>
        private string remarks;

        /// <summary>
        /// Button text for form submission
        /// </summary>
        private string buttonText;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the goal ID
        /// </summary>
        /// <value>String representing the goal ID</value>
        public string GoalId
        {
            get => goalId;
            set
            {
                if (goalId != value)
                {
                    goalId = value;
                    OnPropertyChanged(nameof(GoalId));
                }
            }
        }

        /// <summary>
        /// Gets or sets the transaction ID for editing
        /// </summary>
        /// <value>String representing the transaction ID</value>
        public string TransactionId
        {
            get => transactionId;
            set
            {
                if (transactionId != value)
                {
                    transactionId = value;
                    OnPropertyChanged(nameof(TransactionId));
                }
            }
        }

        /// <summary>
        /// Gets or sets the transaction date
        /// </summary>
        /// <value>String representing the transaction date in dd/MM/yyyy format</value>
        public string TransactionDate
        {
            get => transactionDate;
            set
            {
                if (transactionDate != value)
                {
                    transactionDate = value != string.Empty ? DateTime.Parse(value).ToString("dd/MM/yyyy") : value;
                    OnPropertyChanged(nameof(TransactionDate));
                }
            }
        }

        /// <summary>
        /// Gets or sets the transaction description
        /// </summary>
        /// <value>String representing the transaction description</value>
        public string TransactionDescription
        {
            get => transactionDescription;
            set
            {
                if (transactionDescription != value)
                {
                    transactionDescription = value;
                    OnPropertyChanged(nameof(TransactionDescription));
                }
            }
        }

        /// <summary>
        /// Gets or sets the contribution amount
        /// </summary>
        /// <value>String representing the contribution amount</value>
        public string ContributionAmount
        {
            get => contributionAmount;
            set
            {
                if (contributionAmount != value)
                {
                    contributionAmount = value;
                    OnPropertyChanged(nameof(ContributionAmount));
                }
            }
        }

        /// <summary>
        /// Gets or sets the transaction remarks
        /// </summary>
        /// <value>String representing additional notes about the transaction</value>
        public string Remarks
        {
            get => remarks;
            set
            {
                if (remarks != value)
                {
                    remarks = value;
                    OnPropertyChanged(nameof(Remarks));
                }
            }
        }

        /// <summary>
        /// Gets or sets the button text for form submission
        /// </summary>
        /// <value>String representing the button text (Add/Save)</value>
        public string ButtonText
        {
            get => buttonText;
            set
            {
                if (buttonText != value)
                {
                    buttonText = value;
                    OnPropertyChanged(nameof(ButtonText));
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the GoalTransactionDetails class
        /// </summary>
        public GoalTransactionDetails()
        {
            ButtonText = "Add";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all form fields and resets to default state
        /// </summary>
        public void Clear()
        {
            TransactionId = TransactionDescription = Remarks = ContributionAmount = TransactionDate = string.Empty;
            ButtonText = "Add";
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
    }
}