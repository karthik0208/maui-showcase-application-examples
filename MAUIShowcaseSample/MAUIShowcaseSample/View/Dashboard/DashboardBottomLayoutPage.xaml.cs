using Syncfusion.Maui.Toolkit.Buttons;
using System.Threading.Tasks;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class DashboardBottomLayoutPage : ContentView
{
	public DashboardBottomLayoutPage(DashboardLayoutPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    public async void OnIconClicked(object sender, EventArgs e)
    {
        if (sender is SfButton button)
        {
            var commandParam = button.CommandParameter?.ToString();

            if (commandParam == "Dashboard")
            {
                await Shell.Current.GoToAsync("dashboard");
            }
            else if (commandParam == "Transaction")
            {
                await Shell.Current.GoToAsync("transaction");
            }
            else if (commandParam == "Budget")
            {
                await Shell.Current.GoToAsync("budget");
            }
            else if (commandParam == "Savings")
            {
                await Shell.Current.GoToAsync("savings");
            }
            else if (commandParam == "Goal")
            {
                await Shell.Current.GoToAsync("goal");
            }
        }
    }
}