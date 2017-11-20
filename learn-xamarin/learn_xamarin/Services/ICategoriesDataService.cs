using System.Threading.Tasks;
using learn_xamarin.Model;

namespace learn_xamarin.Services
{
    public interface ICategoriesDataService
    {
        Task<Category[]> GetAll();
    }
}