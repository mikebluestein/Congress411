using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Linq;

namespace Congress411
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Senators = new ObservableCollection<PoliticianViewModel>();
            this.Representatives = new ObservableCollection<PoliticianViewModel>();
        }

        public ObservableCollection<PoliticianViewModel> Senators { get; private set; }
        public ObservableCollection<PoliticianViewModel> Representatives { get; private set; }

        private PoliticianViewModel _selectedPolitician;
        public PoliticianViewModel SelectedPolitician
        {
            get
            {
                return _selectedPolitician;
            }
            set
            {
                if (value != _selectedPolitician)
                {
                    _selectedPolitician = value;
                    NotifyPropertyChanged("SelectedPolitician");
                }
            }
        }

        private FeedItem _selectedFeedItem;
        public FeedItem SelectedFeedItem
        {
            get
            {
                return _selectedFeedItem;
            }
            set
            {
                if (value != _selectedFeedItem)
                {
                    _selectedFeedItem = value;
                    NotifyPropertyChanged("SelectedFeedItem");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            StreamResourceInfo srSenate = Application.GetResourceStream(new Uri("Congress411;component/Data/Senate.xml", UriKind.Relative));
            XDocument senatorsXml = XDocument.Load(srSenate.Stream);
            var senateList = (from politician in senatorsXml.Descendants("Politician")
                              select new PoliticianViewModel
                              {
                                  BioGuideId = politician.Element("BioGuideId").Value,
                                  FirstName = politician.Element("FirstName").Value,
                                  LastName = politician.Element("LastName").Value,
                                  GovTrackId = politician.Element("GovTrackId").Value,
                                  Party = politician.Element("Party").Value,
                                  Phone = politician.Element("Phone").Value,
                                  TwitterId = politician.Element("TwitterId").Value,
                                  State = politician.Element("State").Value,
                                  WebSite = politician.Element("WebSite").Value,
                                  OfficeAddress = politician.Element("OfficeAddress").Value,
                                  SmallImage = String.Format("./Images/{0}.jpg", politician.Element("BioGuideId").Value)
                              }).OrderBy(p => p.LastName).ToList();

            foreach (var senator in senateList)
                Senators.Add(senator);

            senateList = null;

            StreamResourceInfo srHouse = Application.GetResourceStream(new Uri("Congress411;component/Data/House.xml", UriKind.Relative));
            XDocument repsXml = XDocument.Load(srHouse.Stream);
            var repList = (from politician in repsXml.Descendants("Politician")
                           select new PoliticianViewModel
                           {
                               BioGuideId = politician.Element("BioGuideId").Value,
                               FirstName = politician.Element("FirstName").Value,
                               LastName = politician.Element("LastName").Value,
                               GovTrackId = politician.Element("GovTrackId").Value,
                               Party = politician.Element("Party").Value,
                               Phone = politician.Element("Phone").Value,
                               TwitterId = politician.Element("TwitterId").Value,
                               State = politician.Element("State").Value,
                               WebSite = politician.Element("WebSite").Value,
                               OfficeAddress = politician.Element("OfficeAddress").Value,
                               SmallImage = String.Format("./Images/{0}.jpg", politician.Element("BioGuideId").Value)
                           }).OrderBy(p => p.LastName).ToList();

            foreach (var rep in repList)
                Representatives.Add(rep);

            repList = null;

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}