using MAUIShowcaseSample.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// Static data store for managing navigation-related view models across the application
    /// Provides centralized access to view models that need to persist data during navigation
    /// </summary>
    public static class NavigationDataStore
    {
        #region View Model Properties

        /// <summary>
        /// Gets or sets the view model for the Goal Detail page
        /// Used to maintain goal-related data and state during navigation
        /// </summary>
        /// <value>The GoalDetailPageViewModel instance containing goal details and operations</value>
        public static GoalDetailPageViewModel GoalDetailPageViewModel { get; set; }

        /// <summary>
        /// Gets or sets the view model for the Budget Detail page
        /// Used to maintain budget-related data and state during navigation
        /// </summary>
        /// <value>The BudgetDetailPageViewModel instance containing budget details and operations</value>
        public static BudgetDetailPageViewModel BudgetDetailPageViewModel { get; set; }

        /// <summary>
        /// Gets or sets the view model for the Settings page
        /// Used to maintain settings-related data and state during navigation
        /// </summary>
        /// <value>The SettingsPageViewModel instance containing user settings and preferences</value>
        public static SettingsPageViewModel SettingsPageViewModel { get; set; }

        #endregion
    }
}

