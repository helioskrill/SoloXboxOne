


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.YouTube;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp;
using System.Linq;
using SOLOXBOXONE.Config;
using SOLOXBOXONE.ViewModels;

namespace SOLOXBOXONE.Sections
{
    public class YouTubeConfig : SectionConfigBase<YouTubeSchema>
    {
	    private readonly YouTubeDataProvider _dataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens
        {
			ApiKey = "AIzaSyDYy2iuOq9CLXFA1-kzNElZX0FoanxAkkY"
        });

		public override Func<Task<IEnumerable<YouTubeSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new YouTubeDataConfig
                {
                    QueryType = YouTubeQueryType.Videos,
                    Query = @"soloxboxone"
                };

                return () => _dataProvider.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<YouTubeSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<YouTubeSchema>
                {
                    Title = "YouTube",

					PageTitle = "YouTube",

                    ListNavigationInfo = NavigationInfo.FromPage("YouTubeListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
                        return NavigationInfo.FromPage("YouTubeDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<YouTubeSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, YouTubeSchema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Title.ToSafeString();
                    viewModel.Title = null;
                    viewModel.Description = item.Summary.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(null);
                    viewModel.Content = item.EmbedHtmlFragment;
					viewModel.Source = item.ExternalUrl;
                });

                var actions = new List<ActionConfig<YouTubeSchema>>
                {
                    ActionConfig<YouTubeSchema>.Link("Go To Source", (item) => item.ExternalUrl.ToSafeString()),
                };

                return new DetailPageConfig<YouTubeSchema>
                {
                    Title = "YouTube",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
