using MuseRT.Common;
using MuseRT.Data;
using MuseRT.Data.Common;
using MuseRT.Data.DataSources;
using MuseRT.Services;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MuseRT.ViewModels
{
    public class TourViewModel : ViewModelBase<RssSchema>
    {
        private RelayCommandEx<RssSchema> itemClickCommand;
        public RelayCommandEx<RssSchema> ItemClickCommand
        {
            get
            {
                if (itemClickCommand == null)
                {
                    itemClickCommand = new RelayCommandEx<RssSchema>(
                        (item) =>
                        {
                            NavigationServices.NavigateToPage("TourDetailPage", item);
                        });
                }

                return itemClickCommand;
            }
        }

        private RelayCommandEx<string> changeFontSizeCommand;
        public RelayCommandEx<string> ChangeFontSizeCommand
        {
            get
            {
                if (changeFontSizeCommand == null)
                {
                    changeFontSizeCommand = new RelayCommandEx<string>((s) =>
                    {
                        FontSizes fontSize;
                        Enum.TryParse<FontSizes>(s, out fontSize);
                        DisplayFontSize = fontSize;
                    });
                }

                return changeFontSizeCommand;
            }
        }

        public FontSizes DisplayFontSize
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LocalSettingNames.TextViewerFontSizeSetting))
                {
                    FontSizes fontSizes;
                    Enum.TryParse<FontSizes>(ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSizeSetting].ToString(), out fontSizes);
                    return fontSizes;
                }
                return FontSizes.Normal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSizeSetting] = value.ToString();
                this.OnPropertyChanged("DisplayFontSize");
            }
        }

        override protected DataSourceBase<RssSchema> CreateDataSource()
        {
            return new TourDataSource(); // RssDataSource
        }

        override public Visibility TextToSpeechVisibility
        {
            get { return ViewType == ViewTypes.Detail ? Visibility.Visible : Visibility.Collapsed; }
        }

        override protected async void TextToSpeech()
        {
            await base.SpeakText("ID");
        }

        override public Visibility PinToStartVisibility
        {
            get { return ViewType == ViewTypes.Detail ? Visibility.Visible : Visibility.Collapsed; }
        }

        override protected void PinToStart()
        {
            base.PinToStart("TourDetailPage", "{Title}", "{Summary}", "{ImageUrl}");
        }

        override public Visibility ShareItemVisibility
        {
            get { return ViewType == ViewTypes.Detail ? Visibility.Visible : Visibility.Collapsed; }
        }

        override public Visibility GoToSourceVisibility
        {
            get { return ViewType == ViewTypes.Detail ? Visibility.Visible : Visibility.Collapsed; }
        }

        override protected void GoToSource()
        {
            base.GoToSource("{FeedUrl}");
        }

        override public Visibility RefreshVisibility
        {
            get { return ViewType == ViewTypes.List ? Visibility.Visible : Visibility.Collapsed; }
        }

        public RelayCommandEx<Slider> IncreaseSlider
        {
            get
            {
                return new RelayCommandEx<Slider>(s => s.Value++);
            }
        }

        public RelayCommandEx<Slider> DecreaseSlider
        {
            get
            {
                return new RelayCommandEx<Slider>(s => s.Value--);
            }
        }

        override public void NavigateToSectionList()
        {
            NavigationServices.NavigateToPage("TourList");
        }

        override protected void NavigateToSelectedItem()
        {
            NavigationServices.NavigateToPage("TourDetailPage");
        }

        public override async System.Threading.Tasks.Task LoadItemsAsync(bool forceRefresh = false)
        {
            ProgressBarVisibility = Visibility.Visible;

            var data = new ObservableCollection<RssSchema>();
            var timeStamp = await DataSource.LoadDataAsync(data, forceRefresh);

            _items = new ObservableCollection<RssSchema>(from i in data orderby i.TourDate select i);

            OnPropertyChanged("Items");
            OnPropertyChanged("PreviewItems");
            OnPropertyChanged("HasMoreItems");

            ProgressBarVisibility = Visibility.Collapsed;
        }
    }
}
