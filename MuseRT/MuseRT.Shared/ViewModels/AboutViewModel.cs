using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;

namespace MuseRT.ViewModels
{
    public class AboutViewModel
    {
        public string Publisher
        {
            get
            {
                return "fmendo";
            }
        }

        public string AppVersion
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
            }
        }

        public string AboutText
        {
            get
            {
                return "Keep up with everything MUSE!";
            }
        }
    }
}
