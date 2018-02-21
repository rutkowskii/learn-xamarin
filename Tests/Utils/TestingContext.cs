using System;
using System.Collections.Generic;
using learn_xamarin.AppSettings;
using learn_xamarin.DataServices;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Storage;
using learn_xamarin.UiUtils;
using learn_xamarin.Utils;
using Moq;
using Ninject;

namespace Tests.Utils
{
    public class TestingContext
    {
        private readonly Mock<IFilePathProvider> _filePathProvider;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly Dictionary<string, object> _settings;
        private readonly Mock<IAppSettingsDictionaryProvider> _appSettingsDictionaryProvider;

        public StandardKernel Kernel { get; }
        public Mock<IRestConnection> RestConnection { get; }
        public Mock<IExchangeRateDataService> ExchangeRateService { get; }
        public Mock<IDialogService> DialogService { get; }

        public TestingContext()
        {
            Kernel = new StandardKernel();
            RestConnection = new Mock<IRestConnection>();

            _settings = new Dictionary<string, object>();
            _appSettingsDictionaryProvider = new Mock<IAppSettingsDictionaryProvider>();
            _appSettingsDictionaryProvider.Setup(p => p.Settings).Returns(_settings);

            ExchangeRateService = new Mock<IExchangeRateDataService>();
            ExchangeRateService.Setup(s => s.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(1);

            DialogService = new Mock<IDialogService>();

            _filePathProvider = new Mock<IFilePathProvider>();
            _filePathProvider.SetupGet(p => p.Path).Returns(":memory:");

            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.SetupGet(p => p.Now).Returns(new DateTime(2017, 1, 23, 14, 20, 0));

            new BasicInstaller().RunInstallation(Kernel);
            RunInstallation(Kernel);
        }

        public void Run(params ITestSetup[] setups)
        {
            setups.Foreach(x => x.Setup(this));
        }

        public DateTime Now
        {
            get => Kernel.Get<IDateTimeProvider>().Now;
            set => _dateTimeProvider.Setup(p => p.Now).Returns(value);
        }

        private void RunInstallation(IKernel kernel)
        {
            BindToMock(kernel, _filePathProvider);
            BindToMock(kernel, RestConnection);
            BindToMock(kernel, _appSettingsDictionaryProvider);
            BindToMock(kernel, ExchangeRateService);
            BindToMock(kernel, _dateTimeProvider);
            BindToMock(kernel, DialogService);
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