using learn_xamarin.Model;
using Nancy;

namespace learn_xamarin.Sever
{
    public class CategoriesModule : NancyModule
    {
        private readonly ServerRepo _serverRepo;

        public CategoriesModule(ServerRepo serverRepo)
        {
            _serverRepo = serverRepo;
            SetupRoutes();
        }

        private void SetupRoutes()
        {
            Get[$"/{RestCallsConstants.Category}"] = _ => GetAllCategories();
        }

        private object GetAllCategories()
        {
            var res = _serverRepo.GetAllCategories();
            Logger.Debug($"Client asked about categories, returning {res.Length} items");
            return res;
        }
    }
}