//---------------------------------------------------------------------------
//
// <copyright file="YouTubeListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>3/16/2016 7:17:39 PM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.YouTube;
using SOLOXBOXONE.Sections;
using SOLOXBOXONE.ViewModels;
using AppStudio.Uwp;

namespace SOLOXBOXONE.Pages
{
    public sealed partial class YouTubeListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public YouTubeListPage()
        {
			this.ViewModel = ListViewModel.CreateNew(Singleton<YouTubeConfig>.Instance);

            this.InitializeComponent();

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel.LoadDataAsync();
            base.OnNavigatedTo(e);
        }

    }
}
