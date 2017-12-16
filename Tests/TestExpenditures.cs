using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using learn_xamarin.DataServices;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Ninject;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class TestExpenditures
    {
        public TestExpenditures()
        {
            _fixture = new Fixture();
        }

        private readonly Fixture _fixture;
        private SetupLocalSettings _setupLocalSettings;
        private TestingContext _tc;
        private Category _category;
        private decimal _sum;

        [SetUp]
        public void Setup()
        {
            _tc = new TestingContext();
            _setupLocalSettings = new SetupLocalSettings();
            _tc.RunSetups(_setupLocalSettings);
            _category = _fixture.Create<Category>();
            _sum = _fixture.Create<decimal>();
        }
        
        [Test]
        public void Expenditures_are_shown_in_the_statements_when_server_is_DOWN()
        {
            _tc.RunSetups(new SetupUnavailableServer());

            InsertSampleExpenditure();

            AssertTopStatementElementValues();
        }
        
        [Test]
        public void Expenditures_are_shown_in_the_statements_starting_with_the_latest()
        {
            var category1 = _fixture.Create<Category>();
            var category2 = _fixture.Create<Category>();
            var sum1 = _fixture.Create<decimal>();
            var sum2 = _fixture.Create<decimal>();
            
            _tc.RunSetups(new SetupStubServer());

            _tc.SetupTime(DateTime.Today.AddHours(8).AddMinutes(20));
            new InsertExpenditureAction(category1, sum1).Run(_tc);
            _tc.SetupTime(DateTime.Today.AddHours(8).AddMinutes(35));
            new InsertExpenditureAction(category2, sum2).Run(_tc);

            var actualStementElements = _tc.Kernel.Get<StatementViewModel>().StatementElements.ToArray();
            CollectionAssert.AreEqual(new[]{sum2, sum1}, actualStementElements.Select(x => x.Sum).ToArray());
            CollectionAssert.AreEqual(new[]{category2.Id, category1.Id}, actualStementElements.Select(x => x.CategoryId).ToArray());
        }

        [Test]
        public void Expenditures_are_saved_as_unsynchronized_when_the_server_is_DOWN()
        {
            _tc.RunSetups(new SetupUnavailableServer());

            InsertSampleExpenditure();

            var unsynchronizedElementId = _tc.Kernel.Get<ILocalDatabase>()
                .GetAllUnsynchronizedItems()
                .Single()
                .Id;
            
            Assert.AreEqual(TopStatementElement.Id, unsynchronizedElementId);
        }

        [Test]
        public void Unsynchronized_expenditures_are_passed_to_the_server_when_it_is_UP()
        {
            _tc.RunSetups(new SetupUnavailableServer());
            
            InsertSampleExpenditure();
            
            var stubServer = new SetupStubServer();
            _tc.RunSetups(stubServer);
            _tc.Kernel.Get<IExpendituresDataService>().TrySynchronize(_ => { });
            
            var actual = (stubServer.LastData as Expenditure[]).Single();
            Assert.AreEqual(_category.Id, actual.CategoryId);
            Assert.AreEqual(_sum, actual.Sum);
            Assert.AreEqual(_setupLocalSettings.Current, actual.CurrencyCode);
            
            Assert.AreEqual(0, _tc.Kernel.Get<ILocalDatabase>().GetAllUnsynchronizedItems().Length);
        }
        
        private void AssertTopStatementElementValues()
        {
            var actual = TopStatementElement;
            Assert.AreEqual(_category.Id, actual.CategoryId);
            Assert.AreEqual(_sum, actual.Sum);
            Assert.AreEqual(_setupLocalSettings.Current, actual.CurrencyCode);
        }
        
        // todo piotr now the synchronization. think of WHEN to trigger the sync.
        /// todo piotr timezones when displaying the statement?
        // once it works, we done with it!

        private Expenditure TopStatementElement => _tc.Kernel.Get<StatementViewModel>().StatementElements.First();

        private void InsertSampleExpenditure()
        {
            new InsertExpenditureAction(_category, _sum).Run(_tc);
        }
    }
}