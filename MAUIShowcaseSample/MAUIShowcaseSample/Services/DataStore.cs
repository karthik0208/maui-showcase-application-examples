using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MAUIShowcaseSample.Services
{
    public class DataStore
    {

        private double previousBalance = 20000;

        private ObservableCollection<Transaction> dailyTransaction = new()
            {
              new Transaction { TransactionType = "Expense", TransactionName = "Groceries", TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Weekly groceries from Walmart", TransactionAmount = "50", TransactionDate = DateTime.Today },
new Transaction { TransactionType = "Expense", TransactionName = "Fuel", TransactionCategory = ExpenseCategory.Transportation.ToString(), TransactionDescription = "Gas station refill", TransactionAmount = "25", TransactionDate = DateTime.Today.AddDays(-2) },
new Transaction { TransactionType = "Income", TransactionName = "Blog Writing", TransactionCategory = IncomeCategory.Freelance.ToString(), TransactionDescription = "Payment for blog writing", TransactionAmount = "500", TransactionDate = DateTime.Today.AddDays(-4) },
new Transaction { TransactionType = "Expense", TransactionName = "Apartment Rent", TransactionCategory = ExpenseCategory.Mortgage.ToString(), TransactionDescription = "Monthly rent payment", TransactionAmount = "750", TransactionDate = DateTime.Today.AddDays(-5) },
new Transaction { TransactionType = "Expense", TransactionName = "Movie Ticket", TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Weekend movie outing", TransactionAmount = "15", TransactionDate = DateTime.Today.AddDays(-6) },
new Transaction { TransactionType = "Income", TransactionName = "Monthly Salary", TransactionCategory = IncomeCategory.Salary.ToString(), TransactionDescription = "January Salary", TransactionAmount = "2000", TransactionDate = DateTime.Today.AddDays(-7) },
new Transaction { TransactionType = "Income", TransactionName = "Stock Dividends", TransactionCategory = IncomeCategory.Dividends.ToString(), TransactionDescription = "Dividend payout from stocks", TransactionAmount = "150", TransactionDate = DateTime.Today.AddDays(-8) },
new Transaction { TransactionType = "Income", TransactionName = "Savings Interest", TransactionCategory = IncomeCategory.Interest.ToString(), TransactionDescription = "Interest from savings account", TransactionAmount = "100", TransactionDate = DateTime.Today.AddDays(-9) },
new Transaction { TransactionType = "Expense", TransactionName = "Clothing Purchase", TransactionCategory = ExpenseCategory.Shopping.ToString(), TransactionDescription = "New jacket and shoes", TransactionAmount = "300", TransactionDate = DateTime.Today.AddDays(-10) },
new Transaction { TransactionType = "Expense", TransactionName = "Doctor Visit", TransactionCategory = ExpenseCategory.HealthCare.ToString(), TransactionDescription = "Consultation fee", TransactionAmount = "250", TransactionDate = DateTime.Today.AddDays(-11) },
new Transaction { TransactionType = "Expense", TransactionName = "Restaurant Dinner", TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Dinner at a restaurant", TransactionAmount = "40", TransactionDate = DateTime.Today.AddDays(-12) },
new Transaction { TransactionType = "Expense", TransactionName = "Electricity Bill", TransactionCategory = ExpenseCategory.Utilities.ToString(), TransactionDescription = "Monthly electricity charges", TransactionAmount = "180", TransactionDate = DateTime.Today.AddDays(-13) },
new Transaction { TransactionType = "Expense", TransactionName = "Broadband Bill", TransactionCategory = ExpenseCategory.Bills.ToString(), TransactionDescription = "Monthly internet bill", TransactionAmount = "60", TransactionDate = DateTime.Today.AddDays(-14) },
new Transaction { TransactionType = "Expense", TransactionName = "Home Loan EMI", TransactionCategory = ExpenseCategory.Mortgage.ToString(), TransactionDescription = "Home loan installment", TransactionAmount = "600", TransactionDate = DateTime.Today.AddDays(-15) },
new Transaction { TransactionType = "Income", TransactionName = "Annual Bonus", TransactionCategory = IncomeCategory.ExtraIncome.ToString(), TransactionDescription = "Yearly performance bonus", TransactionAmount = "300", TransactionDate = DateTime.Today.AddDays(-16) },
new Transaction { TransactionType = "Expense", TransactionName = "Netflix Subscription", TransactionCategory = ExpenseCategory.Bills.ToString(), TransactionDescription = "Monthly Netflix charge", TransactionAmount = "20", TransactionDate = DateTime.Today.AddDays(-17) },
new Transaction { TransactionType = "Expense", TransactionName = "Online Course", TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Udemy course fee", TransactionAmount = "100", TransactionDate = DateTime.Today.AddDays(-18) },
new Transaction { TransactionType = "Expense", TransactionName = "Cafeteria Lunch", TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Lunch at office cafeteria", TransactionAmount = "15", TransactionDate = DateTime.Today.AddDays(-19) },
new Transaction { TransactionType = "Expense", TransactionName = "Bus Ticket", TransactionCategory = ExpenseCategory.Transportation.ToString(), TransactionDescription = "Daily commute expense", TransactionAmount = "10", TransactionDate = DateTime.Today.AddDays(-20) },
new Transaction { TransactionType = "Income", TransactionName = "Side Project Payment", TransactionCategory = IncomeCategory.Freelance.ToString(), TransactionDescription = "Payment from a freelance project", TransactionAmount = "600", TransactionDate = DateTime.Today.AddDays(-21) },
new Transaction { TransactionType = "Expense", TransactionName = "Concert Ticket", TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Concert event ticket", TransactionAmount = "120", TransactionDate = DateTime.Today.AddDays(-22) },
new Transaction { TransactionType = "Expense", TransactionName = "Electronics Purchase", TransactionCategory = ExpenseCategory.Shopping.ToString(), TransactionDescription = "New phone purchase", TransactionAmount = "450", TransactionDate = DateTime.Today.AddDays(-23) },
new Transaction { TransactionType = "Expense", TransactionName = "Health Insurance", TransactionCategory = ExpenseCategory.Insurance.ToString(), TransactionDescription = "Monthly insurance premium", TransactionAmount = "220", TransactionDate = DateTime.Today.AddDays(-26) },
new Transaction { TransactionType = "Income", TransactionName = "Mutual Fund Dividend", TransactionCategory = IncomeCategory.Dividends.ToString(), TransactionDescription = "Dividend payout from mutual funds", TransactionAmount = "200", TransactionDate = DateTime.Today.AddDays(-27) },
new Transaction { TransactionType = "Expense", TransactionName = "Water Bill", TransactionCategory = ExpenseCategory.Utilities.ToString(), TransactionDescription = "Monthly water bill", TransactionAmount = "55", TransactionDate = DateTime.Today.AddDays(-28) },
new Transaction { TransactionType = "Income", TransactionName = "Fixed Deposit Interest", TransactionCategory = IncomeCategory.Interest.ToString(), TransactionDescription = "Interest earned on FD", TransactionAmount = "120", TransactionDate = DateTime.Today.AddDays(-29) },
new Transaction { TransactionType = "Expense", TransactionName = "Snacks Purchase", TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Snacks from a local store", TransactionAmount = "20", TransactionDate = DateTime.Today.AddDays(-3) },
new Transaction { TransactionType = "Expense", TransactionName = "Mobile Recharge", TransactionCategory = ExpenseCategory.Bills.ToString(), TransactionDescription = "Monthly mobile data plan", TransactionAmount = "35", TransactionDate = DateTime.Today.AddDays(-5) },
new Transaction { TransactionType = "Expense", TransactionName = "Game Subscription", TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Subscription for online gaming service", TransactionAmount = "25", TransactionDate = DateTime.Today.AddDays(-15) },
new Transaction { TransactionType = "Expense", TransactionName = "Pharmacy Expenses", TransactionCategory = ExpenseCategory.HealthCare.ToString(), TransactionDescription = "Medicines and supplements", TransactionAmount = "60", TransactionDate = DateTime.Today.AddDays(-20) }

            };
        static DateTime today = DateTime.Today;
        ObservableCollection<Savings> savingsList = new ObservableCollection<Savings>
        {
            new Savings { SavingsDate = today, SavingsDescription = "General Savings", SavingsType = "Deposit", SavingsAmount = "500" },
            new Savings { SavingsDate = today.AddDays(-1), SavingsDescription = "Savings Usage", SavingsType = "Withdrawal", SavingsAmount = "100" },
            new Savings { SavingsDate = today.AddDays(-2), SavingsDescription = "Flexible Fund", SavingsType = "Deposit", SavingsAmount = "200" },
            new Savings { SavingsDate = today.AddDays(-3), SavingsDescription = "Emergency Access", SavingsType = "Withdrawal", SavingsAmount = "50" },
            new Savings { SavingsDate = today.AddDays(-4), SavingsDescription = "Miscellaneous Savings", SavingsType = "Deposit", SavingsAmount = "300" },
            new Savings { SavingsDate = today.AddDays(-5), SavingsDescription = "General Fund Withdrawal", SavingsType = "Withdrawal", SavingsAmount = "75" },
            new Savings { SavingsDate = today.AddDays(-6), SavingsDescription = "Holiday Fund", SavingsType = "Deposit", SavingsAmount = "600" },
            new Savings { SavingsDate = today.AddDays(-7), SavingsDescription = "Medical Expenses", SavingsType = "Withdrawal", SavingsAmount = "120" },
            new Savings { SavingsDate = today.AddDays(-8), SavingsDescription = "Investment Savings", SavingsType = "Deposit", SavingsAmount = "800" },
            new Savings { SavingsDate = today.AddDays(-9), SavingsDescription = "Home Renovation", SavingsType = "Withdrawal", SavingsAmount = "400" },
            new Savings { SavingsDate = today.AddDays(-10), SavingsDescription = "Car Repair Fund", SavingsType = "Deposit", SavingsAmount = "500" },
            new Savings { SavingsDate = today.AddDays(-1), SavingsDescription = "Car Maintenance", SavingsType = "Withdrawal", SavingsAmount = "95" },
            new Savings { SavingsDate = today.AddDays(-2), SavingsDescription = "Bonus Savings", SavingsType = "Deposit", SavingsAmount = "900" },
            new Savings { SavingsDate = today.AddDays(-3), SavingsDescription = "Charity Donation", SavingsType = "Withdrawal", SavingsAmount = "150" },
            new Savings { SavingsDate = today.AddDays(-4), SavingsDescription = "College Fund", SavingsType = "Deposit", SavingsAmount = "700" },
            new Savings { SavingsDate = today.AddDays(-5), SavingsDescription = "Subscription Payment", SavingsType = "Withdrawal", SavingsAmount = "30" },
            new Savings { SavingsDate = today.AddDays(-6), SavingsDescription = "Grocery Savings", SavingsType = "Deposit", SavingsAmount = "250" },
            new Savings { SavingsDate = today.AddDays(-7), SavingsDescription = "Electricity Bill", SavingsType = "Withdrawal", SavingsAmount = "85" },
            new Savings { SavingsDate = today.AddDays(-8), SavingsDescription = "Travel Fund", SavingsType = "Deposit", SavingsAmount = "450" },
            new Savings { SavingsDate = today.AddDays(-9), SavingsDescription = "Dining Out", SavingsType = "Withdrawal", SavingsAmount = "60" }
        };

        private ObservableCollection<Budget> BudgetList = new ObservableCollection<Budget>()
        {
            new Budget { BudgetTitle = "Budget for March 2025", BudgetAmount = 35000, BudgetCategory = BudgetCategory.MonthlyBudget, BudgetDate = new DateTime (2025, 03, 01), IsCompleted = false, Remarks = "Monthly budget for March 2025" },
            new Budget { BudgetTitle = "Family Trip to Hawaii", BudgetAmount = 8000, BudgetCategory = BudgetCategory.VacationBudget, BudgetDate = new DateTime(2025, 02, 28), IsCompleted = false, Remarks = "Vacation budget for the year 2025"},
            new Budget {BudgetTitle = "Transport expenses", BudgetAmount = 15000, BudgetCategory = BudgetCategory.TransportBudget, BudgetDate = new DateTime(2025, 03, 02), IsCompleted = false, Remarks = "Transportation budget for the year 2025"},
            new Budget { BudgetTitle = "Budget for February 2025", BudgetAmount = 33000, BudgetCategory = BudgetCategory.MonthlyBudget, BudgetDate = new DateTime (2025, 02, 01), IsCompleted = true, Remarks = "Monthly budget for February 2025" },
        };

        private ObservableCollection<Goal> GoalsList = new ObservableCollection<Goal>()
        {
            new Goal { GoalTitle = "Vacation Fund", GoalAmount = 35000,SpentAmount=3000, GoalPriority = GoalPriority.Medium, GoalDate = new DateTime (2025, 03, 01), IsCompleted = false, Remarks = "Family trip to Hawai" },
            new Goal { GoalTitle = "New Car", GoalAmount = 8000, SpentAmount = 2000, GoalPriority = GoalPriority.Low, GoalDate = new DateTime(2025, 02, 28), IsCompleted = false, Remarks = "Buy a new car"},
            new Goal {GoalTitle = "Home Renovation", GoalAmount = 15000,SpentAmount = 8000, GoalPriority = GoalPriority.High, GoalDate = new DateTime(2025, 03, 02), IsCompleted = false, Remarks = "Kitchen Renovation"},
            new Goal { GoalTitle = "Wedding Fund", GoalAmount = 33000, SpentAmount = 6000, GoalPriority = GoalPriority.High, GoalDate = new DateTime (2025, 02, 01), IsCompleted = true, Remarks = "Best Friend's marriage gift" },
        };

        public ObservableCollection<Transaction> GetDailyTransactions(DateTime? startDate = null, DateTime? endDate = null)
        {
            if(startDate != null && endDate != null)
            {
                var filteredTransactions = this.dailyTransaction.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToObservableCollection();
                return filteredTransactions;
            }
            return this.dailyTransaction;
        }

        public ObservableCollection<Savings> GetSavingsData(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate != null && endDate != null)
            {
                var filteredSavings = this.savingsList.Where(t => t.SavingsDate >= startDate && t.SavingsDate <= endDate).ToObservableCollection();
                return filteredSavings;
            }
            return this.savingsList;
        }

        public ObservableCollection<Budget> GetBudgetList(string? budgetStatus)
        {
            if(budgetStatus != null)
            {
                var selectedBudgetList = this.BudgetList.Where(t => t.IsCompleted == (budgetStatus == "Active Budget" ? false : true)).ToObservableCollection<Budget>();
                return selectedBudgetList;
            }
            return this.BudgetList;
        }

        public ObservableCollection<Goal> GetGoalsData(string? goalStatus = null)
        {
            if(goalStatus != null)
            {
                var selectedGoalData = this.GoalsList.Where(t => t.IsCompleted == (goalStatus == "Active" ? false : true)).ToObservableCollection<Goal>();
                return selectedGoalData;
            }
            return this.GoalsList;
        }

        public double GetPreviousBalance()
        {
            return previousBalance;
        }
        
    }
    public class Transaction
    {
        private string? transactionType;

        private string? transactionName;

        private string? transactionCategory;

        private string? transactionDescription;

        private string? transactionAmount;

        private DateTime transactionDate;

        public string? TransactionType
        {
            get
            {
                return this.transactionType;
            }
            set
            {
                this.transactionType = value;
            }
        }

        public string? TransactionName
        {
            get
            {
                return this.transactionName;
            }
            set
            {
                this.transactionName = value;
            }
        }

        public string? TransactionCategory
        {
            get
            {
                return this.transactionCategory;
            }
            set
            {
                this.transactionCategory = value;
            }
        }

        public string? TransactionDescription
        {
            get
            {
                return this.transactionDescription;
            }
            set
            {
                this.transactionDescription = value;
            }
        }
        public string TransactionAmount
        {
            get
            {
                return this.transactionAmount;
            }
            set
            {
                this.transactionAmount = value;
            }
        }
        public DateTime TransactionDate
        {
            get
            {
                return this.transactionDate;
            }
            set
            {
                this.transactionDate = value;
            }
        }
    }

    public enum ExpenseCategory
    {
        Mortgage,
        Food,
        Utilities,
        Bills,
        Shopping,
        Transportation,
        Insurance,
        HealthCare,
        Clothing,
        Others
    }

    public enum IncomeCategory
    {
        Salary,
        Freelance,
        Interest,
        Dividends,
        ExtraIncome
    }

    public class Budget
    {
        private string? budgetTitle;

        private BudgetCategory budgetCategory;

        private double budgetAmount;

        private DateTime budgetDate;

        private string? budgetRemarks;

        private bool isCompleted;

        public string? BudgetTitle
        {
            get
            {
                return this.budgetTitle;
            }
            set
            {
                this.budgetTitle = value;
            }
        }

        public BudgetCategory BudgetCategory
        {
            get
            {
                return this.budgetCategory;
            }
            set
            {
                this.budgetCategory = value;
            }
        }

        public double BudgetAmount
        {
            get
            {
                return this.budgetAmount;
            }
            set
            {
                this.budgetAmount = value;
            }
        }

        public DateTime BudgetDate
        {
            get
            {
                return this.budgetDate;
            }
            set
            {
                this.budgetDate = value;
            }
        }

        public string? Remarks
        {
            get
            {
                return this.budgetRemarks;
            }
            set
            {
                this.budgetRemarks = value;
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
    }

    public enum BudgetCategory
    {
        [Description("Monthly Budget")]
        MonthlyBudget,

        [Description("Vacation Budget")]
        VacationBudget,

        [Description("Transport Budget")]
        TransportBudget,
    }

    public class Savings
    {
        private string? savingsType;

        private string? savingsDescription;

        private string? savingsAmount;

        private DateTime savingsDate;

        private string? savingsRemark;

        public string? SavingsType
        {
            get
            {
                return this.savingsType;
            }
            set
            {
                this.savingsType = value;
            }
        }

        public string? SavingsDescription
        {
            get
            {
                return this.savingsDescription;
            }
            set
            {
                this.savingsDescription = value;
            }
        }
        public string SavingsAmount
        {
            get
            {
                return this.savingsAmount;
            }
            set
            {
                this.savingsAmount = value;
            }
        }
        public DateTime SavingsDate
        {
            get
            {
                return this.savingsDate;
            }
            set
            {
                this.savingsDate = value;
            }
        }

        public string SavingsRemark
        {
            get
            {
                return this.savingsRemark;
            }
            set
            {
                this.savingsRemark = value;
            }
        }
    }

    public class Goal
    {
        private string? goalTitle;

        private GoalPriority goalPriority;

        private double goalAmount;

        private double spentAmount;

        private DateTime goalDate;

        private string? goalRemarks;

        private bool isCompleted;

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

        public double GoalAmount
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
    }

    public enum GoalPriority
    {
        [Description("Low")]
        Low,

        [Description("Medium")]
        Medium,

        [Description("High")]
        High,
    }
}
