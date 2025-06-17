using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing help and support page operations including FAQ search and filtering
    /// </summary>
    public class HelpAndSupportPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Search text for filtering FAQ items
        /// </summary>
        private string searchText;

        /// <summary>
        /// Collection of FAQ items to display
        /// </summary>
        private ObservableCollection<FAQItem> faqItemList;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the collection of FAQ items
        /// </summary>
        /// <value>Observable collection of FAQItem objects</value>
        public ObservableCollection<FAQItem> FAQItemList
        {
            get
            {
                return this.faqItemList;
            }
            set
            {
                this.faqItemList = value;
                OnPropertyChanged(nameof(FAQItemList));
            }
        }

        /// <summary>
        /// Gets or sets the search text for filtering FAQ items
        /// </summary>
        /// <value>String representing the search query</value>
        public string SearchText
        {
            get
            {
                return this.searchText;
            }
            set
            {
                this.searchText = value;
                OnPropertyChanged(nameof(SearchText));
                
                // Trigger filtering when search text ends with whitespace
                if (!string.IsNullOrEmpty(this.searchText) && char.IsWhiteSpace(this.searchText.Last()))
                {
                    FilterFAQItems().ConfigureAwait(false);
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the HelpAndSupportPageViewModel class
        /// </summary>
        public HelpAndSupportPageViewModel()
        {
            // Initialize FAQ items with common questions and answers
            FAQItemList = new ObservableCollection<FAQItem>
            {
                new FAQItem
                {
                    Question = "How to set Budget?",
                    Answer = "Go to the Budget section in your app settings and enter your monthly or custom spending limits."
                },
                new FAQItem
                {
                    Question = "How can we edit goals?",
                    Answer = "Navigate to the Goals tab, tap the goal you wish to edit, and select the edit option from the menu."
                },
                new FAQItem
                {
                    Question = "How to pay?",
                    Answer = "You can pay through the Payments section using your preferred payment method like UPI, card, or wallet."
                },
                new FAQItem
                {
                    Question = "What is my current balance?",
                    Answer = "Your current balance is displayed on the home screen or under the Account Overview section."
                },
                new FAQItem
                {
                    Question = "When is my due for medical?",
                    Answer = "Check your Medical section for the next scheduled payment date under 'Upcoming Bills'."
                },
                new FAQItem
                {
                    Question = "How to set Budget?",
                    Answer = "To set a new budget, go to Budget → Add New Budget and define your limits and categories."
                },
            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Filters FAQ items based on the search text
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task FilterFAQItems()
        {
            // Show all items if search text is empty
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FAQItemList = new ObservableCollection<FAQItem>(FAQItemList);
            }
            else
            {
                // Filter items based on question or answer containing search text
                var filtered = FAQItemList
                    .Where(f => (f.Question?.ToLower().Contains(SearchText.ToLower()) ?? false) ||
                                (f.Answer?.ToLower().Contains(SearchText.ToLower()) ?? false))
                    .ToList();
                FAQItemList = new ObservableCollection<FAQItem>(filtered);
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
    }

    /// <summary>
    /// Represents a frequently asked question item with question and answer
    /// </summary>
    public class FAQItem
    {
        #region Private Fields

        /// <summary>
        /// The question text
        /// </summary>
        private string question;

        /// <summary>
        /// The answer text
        /// </summary>
        private string answer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the question text
        /// </summary>
        /// <value>String representing the FAQ question</value>
        public string Question
        {
            get
            {
                return this.question;
            }
            set
            {
                this.question = value;
            }
        }

        /// <summary>
        /// Gets or sets the answer text
        /// </summary>
        /// <value>String representing the FAQ answer</value>
        public string Answer
        {
            get
            {
                return this.answer;
            }
            set
            {
                this.answer = value;
            }
        }

        #endregion
    }
}