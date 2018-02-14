using Xamarin.Forms;

namespace learn_xamarin.Storage
{
    class XamarinFilePathProvider : IFilePathProvider
    {
        private const string FileName = "ExpendituresDb";
        
        public string Path => DependencyService.Get<IFileHelper>().GetLocalFilePath(FileName);
    }
}