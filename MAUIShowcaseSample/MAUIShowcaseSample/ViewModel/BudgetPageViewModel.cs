using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class BudgetPageViewModel : INotifyPropertyChanged
    {
        private List<SfSegmentItem> segmentTitle;

        private int selectedSegmentIndex;

        private ObservableCollection<Budget> selectedSegmentData = new ObservableCollection<Budget>();

        private ObservableCollection<SummarizedBudgetData> budgetData = new();

        private string currencySymbol;

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

        public ObservableCollection<Budget> SelectedSegmentData
        {
            get
            {
                return this.selectedSegmentData;
            }
            set
            {
                this.selectedSegmentData = value;
            }
        }

        public ObservableCollection<SummarizedBudgetData> BudgetData
        {
            get
            {
                return this.budgetData;
            }
            set
            {
                this.budgetData = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BudgetPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "Active Budget"},
                new SfSegmentItem() {Text = "Completed Budget"}
            };
            SelectedSegmentIndex = 0;
            currencySymbol = userDataService.GetUserCurrencySymbol(userDataService.LoggedInAccount);
            selectedSegmentData = dataStore.GetBudgetList(SegmentTitle[SelectedSegmentIndex].Text);
            BudgetData = GetSummarizedBudgetData(selectedSegmentData, false);
        }

        private ObservableCollection<SummarizedBudgetData> GetSummarizedBudgetData(ObservableCollection<Budget> budgetData, bool isBudgetCompleted)
        {
            var summarizedData = new ObservableCollection<SummarizedBudgetData>();

            var filterData = budgetData.Where(t => t.IsCompleted == isBudgetCompleted);            

            foreach(var data in filterData)
            {
                var categoryIcon = GetCategoryIcon(data.BudgetCategory.ToString());
                summarizedData.Add(new SummarizedBudgetData() { BudgetTitle = data.BudgetTitle, BudgetCategory = data.BudgetCategory, BudgetDate = data.BudgetDate, BudgetAmount = data.BudgetAmount, CurrencySymbol = this.currencySymbol, Icon = categoryIcon });
            }
            return summarizedData;
        }

        public string GetCategoryIcon(string category)
        {
            string iconCode = "\ue73d";

            if (Enum.TryParse(category, out ExpenseCategory expenseCategory))
            {
                switch (expenseCategory)
                {
                    case ExpenseCategory.Mortgage:
                        iconCode = "\ue716";
                        break;

                    case ExpenseCategory.Food:
                        iconCode = "\ue744";
                        break;

                    case ExpenseCategory.Bills:
                        iconCode = "\ue738";
                        break;

                    case ExpenseCategory.Shopping:
                        iconCode = "\ue71a";
                        break;

                    case ExpenseCategory.Transportation:
                        iconCode = "\ue740";
                        break;

                    case ExpenseCategory.Insurance:
                        iconCode = "\ue750";
                        break;

                    case ExpenseCategory.HealthCare:
                        iconCode = "\ue751";
                        break;

                    case ExpenseCategory.Clothing:
                        iconCode = "\ue747";
                        break;

                    case ExpenseCategory.Utilities:
                        iconCode = "\ue743";
                        break;
                }
            }
            else if (Enum.TryParse(category, out IncomeCategory incomeCategory))
            {
                switch (incomeCategory)
                {
                    case IncomeCategory.Salary:
                        iconCode = "\ue734";
                        break;

                    case IncomeCategory.Freelance:
                        iconCode = "\ue726";
                        break;

                    case IncomeCategory.Interest:
                        iconCode = "\ue74a";
                        break;

                    case IncomeCategory.Dividends:
                        iconCode = "\ue74c";
                        break;

                    case IncomeCategory.ExtraIncome:
                        iconCode = "\ue737";
                        break;
                }
            }
            return iconCode;
        }
    }

    public class SummarizedBudgetData
    {
        private string? budgetTitle;

        private BudgetCategory budgetCategory;

        private double? budgetAmount;

        private DateTime budgetDate;

        private string? budgetRemarks;

        private bool isCompleted;

        private string? currencySymbol;

        private double amountSpent;

        private double remainingAmount;

        private double utilization;

        private string? icon;

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

        public double? BudgetAmount
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
    }
}
