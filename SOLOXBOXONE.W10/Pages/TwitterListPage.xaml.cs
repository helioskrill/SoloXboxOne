//---------------------------------------------------------------------------
//
// <copyright file="TwitterListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>3/16/2016 7:17:39 PM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.Twitter;
using SOLOXBOXONE.Sections;
using SOLOXBOXONE.ViewModels;
using AppStudio.Uwp;

namespace SOLOXBOXONE.Pages
{
    public sealed partial class TwitterListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public TwitterListPage()
        {
			this.ViewModel = ListViewModel.CreateNew(Singleton<TwitterConfig>.Instance);

            this.InitializeComponent();

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel.LoadDataAsync();
            base.OnNavigatedTo(e);
        }

    }
}
