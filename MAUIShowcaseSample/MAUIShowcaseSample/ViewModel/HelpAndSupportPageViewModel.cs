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
    public class HelpAndSupportPageViewModel : INotifyPropertyChanged
    {
        private string searchText;

        private ObservableCollection<FAQItem> faqItemList;

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
                if (!string.IsNullOrEmpty(this.searchText) && char.IsWhiteSpace(this.searchText.Last()))
                {
                    FilterFAQItems().ConfigureAwait(false);
                }
            }
        }

        public HelpAndSupportPageViewModel()
        {
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

        private async Task FilterFAQItems() // Changed return type from 'void' to 'Task'
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FAQItemList = new ObservableCollection<FAQItem>(FAQItemList);
            }
            else
            {
                var filtered = FAQItemList
                    .Where(f => (f.Question?.ToLower().Contains(SearchText.ToLower()) ?? false) ||
                                (f.Answer?.ToLower().Contains(SearchText.ToLower()) ?? false))
                    .ToList();
                FAQItemList = new ObservableCollection<FAQItem>(filtered);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FAQItem
    {
        private string question;

        private string answer;

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
    }
}
