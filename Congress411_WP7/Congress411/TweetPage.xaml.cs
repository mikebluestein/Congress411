﻿using System;
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

namespace Congress411
{
    public partial class TweetPage : PhoneApplicationPage
    {
        public TweetPage()
        {
            InitializeComponent();
        }

        private void TweetBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            TweetBrowser.Navigate(new Uri(App.ViewModel.SelectedFeedItem.Link));
        }
    }
}