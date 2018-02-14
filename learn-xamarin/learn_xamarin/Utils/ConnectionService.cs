using Plugin.Connectivity;

namespace learn_xamarin.Utils
{
    class ConnectionService : IConnectionService
    {
        public bool IsConnected => CrossConnectivity.Current.IsConnected;
    }
}
