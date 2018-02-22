using learn_xamarin.DataServices;
using Ninject;

namespace Tests.Utils
{
    public class SynchronizationAction : ITestSetup
    {
        public void Setup(TestingContext testingContext)
        {
            testingContext.Kernel.Get<Synchronizer>().Synchronize();
        }
    }
}