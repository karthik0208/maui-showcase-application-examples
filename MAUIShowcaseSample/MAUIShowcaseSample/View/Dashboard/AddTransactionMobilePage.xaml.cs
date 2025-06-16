using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class AddTransactionMobilePage : ContentView
{
    DashboardLayoutPage dashboardLayoutPage;

	public AddTransactionMobilePage(DashboardLayoutPageViewModel viewModel, DashboardLayoutPage layoutPage, UserDataService userCredentials, DataStore dataStore)
	{
		InitializeComponent();
		BindingContext = viewModel;
        dashboardLayoutPage = layoutPage;
    }

    private void OnFabClicked(object sender, EventArgs e)
    {
        //var parentPage = this.Parent.BindingContext;

        //if (parentPage is DashboardPageViewModel)
        //{
        //    ((DashboardPageViewModel)parentPage).IsPageEnabled = !((DashboardPageViewModel)parentPage).IsPageEnabled;
        //    ((DashboardPageViewModel)parentPage).PageBackgroundColor = ((DashboardPageViewModel)parentPage).PageBackgroundColor == Colors.Transparent ? Colors.Gray : Colors.Transparent;
        //}
        //else if (parentPage is BudgetPageViewModel)
        //{
        //    ((BudgetPageViewModel)parentPage).IsPageEnabled = !((BudgetPageViewModel)parentPage).IsPageEnabled;
        //}
        //else if (parentPage is TransactionPageViewModel)
        //{
        //    ((TransactionPageViewModel)parentPage).IsPageEnabled = !((TransactionPageViewModel)parentPage).IsPageEnabled;
        //}
        //else if (parentPage is SavingsPageViewModel)
        //{
        //    ((SavingsPageViewModel)parentPage).IsPageEnabled = !((SavingsPageViewModel)parentPage).IsPageEnabled;
        //}
        //else if (parentPage is GoalsPageViewModel)
        //{
        //    ((GoalsPageViewModel)parentPage).IsPageEnabled = !((GoalsPageViewModel)parentPage).IsPageEnabled;
        //}

        this.FabMenu.ShowRelativeToView(this.MainFab, Syncfusion.Maui.Toolkit.Popup.PopupRelativePosition.AlignTopLeft);
    }

    private void OnGoalClicked(object sender, EventArgs e)
    {
        dashboardLayoutPage.TriggerEditGoalPopup();
    }

    private void OnSavingsClicked(object sender, EventArgs e)
    {
        dashboardLayoutPage.TriggerEditSavePopup();
    }

    private void OnBudgetClicked(object sender, EventArgs e)
    {
        dashboardLayoutPage.TriggerEditBudgetPopup();
    }

    private void OnTransactionClicked(object sender, EventArgs e)
    {
        dashboardLayoutPage.TriggerEditTransactionPopup();
    }
}