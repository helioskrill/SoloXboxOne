using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.YouTube;
using AppStudio.DataProviders.Twitter;
using AppStudio.DataProviders.Menu;
using AppStudio.DataProviders.LocalStorage;
using SOLOXBOXONE.Sections;


namespace SOLOXBOXONE.ViewModels
{
    public class MainViewModel : ObservableBase
    {
        public MainViewModel(int visibleItems) : base()
        {
            PageTitle = "SOLOXBOXONE";
            Articulos = ListViewModel.CreateNew(Singleton<ArticulosConfig>.Instance, visibleItems);
            Reviews = ListViewModel.CreateNew(Singleton<ReviewsConfig>.Instance, visibleItems);
            YouTube = ListViewModel.CreateNew(Singleton<YouTubeConfig>.Instance, visibleItems);
            Windows10 = ListViewModel.CreateNew(Singleton<Windows10Config>.Instance, visibleItems);
            Noticias = ListViewModel.CreateNew(Singleton<NoticiasConfig>.Instance, visibleItems);
            Podcasts = ListViewModel.CreateNew(Singleton<PodcastsConfig>.Instance, visibleItems);
            Foro20 = ListViewModel.CreateNew(Singleton<Foro20Config>.Instance, visibleItems);
            Twitter = ListViewModel.CreateNew(Singleton<TwitterConfig>.Instance, visibleItems);
            Info = ListViewModel.CreateNew(Singleton<InfoConfig>.Instance);

            Actions = new List<ActionInfo>();

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = new RelayCommand(Refresh),
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

        public string PageTitle { get; set; }
        public ListViewModel Articulos { get; private set; }
        public ListViewModel Reviews { get; private set; }
        public ListViewModel YouTube { get; private set; }
        public ListViewModel Windows10 { get; private set; }
        public ListViewModel Noticias { get; private set; }
        public ListViewModel Podcasts { get; private set; }
        public ListViewModel Foro20 { get; private set; }
        public ListViewModel Twitter { get; private set; }
        public ListViewModel Info { get; private set; }

        public RelayCommand<INavigable> SectionHeaderClickCommand
        {
            get
            {
                return new RelayCommand<INavigable>(item =>
                    {
                        NavigationService.NavigateTo(item);
                    });
            }
        }
		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(
                (text) =>
                {
                    NavigationService.NavigateToPage("SearchPage", text);
                }, SearchViewModel.CanSearch);
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return GetViewModels().Select(vm => vm.LastUpdated)
                            .OrderByDescending(d => d).FirstOrDefault();
            }
        }

        public List<ActionInfo> Actions { get; private set; }

        public bool HasActions
        {
            get
            {
                return Actions != null && Actions.Count > 0;
            }
        }

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private async void Refresh()
        {
            var refreshDataTasks = GetViewModels()
                                        .Where(vm => !vm.HasLocalData).Select(vm => vm.LoadDataAsync(true));

            await Task.WhenAll(refreshDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return Articulos;
            yield return Reviews;
            yield return YouTube;
            yield return Windows10;
            yield return Noticias;
            yield return Podcasts;
            yield return Foro20;
            yield return Twitter;
            yield return Info;
        }
    }
}
