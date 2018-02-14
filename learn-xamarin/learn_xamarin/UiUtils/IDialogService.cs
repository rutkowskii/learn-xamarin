using System.Threading.Tasks;
using Xamarin.Forms;

namespace learn_xamarin.UiUtils
{
    public interface IDialogService
    {
        Task<string> DisplayActionSheet(Page page, string title, params string[] buttons);
    }

    class DialogService : IDialogService
    {
        public Task<string> DisplayActionSheet(Page page, string title, params string[] buttons)
        {
            return page.DisplayActionSheet(title, "Cancel", null, buttons);
        }
    }
}