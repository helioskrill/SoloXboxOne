


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Twitter;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp;
using System.Linq;
using SOLOXBOXONE.Config;
using SOLOXBOXONE.ViewModels;

namespace SOLOXBOXONE.Sections
{
    public class TwitterConfig : SectionConfigBase<TwitterSchema>
    {
		private readonly TwitterDataProvider _dataProvider = new TwitterDataProvider(new TwitterOAuthTokens
        {
			ConsumerKey = "bMU5raV0GvS8WOOKLC8Czyoq8",
                    ConsumerSecret = "t5LjQNG6ghk5GvM7xFLx0LoGzgxpX45ypDjTwIRmKh3piBg1vK",
                    AccessToken = "545942883-rZY2b3s5JETNC38Gahn5KB3l3x0OZkAVSCDW1onL",
                    AccessTokenSecret = "48AzybHA8RYzLvkMtLWO0XYaRJ6tDijk7H4hwkjRGE7PR"
        });

		public override Func<Task<IEnumerable<TwitterSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new TwitterDataConfig
                {
                    QueryType = TwitterQueryType.User,
                    Query = @"soloxboxone"
                };

                return () => _dataProvider.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<TwitterSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<TwitterSchema>
                {
                    Title = "Twitter",

					PageTitle = "Twitter",

                    ListNavigationInfo = NavigationInfo.FromPage("TwitterListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.UserName.ToSafeString();
                        viewModel.SubTitle = item.Text.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.UserProfileImageUrl.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
                        return new NavigationInfo
                        {
                            NavigationType = NavigationType.DeepLink,
                            TargetUri = new Uri(item.Url)
                        };
                    }
                };
            }
        }

        public override DetailPageConfig<TwitterSchema> DetailPage
        {
            get { return null; }
        }
    }
}
