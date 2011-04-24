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
using System.Windows.Navigation;

namespace Congress411
{
    public partial class TwitterTimeline : UserControl
    {
        public NavigationService NavSvc { get; set; }

        public TwitterTimeline()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var feedItem = (sender as Button).DataContext as FeedItem;
            if (feedItem != null)
            {
                App.ViewModel.SelectedFeedItem = feedItem;
                if (feedItem != null)
                {
                    App.ViewModel.SelectedFeedItem = feedItem;
                    NavSvc.Navigate(new Uri("/TweetPage.xaml", UriKind.Relative));
                }
            }
        }
    }
}
