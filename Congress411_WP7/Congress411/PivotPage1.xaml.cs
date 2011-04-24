using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Text;

namespace Congress411
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        ProgressBar _bar = null;

        public PivotPage1()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PivotPage1_Loaded);

            PoliticianPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(PoliticianPivot_LoadedPivotItem);
        }

        void AddProgressBar()
        {
            try
            {
                Style style = (Style)App.Current.Resources["PerformanceProgressBar"];
                _bar = new ProgressBar
                {
                    IsIndeterminate = true,
                    Style = style,
                };
                LayoutRoot.Children.Add(_bar);
            }
            catch { _bar = null; }
        }

        void RemoveProgressBar()
        {
            try
            {
                if (_bar != null)
                {
                    LayoutRoot.Children.Remove(_bar);

                    //not sure if I need to still do this since I'm removing the progress bar altogether, but can't hurt I suppose
                    //see: http://www.jeff.wilcox.name/2010/08/performanceprogressbar/
                    _bar.IsIndeterminate = false;
                    _bar = null;
                }
            }
            catch { _bar = null; }
        }

        void PoliticianPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            //HACK: remove any progress bar that inadvertently wasn't destroyed
            RemoveProgressBar();

            if (e.Item.Name == "BillsPivotItem")
            {
                try
                {
                    AddProgressBar();

                    string memberCode;
                    if (App.ViewModel.SelectedPolitician.IsSenator)
                        memberCode = "SMEMB";
                    else
                        memberCode = "HMEMB";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Dbd111=d111");
                    sb.Append("&srch=/bss/d111query.html");
                    sb.Append("&TYPE1=bimp");
                    sb.AppendFormat("&{0}={1}", memberCode, App.ViewModel.SelectedPolitician.LastName);
                    sb.Append("&Sponfld=SPON");
                    string data = sb.ToString();
                    string dataEncoded = data.Replace("/", "%2F");

                    Byte[] bytes = Encoding.UTF8.GetBytes(dataEncoded);
                    ThomasBillsBrowser.Navigate(new Uri("http://thomas.loc.gov/cgi-bin/bdquery"), bytes, "Content-Type: application/x-www-form-urlencoded");
                }
                catch
                {
                    RemoveProgressBar();
                }
            }
            else if (e.Item.Name == "VotesPivotItem")
            {
                try
                {
                    AddProgressBar();

                    RssFeed recentVoteFeed = new RssFeed();
                    recentVoteFeed.FeedCompleted += new EventHandler<FeedCompletedEventArgs>(recentVoteFeed_FeedCompleted);
                    recentVoteFeed.FeedError += new EventHandler(recentVoteFeed_FeedError);
                    recentVoteFeed.GetFeedAsync(String.Format("http://www.govtrack.us/users/events-rss2.xpd?monitors=pv:{0}&days=30", App.ViewModel.SelectedPolitician.GovTrackId));
                }
                catch
                {
                    RemoveProgressBar();
                }
            }
            else if (e.Item.Name == "TwitterPivotItem")
            {
                try
                {
                    AddProgressBar();

                    RssFeed twitterTimelineFeed = new RssFeed();
                    twitterTimelineFeed.FeedCompleted += new EventHandler<FeedCompletedEventArgs>(twitterTimelineFeed_FeedCompleted);
                    twitterTimelineFeed.FeedError += new EventHandler(twitterTimelineFeed_FeedError);
                    twitterTimelineFeed.GetFeedAsync(String.Format("http://twitter.com/statuses/user_timeline/{0}.rss", App.ViewModel.SelectedPolitician.TwitterId));
                }
                catch
                {
                    RemoveProgressBar();
                }
            }
            else if (e.Item.Name == "WebPivotItem")
            {
                AddProgressBar();

                try
                {
                    WebSiteBrowser.Navigate(new Uri(App.ViewModel.SelectedPolitician.WebSite));
                }
                catch
                {
                    RemoveProgressBar();
                }
            }
            else if (e.Item.Name == "ContactPivotItem")
            {
                ContactPivotItem.DataContext = App.ViewModel.SelectedPolitician;
            }
        }

        void twitterTimelineFeed_FeedError(object sender, EventArgs e)
        {
            RemoveProgressBar();

            //display message that twitter timeline is not available
            TwitterNotAvaialble.Visibility = Visibility.Visible;
        }

        void twitterTimelineFeed_FeedCompleted(object sender, FeedCompletedEventArgs e)
        {
            RemoveProgressBar();
            TwitterNotAvaialble.Visibility = Visibility.Collapsed;

            Twitter.NavSvc = NavigationService;
            Twitter.DataContext = e.FeedItems;
        }

        void recentVoteFeed_FeedError(object sender, EventArgs e)
        {
            RemoveProgressBar();

            VotesNotAvaialble.Visibility = Visibility.Visible;
        }

        void recentVoteFeed_FeedCompleted(object sender, FeedCompletedEventArgs e)
        {
            RemoveProgressBar();
            VotesNotAvaialble.Visibility = Visibility.Collapsed;

            VotesList.ItemsSource = e.FeedItems;
        }

        void PivotPage1_Loaded(object sender, RoutedEventArgs e)
        {
            PoliticianPivot.Title = String.Format("{0} {1}", App.ViewModel.SelectedPolitician.FirstName, App.ViewModel.SelectedPolitician.LastName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var feedItem = (sender as Button).DataContext as FeedItem;
            if (feedItem != null)
            {
                App.ViewModel.SelectedFeedItem = feedItem;
                NavigationService.Navigate(new Uri("/GovTrackVoteDetail.xaml", UriKind.Relative));
            }
        }

        private void CallLegislator_Click(object sender, RoutedEventArgs e)
        {
            var phoneCall = new Microsoft.Phone.Tasks.PhoneCallTask
            {
                DisplayName = String.Format("{0} {1}", App.ViewModel.SelectedPolitician.FirstName, App.ViewModel.SelectedPolitician.LastName),
                PhoneNumber = App.ViewModel.SelectedPolitician.Phone
            };

            phoneCall.Show();
        }

        private void ThomasBillsBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            RemoveProgressBar();
        }

        private void WebSiteBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            RemoveProgressBar();
        }

    }
}