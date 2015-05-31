using MuseRT.Common;
using MuseRT.Data;
using MuseRT.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MuseRT.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = NewsModel;
        }

        private NewsViewModel _newsModel;
        public NewsViewModel NewsModel
        {
            get { return _newsModel ?? (_newsModel = new NewsViewModel()); }
        }

        private TourViewModel _tourModel;
        public TourViewModel TourModel
        {
            get { return _tourModel ?? (_tourModel = new TourViewModel()); }
        }

        private GalleryViewModel _galleryModel;
        public GalleryViewModel GalleryModel
        {
            get { return _galleryModel ?? (_galleryModel = new GalleryViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            NewsModel.ViewType = viewType;
            TourModel.ViewType = viewType;
            GalleryModel.ViewType = viewType;
        }
        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

            get { return Visibility.Visible; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadDataAsync(bool forceRefresh = false)
        {
            var loadTasks = new Task[]
            { 
                NewsModel.LoadItemsAsync(forceRefresh),
                TourModel.LoadItemsAsync(forceRefresh),
                GalleryModel.LoadItemsAsync(forceRefresh)
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await LoadDataAsync(true);
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutPage");
                });
            }
        }

        //public ICommand PrivacyCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(() =>
        //        {
        //            NavigationServices.NavigateTo(_privacyModel.Url);
        //        });
        //    }
        //}
    }
}
