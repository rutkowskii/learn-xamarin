using System;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
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

        public TestingContext()
        {
            Kernel = new StandardKernel();
            RestConnection = new Mock<IRestConnection>();
            SettingsRepo = new Mock<ISettingsRepo>();
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

    public class InsertExpenditureAction
    {
        private Category _category;
        private decimal _sum;

        public InsertExpenditureAction(Category category, decimal sum)
        {
            _category = category;
            _sum = sum;
        }

        public void Run(TestingContext tc)
        {
            tc.Kernel.Get<MoneySpentDialogViewModel>().CategorySelected = _category;
            tc.Kernel.Get<MoneySpentDialogViewModel>().Sum = _sum;
            tc.Kernel.Get<MoneySpentSumViewModel>().ConfirmationCommand.Execute(null);
        }
    }
}