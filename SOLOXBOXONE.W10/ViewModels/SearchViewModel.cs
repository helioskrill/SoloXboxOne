using System;
using System.Collections.Generic;
using AppStudio.Uwp;
using AppStudio.Uwp.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SOLOXBOXONE.Sections;
namespace SOLOXBOXONE.ViewModels
{
    public class SearchViewModel : ObservableBase
    {
        public SearchViewModel() : base()
        {
			PageTitle = "SOLOXBOXONE";
            Articulos = ListViewModel.CreateNew(Singleton<ArticulosConfig>.Instance);
            Reviews = ListViewModel.CreateNew(Singleton<ReviewsConfig>.Instance);
            YouTube = ListViewModel.CreateNew(Singleton<YouTubeConfig>.Instance);
            Windows10 = ListViewModel.CreateNew(Singleton<Windows10Config>.Instance);
            Noticias = ListViewModel.CreateNew(Singleton<NoticiasConfig>.Instance);
            Podcasts = ListViewModel.CreateNew(Singleton<PodcastsConfig>.Instance);
            Foro20 = ListViewModel.CreateNew(Singleton<Foro20Config>.Instance);
            Twitter = ListViewModel.CreateNew(Singleton<TwitterConfig>.Instance);
                        
        }

        private string _searchText;
        private bool _hasItems = true;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetProperty(ref _hasItems, value); }
        }

		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(
                async (text) =>
                {
                    await SearchDataAsync(text);
                }, SearchViewModel.CanSearch);
            }
        }      
        public ListViewModel Articulos { get; private set; }
        public ListViewModel Reviews { get; private set; }
        public ListViewModel YouTube { get; private set; }
        public ListViewModel Windows10 { get; private set; }
        public ListViewModel Noticias { get; private set; }
        public ListViewModel Podcasts { get; private set; }
        public ListViewModel Foro20 { get; private set; }
        public ListViewModel Twitter { get; private set; }
        
		public string PageTitle { get; set; }
        public async Task SearchDataAsync(string text)
        {
            this.HasItems = true;
            SearchText = text;
            var loadDataTasks = GetViewModels()
                                    .Select(vm => vm.SearchDataAsync(text));

            await Task.WhenAll(loadDataTasks);
			this.HasItems = GetViewModels().Any(vm => vm.HasItems);
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
        }
		private void CleanItems()
        {
            foreach (var vm in GetViewModels())
            {
                vm.CleanItems();
            }
        }
		public static bool CanSearch(string text) { return !string.IsNullOrWhiteSpace(text) && text.Length >= 3; }
    }
}
