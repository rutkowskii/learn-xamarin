using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Moq;
using Ninject;
using RestSharp.Portable;
using Xbehave;
using Xunit;

namespace Tests
{
    public class TestingContext : IInstaller, IDisposable
    {
        private readonly Mock<IFilePathProvider> _filePathProvider;
        public StandardKernel Kernel { get; }
        public Mock<IRestConnection> RestConnection { get; }
        public Mock<ISettingsRepo> SettingsRepo { get; }

        private const string LocalDbFile = "sqlite-base";

        public TestingContext()
        {
            Kernel = new StandardKernel();
            RestConnection = new Mock<IRestConnection>();
            SettingsRepo = new Mock<ISettingsRepo>();
            _filePathProvider = new Mock<IFilePathProvider>();
            _filePathProvider.SetupGet(p => p.Path).Returns(LocalDbFile);
            
            new BasicInstaller().RunInstallation(Kernel);
            RunInstallation(Kernel);
        }

        public void RunInstallation(IKernel kernel)
        {
            BindToMock(kernel, _filePathProvider);
            BindToMock(kernel, RestConnection);
            BindToMock(kernel, SettingsRepo);
        }

        private void BindToMock<T>(IKernel kernel, Mock<T> mock) where T : class
        {
            kernel.Rebind<T>().ToConstant(mock.Object);
        }

        public void Dispose()
        {
            File.Delete(LocalDbFile);
        }
    }

    public interface ITestSetup
    {
        void Setup(TestingContext testingContext);
    }

    class SetupUnavailableServer : ITestSetup
    {
        public void Setup(TestingContext testingContext)
        {
            testingContext.RestConnection
                .Setup(r => r.Post(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(TaskThrowing);
            testingContext.RestConnection
                .Setup(r => r.Get(It.IsAny<string>(), It.IsAny<IEnumerable<RequestParameter>>()))
                .Returns(TaskThrowing);
            testingContext.RestConnection
                .Setup(r => r.Get(It.IsAny<string>(), It.IsAny<CancellationTokenSource>()))
                .Returns(TaskThrowing);
            testingContext.RestConnection
                .Setup(r => r.Get(It.IsAny<string>(), It.IsAny<CancellationTokenSource>(), It.IsAny<IEnumerable<RequestParameter>>()))
                .Returns(TaskThrowing);
        }

        private Task<IRestResponse> TaskThrowing => new Task<IRestResponse>(
            () => { throw new InvalidOperationException(); });
    }

    class SetupLocalSettings : ITestSetup
    {
        public string Main { get; }
        public string Current { get; }

        public SetupLocalSettings(string main = "PLN", string current = "GBP")
        {
            Main = main;
            Current = current;
        }
        
        public void Setup(TestingContext testingContext)
        {
            testingContext.SettingsRepo.Setup(r => r.Get(SettingsKeys.CurrentCurrency)).Returns(Current);
            testingContext.SettingsRepo.Setup(r => r.Get(SettingsKeys.MainCurrency)).Returns(Main);
        }
    }

}