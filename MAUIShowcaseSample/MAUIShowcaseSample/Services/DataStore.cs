using Syncfusion.Maui.DataSource.Extensions;
using Syncfusion.XPS;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample.Services
{
    public class DataStore
    {

        private double previousBalance = 20000;

        private ObservableCollection<Transaction> dailyTransaction = new ObservableCollection<Transaction>
        {
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.Groceries.ToString(), TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Weekly groceries from Walmart", TransactionAmount = "50", TransactionDate = DateTime.Today },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.Fuel.ToString(), TransactionCategory = ExpenseCategory.Transportation.ToString(), TransactionDescription = "Gas station refill", TransactionAmount = "25", TransactionDate = DateTime.Today.AddDays(-2) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.BlogWriting.ToString(), TransactionCategory = IncomeCategory.Freelance.ToString(), TransactionDescription = "Payment for blog writing", TransactionAmount = "500", TransactionDate = DateTime.Today.AddDays(-4) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.ApartmentRent.ToString(), TransactionCategory = ExpenseCategory.Mortgage.ToString(), TransactionDescription = "Monthly rent payment", TransactionAmount = "750", TransactionDate = DateTime.Today.AddDays(-5) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.MovieTicket.ToString(), TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Weekend movie outing", TransactionAmount = "15", TransactionDate = DateTime.Today.AddDays(-6) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.MonthlySalary.ToString(), TransactionCategory = IncomeCategory.Salary.ToString(), TransactionDescription = "January Salary", TransactionAmount = "2000", TransactionDate = DateTime.Today.AddDays(-7) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.StockDividends.ToString(), TransactionCategory = IncomeCategory.Dividends.ToString(), TransactionDescription = "Dividend payout from stocks", TransactionAmount = "150", TransactionDate = DateTime.Today.AddDays(-8) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.SavingsInterest.ToString(), TransactionCategory = IncomeCategory.Interest.ToString(), TransactionDescription = "Interest from savings account", TransactionAmount = "100", TransactionDate = DateTime.Today.AddDays(-9) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.ClothingPurchase.ToString(), TransactionCategory = ExpenseCategory.Shopping.ToString(), TransactionDescription = "New jacket and shoes", TransactionAmount = "300", TransactionDate = DateTime.Today.AddDays(-10) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.DoctorVisit.ToString(), TransactionCategory = ExpenseCategory.HealthCare.ToString(), TransactionDescription = "Consultation fee", TransactionAmount = "250", TransactionDate = DateTime.Today.AddDays(-11) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.RestaurantDinner.ToString(), TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Dinner at a restaurant", TransactionAmount = "40", TransactionDate = DateTime.Today.AddDays(-12) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.ElectricityBill.ToString(), TransactionCategory = ExpenseCategory.Utilities.ToString(), TransactionDescription = "Monthly electricity charges", TransactionAmount = "180", TransactionDate = DateTime.Today.AddDays(-13) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.BroadbandBill.ToString(), TransactionCategory = ExpenseCategory.Bills.ToString(), TransactionDescription = "Monthly internet bill", TransactionAmount = "60", TransactionDate = DateTime.Today.AddDays(-14) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.HomeLoanEMI.ToString(), TransactionCategory = ExpenseCategory.Mortgage.ToString(), TransactionDescription = "Home loan installment", TransactionAmount = "600", TransactionDate = DateTime.Today.AddDays(-15) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.AnnualBonus.ToString(), TransactionCategory = IncomeCategory.ExtraIncome.ToString(), TransactionDescription = "Yearly performance bonus", TransactionAmount = "300", TransactionDate = DateTime.Today.AddDays(-16) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.NetflixSubscription.ToString(), TransactionCategory = ExpenseCategory.Bills.ToString(), TransactionDescription = "Monthly Netflix charge", TransactionAmount = "20", TransactionDate = DateTime.Today.AddDays(-17) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.OnlineCourse.ToString(), TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Udemy course fee", TransactionAmount = "100", TransactionDate = DateTime.Today.AddDays(-18) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.CafeteriaLunch.ToString(), TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Lunch at office cafeteria", TransactionAmount = "15", TransactionDate = DateTime.Today.AddDays(-19) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.BusTicket.ToString(), TransactionCategory = ExpenseCategory.Transportation.ToString(), TransactionDescription = "Daily commute expense", TransactionAmount = "10", TransactionDate = DateTime.Today.AddDays(-20) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.SideProjectPayment.ToString(), TransactionCategory = IncomeCategory.Freelance.ToString(), TransactionDescription = "Payment from a freelance project", TransactionAmount = "600", TransactionDate = DateTime.Today.AddDays(-21) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.ConcertTicket.ToString(), TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Concert event ticket", TransactionAmount = "120", TransactionDate = DateTime.Today.AddDays(-22) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.ElectronicsPurchase.ToString(), TransactionCategory = ExpenseCategory.Shopping.ToString(), TransactionDescription = "New phone purchase", TransactionAmount = "450", TransactionDate = DateTime.Today.AddDays(-23) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.HealthInsurance.ToString(), TransactionCategory = ExpenseCategory.Insurance.ToString(), TransactionDescription = "Monthly insurance premium", TransactionAmount = "220", TransactionDate = DateTime.Today.AddDays(-26) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.MutualFundDividend.ToString(), TransactionCategory = IncomeCategory.Dividends.ToString(), TransactionDescription = "Dividend payout from mutual funds", TransactionAmount = "200", TransactionDate = DateTime.Today.AddDays(-27) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.WaterBill.ToString(), TransactionCategory = ExpenseCategory.Utilities.ToString(), TransactionDescription = "Monthly water bill", TransactionAmount = "55", TransactionDate = DateTime.Today.AddDays(-28) },
            new Transaction { TransactionType = "Income", TransactionName = IncomeSubCategory.FixedDepositInterest.ToString(), TransactionCategory = IncomeCategory.Interest.ToString(), TransactionDescription = "Interest earned on FD", TransactionAmount = "120", TransactionDate = DateTime.Today.AddDays(-29) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.SnacksPurchase.ToString(), TransactionCategory = ExpenseCategory.Food.ToString(), TransactionDescription = "Snacks from a local store", TransactionAmount = "20", TransactionDate = DateTime.Today.AddDays(-3) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.MobileRecharge.ToString(), TransactionCategory = ExpenseCategory.Bills.ToString(), TransactionDescription = "Monthly mobile data plan", TransactionAmount = "35", TransactionDate = DateTime.Today.AddDays(-5) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.GameSubscription.ToString(), TransactionCategory = ExpenseCategory.Others.ToString(), TransactionDescription = "Subscription for online gaming service", TransactionAmount = "25", TransactionDate = DateTime.Today.AddDays(-15) },
            new Transaction { TransactionType = "Expense", TransactionName = ExpenseSubCategory.PharmacyExpenses.ToString(), TransactionCategory = ExpenseCategory.HealthCare.ToString(), TransactionDescription = "Medicines and supplements", TransactionAmount = "60", TransactionDate = DateTime.Today.AddDays(-20) }
        };

        static DateTime today = DateTime.Today;
        ObservableCollection<Savings> savingsList = new ObservableCollection<Savings>
        {
            new Savings { SavingsDate = today, SavingsDescription = "General Savings", SavingsType = "Deposit", SavingsAmount = "500", SavingsRemark = "Salary surplus" },
            new Savings { SavingsDate = today.AddDays(-1), SavingsDescription = "Savings Usage", SavingsType = "Withdrawal", SavingsAmount = "100", SavingsRemark = "Minor house repairs" },
            new Savings { SavingsDate = today.AddDays(-2), SavingsDescription = "Flexible Fund", SavingsType = "Deposit", SavingsAmount = "200", SavingsRemark = "Bonus from employer" },
            new Savings { SavingsDate = today.AddDays(-3), SavingsDescription = "Emergency Access", SavingsType = "Withdrawal", SavingsAmount = "50", SavingsRemark = "Grocery adjustment" },
            new Savings { SavingsDate = today.AddDays(-4), SavingsDescription = "Miscellaneous Savings", SavingsType = "Deposit", SavingsAmount = "300", SavingsRemark = "Unexpected refund" },
            new Savings { SavingsDate = today.AddDays(-5), SavingsDescription = "General Fund Withdrawal", SavingsType = "Withdrawal", SavingsAmount = "75", SavingsRemark = "Car maintenance" },
            new Savings { SavingsDate = today.AddDays(-6), SavingsDescription = "Holiday Fund", SavingsType = "Deposit", SavingsAmount = "600", SavingsRemark = "Planned vacation savings" },
            new Savings { SavingsDate = today.AddDays(-7), SavingsDescription = "Medical Expenses", SavingsType = "Withdrawal", SavingsAmount = "120", SavingsRemark = "Doctor visit and medicines" },
            new Savings { SavingsDate = today.AddDays(-8), SavingsDescription = "Investment Savings", SavingsType = "Deposit", SavingsAmount = "800", SavingsRemark = "Monthly stock investments" },
            new Savings { SavingsDate = today.AddDays(-9), SavingsDescription = "Home Renovation", SavingsType = "Withdrawal", SavingsAmount = "400", SavingsRemark = "Paint and repairs" },
            new Savings { SavingsDate = today.AddDays(-10), SavingsDescription = "Car Repair Fund", SavingsType = "Deposit", SavingsAmount = "500", SavingsRemark = "Fund for future car service" },
            new Savings { SavingsDate = today.AddDays(-1), SavingsDescription = "Car Maintenance", SavingsType = "Withdrawal", SavingsAmount = "95", SavingsRemark = "Oil change and service" },
            new Savings { SavingsDate = today.AddDays(-2), SavingsDescription = "Bonus Savings", SavingsType = "Deposit", SavingsAmount = "900", SavingsRemark = "Annual performance bonus" },
            new Savings { SavingsDate = today.AddDays(-3), SavingsDescription = "Charity Donation", SavingsType = "Withdrawal", SavingsAmount = "150", SavingsRemark = "Support for local NGO" },
            new Savings { SavingsDate = today.AddDays(-4), SavingsDescription = "College Fund", SavingsType = "Deposit", SavingsAmount = "700", SavingsRemark = "Education savings for child" },
            new Savings { SavingsDate = today.AddDays(-5), SavingsDescription = "Subscription Payment", SavingsType = "Withdrawal", SavingsAmount = "30", SavingsRemark = "Monthly app subscription" },
            new Savings { SavingsDate = today.AddDays(-6), SavingsDescription = "Grocery Savings", SavingsType = "Deposit", SavingsAmount = "250", SavingsRemark = "Set aside for monthly groceries" },
            new Savings { SavingsDate = today.AddDays(-7), SavingsDescription = "Electricity Bill", SavingsType = "Withdrawal", SavingsAmount = "85", SavingsRemark = "Monthly electricity payment" },
            new Savings { SavingsDate = today.AddDays(-8), SavingsDescription = "Emergency Fund", SavingsType = "Deposit", SavingsAmount = "450", SavingsRemark = "Fund for emergency purpose" },
            new Savings { SavingsDate = today.AddDays(-9), SavingsDescription = "Dining Out", SavingsType = "Withdrawal", SavingsAmount = "60", SavingsRemark = "Weekend dinner" }
        };

        public static List<string> BudgetCategory = new List<string>()
        {
            "Monthly Budget", "Vacation Budget", "Transport Budget"
        };

        private ObservableCollection<Budget> BudgetList = new ObservableCollection<Budget>()
        {
            new Budget { BudgetTitle = "Budget for March 2025", BudgetAmount = 35000, BudgetCategory = BudgetCategory[0], BudgetDate = new DateTime (2025, 03, 01), IsCompleted = false, Remarks = "Monthly budget for March 2025" },
            new Budget { BudgetTitle = "Family Trip to Hawaii", BudgetAmount = 8000, BudgetCategory = BudgetCategory[1], BudgetDate = new DateTime(2025, 02, 28), IsCompleted = false, Remarks = "Vacation budget for the year 2025"},
            new Budget { BudgetTitle = "Transport expenses", BudgetAmount = 15000, BudgetCategory = BudgetCategory[2], BudgetDate = new DateTime(2025, 03, 02), IsCompleted = false, Remarks = "Transportation budget for the year 2025"},
            new Budget { BudgetTitle = "Budget for February 2025", BudgetAmount = 33000, BudgetCategory = BudgetCategory[0], BudgetDate = new DateTime (2025, 02, 01), IsCompleted = true, Remarks = "Monthly budget for February 2025" },
        };

        private ObservableCollection<Goal> GoalsList = new ObservableCollection<Goal>()
        {
            new Goal
            {
                GoalTitle = "Vacation Fund",
                GoalAmount = 35000,
                GoalPriority = GoalPriority.Medium,
                GoalDate = DateTime.Today.AddDays(30),
                IsCompleted = false,
                Remarks = "Family trip to Hawai",
                Transactions = new ObservableCollection<GoalsTransaction>
                {
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-5), TransactionDescription = "Vacation Fund", ContributionAmount = 500, Remarks = "Fund Contribution" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-10), TransactionDescription = "Vacation Fund", ContributionAmount = 400, Remarks = "Fund Contribution" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-15), TransactionDescription = "Vacation Fund", ContributionAmount = 600, Remarks = "Bonus Savings" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-20), TransactionDescription = "Vacation Fund", ContributionAmount = 800, Remarks = "Extra Savings" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-25), TransactionDescription = "Vacation Fund", ContributionAmount = 1000, Remarks = "Side Hustle" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-30), TransactionDescription = "Vacation Fund", ContributionAmount = 1200, Remarks = "Freelance Income" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-35), TransactionDescription = "Vacation Fund", ContributionAmount = 1500, Remarks = "Bonus Salary" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-40), TransactionDescription = "Vacation Fund", ContributionAmount = 900, Remarks = "Cashback Bonus" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-45), TransactionDescription = "Vacation Fund", ContributionAmount = 700, Remarks = "Stock Dividend" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-50), TransactionDescription = "Vacation Fund", ContributionAmount = 650, Remarks = "Cash Deposit" }
                }
            },
            new Goal
            {
                GoalTitle = "Wedding Fund",
                GoalAmount = 73000,
                GoalPriority = GoalPriority.High,
                GoalDate = DateTime.Today.AddDays(120), // 120 days from today
                IsCompleted = true,
                Remarks = "Wedding expenses",
                Transactions = new ObservableCollection<GoalsTransaction>
                {
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-7), TransactionDescription = "Wedding Fund", ContributionAmount = 10000, Remarks = "Initial Savings" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-14), TransactionDescription = "Wedding Fund", ContributionAmount = 9000, Remarks = "Extra Savings" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-21), TransactionDescription = "Wedding Fund", ContributionAmount = 8500, Remarks = "Bonus Salary" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-28), TransactionDescription = "Wedding Fund", ContributionAmount = 8000, Remarks = "Gift Contribution" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-35), TransactionDescription = "Wedding Fund", ContributionAmount = 7000, Remarks = "Investment Return" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-42), TransactionDescription = "Wedding Fund", ContributionAmount = 7000, Remarks = "Savings Allocation" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-49), TransactionDescription = "Wedding Fund", ContributionAmount = 6500, Remarks = "Cash Deposit" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-56), TransactionDescription = "Wedding Fund", ContributionAmount = 6000, Remarks = "Family Contribution" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-63), TransactionDescription = "Wedding Fund", ContributionAmount = 5500, Remarks = "Tax Refund" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-70), TransactionDescription = "Wedding Fund", ContributionAmount = 5500, Remarks = "Stock Dividend" }
                }
            },
            new Goal
            {
                GoalTitle = "Emergency Fund",
                GoalAmount = 50000,
                GoalPriority = GoalPriority.High,
                GoalDate = DateTime.Today.AddDays(60),
                IsCompleted = false,
                Remarks = "Backup savings for emergencies",
                Transactions = new ObservableCollection<GoalsTransaction>
                {
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-7), TransactionDescription = "Emergency Fund", ContributionAmount = 1000, Remarks = "Salary Contribution" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-12), TransactionDescription = "Emergency Fund", ContributionAmount = 1200, Remarks = "Savings" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-17), TransactionDescription = "Emergency Fund", ContributionAmount = 900, Remarks = "Investment Return" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-22), TransactionDescription = "Emergency Fund", ContributionAmount = 1100, Remarks = "Bonus Savings" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-27), TransactionDescription = "Emergency Fund", ContributionAmount = 1300, Remarks = "Extra Contribution" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-32), TransactionDescription = "Emergency Fund", ContributionAmount = 800, Remarks = "Cash Deposit" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-37), TransactionDescription = "Emergency Fund", ContributionAmount = 950, Remarks = "Extra Income" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-42), TransactionDescription = "Emergency Fund", ContributionAmount = 1250, Remarks = "Tax Refund" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-47), TransactionDescription = "Emergency Fund", ContributionAmount = 1050, Remarks = "Stock Dividend" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-52), TransactionDescription = "Emergency Fund", ContributionAmount = 1400, Remarks = "Government Stimulus" }
                }
            },
            new Goal
            {
                GoalTitle = "Home Renovation",
                GoalAmount = 75000,
                GoalPriority = GoalPriority.Low,
                GoalDate = DateTime.Today.AddDays(220),
                IsCompleted = false,
                Remarks = "Kitchen and Living Room upgrade",
                Transactions = new ObservableCollection<GoalsTransaction>
                {
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-10), TransactionDescription = "Home Renovation", ContributionAmount = 2000, Remarks = "Home Repair" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-15), TransactionDescription = "Home Renovation", ContributionAmount = 1500, Remarks = "Woodwork" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-20), TransactionDescription = "Home Renovation", ContributionAmount = 1000, Remarks = "Initial Deposit" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-25), TransactionDescription = "Home Renovation", ContributionAmount = 1800, Remarks = "Painting" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-30), TransactionDescription = "Home Renovation", ContributionAmount = 2100, Remarks = "Electrical Work" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-35), TransactionDescription = "Home Renovation", ContributionAmount = 1750, Remarks = "Flooring" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-40), TransactionDescription = "Home Renovation", ContributionAmount = 2200, Remarks = "Furniture Upgrade" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-45), TransactionDescription = "Home Renovation", ContributionAmount = 2500, Remarks = "Plumbing" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-50), TransactionDescription = "Home Renovation", ContributionAmount = 1950, Remarks = "Material Purchase" },
                    new GoalsTransaction { TransactionDate = DateTime.Today.AddDays(-55), TransactionDescription = "Home Renovation", ContributionAmount = 2300, Remarks = "Contractor Fees" }
                }
            }
        };

        public ObservableCollection<Transaction> GetDailyTransactions(DateTime? startDate = null, DateTime? endDate = null, double? transactionId = null)
        {
            if (transactionId == null && startDate != null && endDate != null)
            {
                var filteredTransactions = this.dailyTransaction.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).OrderByDescending(t => t.TransactionDate).ToObservableCollection();
                return filteredTransactions;
            }
            else if(transactionId != null)
            {
                var transactionById = this.dailyTransaction.Where(t => t.TransactionId == transactionId).OrderByDescending(t => t.TransactionDate).ToObservableCollection();
                return transactionById;
            }
            return this.dailyTransaction.OrderByDescending(t => t.TransactionDate).ToObservableCollection();
        }

        public Transaction GetTransactionById(double transactionId)
        {
            return GetDailyTransactions(null, null, transactionId).ElementAt(0);
        }

        public Budget GetBudgetById(double budgetId)
        {
            return GetBudgetList(null, budgetId).ElementAt(0);
        }

        public Goal GetGoalById(double goalId)
        {
            return GetGoalsData(null, goalId).ElementAt(0);
        }

        public ObservableCollection<Savings> GetSavingsData(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate != null && endDate != null)
            {
                var filteredSavings = this.savingsList.Where(t => t.SavingsDate >= startDate && t.SavingsDate <= endDate).OrderBy(t => t.SavingsDate).ToObservableCollection();
                return filteredSavings;
            }
            return this.savingsList;
        }

        public ObservableCollection<Budget> GetBudgetList(string? budgetStatus = null, double? budgetId = null)
        {
            if (budgetStatus != null)
            {
                var selectedBudgetList = this.BudgetList.Where(t => t.IsCompleted == (budgetStatus == "Active Budget" ? false : true)).ToObservableCollection<Budget>();
                return selectedBudgetList;
            }
            else if(budgetId != null)
            {
                return this.BudgetList.Where(t => t.BudgetId == budgetId).ToObservableCollection();
            }
            return this.BudgetList;
        }

        public ObservableCollection<Goal> GetGoalsData(string? goalStatus = null, double? goalId = null)
        {
            var selectedGoalData = this.GoalsList;
            if (goalStatus != null)
            {
                selectedGoalData = this.GoalsList.Where(t => t.IsCompleted == (goalStatus == "Active" ? false : true)).ToObservableCollection<Goal>();
            }
            else if(goalId != null)
            {
                selectedGoalData = this.GoalsList.Where(t => t.GoalId == goalId).ToObservableCollection();
            }
            // Update ContributionAmount for each goal
            foreach (var goal in selectedGoalData)
            {
                goal.ContributionAmount = goal.Transactions.Sum(t => t.ContributionAmount);
            }
            return selectedGoalData;
        }

        public double GetPreviousBalance()
        {
            return previousBalance;
        }   

        public async Task<bool> UpdateSavings(Savings savingsData, double? transactionId = null)
        {
            try
            {
                if(transactionId  != null)
                {
                    int index = savingsList.IndexOf(savingsList.FirstOrDefault(t => t.TransactionId == transactionId));
                    savingsList[index] = savingsData;
                }
                else
                {
                    this.savingsList.Add(savingsData);
                }                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTransaction(Transaction transactionData, double? transactionId = null)
        {
            try
            {
                if (transactionId != null)
                {
                    int index = dailyTransaction.IndexOf(dailyTransaction.FirstOrDefault(t => t.TransactionId == transactionId));
                    dailyTransaction[index] = transactionData;
                }
                else
                {
                    this.dailyTransaction.Add(transactionData);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateBudget(Budget budgetData, double? budgetId = null)
        {
            try
            {
                if (budgetId != null)
                {
                    int index = BudgetList.IndexOf(BudgetList.FirstOrDefault(t => t.BudgetId == budgetId));
                    BudgetList[index] = budgetData;
                }
                else
                {
                    this.BudgetList.Add(budgetData);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateGoal(Goal goalData, double? goalId = null)
        {
            try
            {
                if(goalId != null)
                {
                    int index = GoalsList. IndexOf(GoalsList.FirstOrDefault(t => t.GoalId == goalId));
                    GoalsList[index] = goalData;
                }
                else
                {
                    this.GoalsList.Add(goalData);
                }                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UpdateGoalTransaction(GoalsTransaction goalData, double goalId, double? transactionId = null)
        {
            try
            {
                var goal = GoalsList.FirstOrDefault(t => t.GoalId == goalId);
                if (goal == null)
                    return false;

                if (transactionId != null)
                {
                    var transactions = goal.Transactions;
                    int index = transactions.IndexOf(transactions.FirstOrDefault(t => t.TransactionId == transactionId));
                    if (index >= 0)
                    {
                        transactions[index] = goalData;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    goal.Transactions.Add(goalData);
                }
                // Update ContributionAmount after change
                goal.ContributionAmount = goal.Transactions.Sum(t => t.ContributionAmount);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public Savings GetSavingsByTransactionId(double transactionId)
        {
            return savingsList.Where(t => transactionId.Equals(t.TransactionId)).First();            
        }

        public bool DeleteTransactions(List<double> transactionIds, string category)
        {
            try
            {
                switch(category)
                {
                    case "Savings":
                        var itemsToRemove = savingsList.Where(t => transactionIds.Contains(t.TransactionId)).ToList().FirstOrDefault();
                        savingsList.Remove(itemsToRemove);
                        break;

                    case "Transaction":
                        var transactionToRemove = dailyTransaction.Where(t => transactionIds.Contains(t.TransactionId)).ToList().FirstOrDefault();
                        dailyTransaction.Remove(transactionToRemove);
                        break;

                    case "Budget":
                        var budgetToRemove = BudgetList.Where(t => transactionIds.Contains(t.BudgetId)).ToList().FirstOrDefault();
                        BudgetList.Remove(budgetToRemove);
                        break;

                    case "Goal":
                        var goalToRemove = GoalsList.Where(t => transactionIds.Contains(t.GoalId)).ToList().FirstOrDefault();
                        GoalsList.Remove(goalToRemove);
                        break;
                }
                return true; 
            }
            catch(Exception e)
            {
                return false;
            }
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

        public double TransactionId { get; set; }

        public string? TransactionType
        {
            get { return this.transactionType; }
            set { this.transactionType = value; }
        }

        public string? TransactionName
        {
            get { return this.transactionName; }
            set { this.transactionName = value; }
        }

        public string? TransactionCategory
        {
            get { return this.transactionCategory; }
            set { this.transactionCategory = value; }
        }

        public string? TransactionDescription
        {
            get { return this.transactionDescription; }
            set { this.transactionDescription = value; }
        }

        public string TransactionAmount
        {
            get { return this.transactionAmount; }
            set { this.transactionAmount = value; }
        }

        public DateTime TransactionDate
        {
            get { return this.transactionDate; }
            set { this.transactionDate = value; }
        }

        public Transaction()
        {
            // Use ticks to generate a unique double based on date and time
            TransactionId = DateTime.UtcNow.Ticks / 10000.0;
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

    public enum IncomeSubCategory
    {
        BlogWriting,
        MonthlySalary,
        StockDividends,
        SavingsInterest,
        AnnualBonus,
        SideProjectPayment,
        MutualFundDividend,
        FixedDepositInterest
    }

    public enum ExpenseSubCategory
    {
        Groceries,
        Fuel,
        ApartmentRent,
        MovieTicket,
        ClothingPurchase,
        DoctorVisit,
        RestaurantDinner,
        ElectricityBill,
        BroadbandBill,
        HomeLoanEMI,
        NetflixSubscription,
        OnlineCourse,
        CafeteriaLunch,
        BusTicket,
        ConcertTicket,
        ElectronicsPurchase,
        HealthInsurance,
        WaterBill,
        SnacksPurchase,
        MobileRecharge,
        GameSubscription,
        PharmacyExpenses
    }


    public class Budget
    {
        private string? budgetTitle;
        private string budgetCategory;
        private double budgetAmount;
        private DateTime budgetDate;
        private string? budgetRemarks;
        private bool isCompleted;

        public double BudgetId { get; set; }

        public string? BudgetTitle
        {
            get { return this.budgetTitle; }
            set { this.budgetTitle = value; }
        }

        public string BudgetCategory
        {
            get { return this.budgetCategory; }
            set { this.budgetCategory = value; }
        }

        public double BudgetAmount
        {
            get { return this.budgetAmount; }
            set { this.budgetAmount = value; }
        }

        public DateTime BudgetDate
        {
            get { return this.budgetDate; }
            set { this.budgetDate = value; }
        }

        public string? Remarks
        {
            get { return this.budgetRemarks; }
            set { this.budgetRemarks = value; }
        }

        public bool IsCompleted
        {
            get { return this.isCompleted; }
            set { this.isCompleted = value; }
        }

        public Budget()
        {
            // Use ticks to generate a unique double based on date and time
            BudgetId = DateTime.UtcNow.Ticks / 10000.0;
        }
    }

    //public enum BudgetCategory
    //{
    //    [Description("Monthly Budget")]
    //    MonthlyBudget,

    //    [Description("Vacation Budget")]
    //    VacationBudget,

    //    [Description("Transport Budget")]
    //    TransportBudget,
    //}

    public class Savings
    {
        private double transactionId;

        private string? savingsType;

        private string? savingsDescription;

        private string? savingsAmount;

        private DateTime savingsDate;

        private string? savingsRemark;

        public double TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = value;
            }
        }

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
        public Savings()
        {
            // Use ticks to generate a unique double based on date and time
            TransactionId = DateTime.UtcNow.Ticks / 10000.0;
        }
    }

    public class Goal
    {
        private double goalId;

        private string? goalTitle;

        private GoalPriority goalPriority;

        private double goalAmount;

        private double contributionAmount;

        private DateTime goalDate;

        private string? goalRemarks;

        private bool isCompleted;

        private ObservableCollection<GoalsTransaction> transactions;

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

        public bool IsPopupOpen
        {
            get; set;
        }

        public Goal()
        {
            GoalId = DateTime.UtcNow.Ticks / 10000.0;
            IsPopupOpen = false;
        }
    }

    public class GoalsTransaction : INotifyPropertyChanged
    {
        private double transactionId;

        private bool isSelected;

        private DateTime transactionDate;

        private string transactionDescription;

        private double contributionAmount;

        private string remarks;

        public double TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                OnPropertyChanged("IsSelected");
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

        public string TransactionDescription
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

        public string Remarks
        {
            get
            {
                return this.remarks;
            }
            set
            {
                this.remarks = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
        // Add this constructor to set TransactionId as double based on DateTime.UtcNow.Ticks
        public GoalsTransaction()
        {
            // Use ticks to generate a unique double based on date and time
            this.TransactionId = DateTime.UtcNow.Ticks / 10000.0;
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
