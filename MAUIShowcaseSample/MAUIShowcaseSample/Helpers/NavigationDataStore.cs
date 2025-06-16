using MAUIShowcaseSample.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIShowcaseSample
{
    public static class NavigationDataStore
    {
        public static GoalDetailPageViewModel GoalDetailPageViewModel { get; set; }

        public static BudgetDetailPageViewModel BudgetDetailPageViewModel { get; set; }

        public static SettingsPageViewModel SettingsPageViewModel { get; set; }
    }
}
