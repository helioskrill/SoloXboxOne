


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Rss;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp;
using System.Linq;
using SOLOXBOXONE.Config;
using SOLOXBOXONE.ViewModels;

namespace SOLOXBOXONE.Sections
{
    public class ArticulosConfig : SectionConfigBase<RssSchema>
    {
		public override Func<Task<IEnumerable<RssSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new RssDataConfig
                {
                    Url = new Uri("http://soloxboxone.com/category/articulos/feed/")
                };

                return () => Singleton<RssDataProvider>.Instance.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<RssSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<RssSchema>
                {
                    Title = "Articulos",

					PageTitle = "Articulos",

                    ListNavigationInfo = NavigationInfo.FromPage("ArticulosListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
						viewModel.Header = item.Author.ToSafeString();
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());

						viewModel.GroupBy = item.Author.SafeType();

						viewModel.OrderBy = item.Author;
                    },
					OrderType = OrderType.Ascending,
                    DetailNavigation = (item) =>
                    {
                        return NavigationInfo.FromPage("ArticulosDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<RssSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, RssSchema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Author.ToSafeString();
                    viewModel.Title = item.Title.ToSafeString();
                    viewModel.Description = item.Content.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    viewModel.Content = null;
					viewModel.Source = item.FeedUrl;
                });

                var actions = new List<ActionConfig<RssSchema>>
                {
                    ActionConfig<RssSchema>.Link("Go To Source", (item) => item.FeedUrl.ToSafeString()),
                    ActionConfig<RssSchema>.AddToCalendar("Fecha", (item) => new Windows.ApplicationModel.Appointments.Appointment() {StartTime = item.PublishDate.SafeType() == DateTime.MinValue ? DateTime.Now : item.PublishDate.SafeType(), Subject = item.Title.ToSafeString(), AllDay = false }),
                };

                return new DetailPageConfig<RssSchema>
                {
                    Title = "Articulos",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
