
using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Congress411
{
    public class RssFeed
    {
        public List<FeedItem> FeedItems { get; set; }
        public event EventHandler<FeedCompletedEventArgs> FeedCompleted;
        public event EventHandler FeedError;

        public void GetFeedAsync(string url)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(url));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string xml = e.Result;
                StringReader sr = new StringReader(xml);
                XDocument voteFeed = XDocument.Load(sr);

                FeedItems = (from item in voteFeed.Descendants("item")
                             select new FeedItem
                             {
                                 Title = item.Element("title").Value,
                                 PubDate = DateTime.Parse(item.Element("pubDate").Value),
                                 Link = item.Element("link").Value,
                                 Description = item.Element("description").Value
                             }).OrderByDescending(fi => fi.PubDate).ToList();
            }
            catch (Exception)
            {
                if (FeedError != null)
                    FeedError(this, null);

                return;
            }

            if (FeedCompleted != null)
            {
                FeedCompleted(this, new FeedCompletedEventArgs(FeedItems));
            }
        }
    }

    public class FeedCompletedEventArgs : EventArgs
    {
        public List<FeedItem> FeedItems { get; set; }

        public FeedCompletedEventArgs(List<FeedItem> items)
        {
            FeedItems = items;
        }
    }

    public class FeedItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string Link { get; set; }
    }
}