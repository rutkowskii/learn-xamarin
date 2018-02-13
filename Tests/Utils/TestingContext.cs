using System;
using learn_xamarin.DataServices;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Moq;
using Ninject;

namespace Tests.Utils
{
    public class TestingContext
    {
        private readonly Mock<IFilePathProvider> _filePathProvider;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        public StandardKernel Kernel { get; }
        public Mock<IRestConnection> RestConnection { get; }
        public Mock<ISettingsRepo> SettingsRepo { get; }
        public Mock<IExchangeRateDataService> ExchangeRateService { get; }

        public TestingContext()
        {
            Kernel = new StandardKernel();
            RestConnection = new Mock<IRestConnection>();
            SettingsRepo = new Mock<ISettingsRepo>();
            ExchangeRateService = new Mock<IExchangeRateDataService>();
            _filePathProvider = new Mock<IFilePathProvider>();
            _filePathProvider.SetupGet(p => p.Path).Returns(":memory:");
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            
            new BasicInstaller().RunInstallation(Kernel);
            RunInstallation(Kernel);
        }

        public void SetupTime(DateTime dt)
        {
            _dateTimeProvider.SetupGet(p => p.Now).Returns(dt);
        }

        public void RunSetups(params ITestSetup[] setups)
        {
            setups.Foreach(x => x.Setup(this));
        }

        private void RunInstallation(IKernel kernel)
        {
            BindToMock(kernel, _filePathProvider);
            BindToMock(kernel, RestConnection);
            BindToMock(kernel, SettingsRepo);
            BindToMock(kernel, ExchangeRateService);
            BindToMock(kernel, _dateTimeProvider);
            BindToMock<INavigationService>(kernel);
        }

        private void BindToMock<T>(IKernel kernel, Mock<T> mock) where T : class
        {
            kernel.Rebind<T>().ToConstant(mock.Object);
        }
        
        private void BindToMock<T>(IKernel kernel) where T : class
        {
            kernel.Rebind<T>().ToConstant(new Mock<T>().Object);
        }
    }
}
