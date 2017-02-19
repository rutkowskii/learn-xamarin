using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Vm;

namespace learn_xamarin.Services
{
    public interface ICategoriesDataService
    {
        Category[] GetAll(); //todo tmp, we need generic async approach. 
    }
}