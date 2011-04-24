using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace Congress411
{
    public class PoliticianViewModel : INotifyPropertyChanged
    {
        public bool IsSenator { get; set; }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        private string _bioGuideId;
        public string BioGuideId
        {
            get
            {
                return _bioGuideId;
            }
            set
            {
                if (value != _bioGuideId)
                {
                    _bioGuideId = value;
                    NotifyPropertyChanged("BioGuideId");
                }
            }
        }

        private string _govTrackId;
        public string GovTrackId
        {
            get
            {
                return _govTrackId;
            }
            set
            {
                if (value != _govTrackId)
                {
                    _govTrackId = value;
                    NotifyPropertyChanged("GovTrackId");
                }
            }
        }

        private string _party;
        public string Party
        {
            get
            {
                return _party;
            }
            set
            {
                if (value != _party)
                {
                    _party = value;
                    NotifyPropertyChanged("Party");
                }
            }
        }

        private string _webSite;
        public string WebSite
        {
            get
            {
                return _webSite;
            }
            set
            {
                if (value != _webSite)
                {
                    _webSite = value;
                    NotifyPropertyChanged("WebSite ");
                }
            }
        }

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        private string _twitterId;
        public string TwitterId
        {
            get
            {
                return _twitterId;
            }
            set
            {
                if (value != _twitterId)
                {
                    _twitterId = value;
                    NotifyPropertyChanged("TwitterId");
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }

        private string _officeAddress;
        public string OfficeAddress
        {
            get
            {
                return _officeAddress;
            }
            set
            {
                if (value != _officeAddress)
                {
                    _officeAddress = value;
                    NotifyPropertyChanged("OfficeAddress");
                }
            }
        }

        private string _smallImage; //the file name of the image (convert to image with value converter)
        public string SmallImage
        {
            get { return _smallImage; }
            set
            {
                if (value != _smallImage)
                {
                    _smallImage = value;
                    NotifyPropertyChanged("SmallImage");
                }
            }
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

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value.ToString();

            BitmapImage image = new BitmapImage(new Uri(fileName, UriKind.Relative));

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string dateString = "";
            try
            {
                DateTime date = (DateTime)value;
                dateString = String.Format("{0:M/d/yyyy h:mm tt}", date);
            }
            catch
            {
                //no-op
            }
            return dateString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}