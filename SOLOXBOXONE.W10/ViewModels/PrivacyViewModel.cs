using System;
using AppStudio.Uwp;

namespace SOLOXBOXONE.ViewModels
{
    public class PrivacyViewModel : ObservableBase
    {
        public Uri Url
        {
            get
            {
                return new Uri(UrlText, UriKind.RelativeOrAbsolute);
            }
        }
        public string UrlText
        {
            get
            {
                return "http://soloxboxone.com/politica-de-privacidad/";
            }
        }
    }
}

