using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using learn_xamarin.Cache;
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

        [SetUp]
        public void Setup()
        {
            _tc = new TestingContext();
            _setupLocalSettings = new SetupLocalSettings();
            _tc.Run(_setupLocalSettings);
        }

        [Test]
        public void Expenditures_are_shown_in_the_statements_when_server_is_DOWN()
        {
            var insertExpenditureAction = new InsertExpenditureAction();

            _tc.Run(new SetupUnavailableServer(), insertExpenditureAction);

            AssertTopStatementElement(insertExpenditureAction);
        }

        [Test]
        public void Expenditures_are_shown_in_the_statements_starting_with_the_latest()
        {
            _tc.Run(new SetupExpendituresServer());
            var expenditure1 = new InsertExpenditureAction();
            var expenditure2 = new InsertExpenditureAction();
            var expenditure3 = new InsertExpenditureAction();
            var expenditureActionsByLatest = new[] {expenditure3, expenditure2, expenditure1};

            _tc.Run(
                expenditure1,
                new AdvanceTimeAction(),
                expenditure2,
                new AdvanceTimeAction(),
                expenditure3);


            var expectedSums = expenditureActionsByLatest.Select(x => x.Sum).ToArray();
            var expectedCategories = expenditureActionsByLatest.Select(x => x.CategoryId).ToArray();

            var actualStementElements = _tc.Kernel.Get<StatementViewModel>().StatementElements.ToArray();
            CollectionAssert.AreEqual(expectedSums, actualStementElements.Select(x => x.Sum).ToArray());
            CollectionAssert.AreEqual(expectedCategories, actualStementElements.Select(x => x.CategoryId).ToArray());
        }

        [Test]
        public void Expenditures_are_saved_as_unsynchronized_when_the_server_is_DOWN()
        {
            _tc.Run(
                new SetupUnavailableServer(),
                new InsertExpenditureAction(),
                new InsertExpenditureAction(),
                new InsertExpenditureAction()
            );

            var unsynchronizedElementId =
                _tc.Kernel.Get<ILocalDatabase>().GetAllUnsynchronizedItems().Select(x => x.Id).ToArray();
            var statementElementIds =
                _tc.Kernel.Get<StatementViewModel>().StatementElements.Select(x => x.Id).ToArray();
            CollectionAssert.AreEquivalent(statementElementIds, unsynchronizedElementId);
        }

        [Test]
        public void Unsynchronized_expenditures_are_passed_to_the_server_when_it_is_UP()
        {
            var stubServer = new SetupExpendituresServer();
            var insertExpenditureAction = new InsertExpenditureAction();

            _tc.Run(
                new SetupUnavailableServer(),
                insertExpenditureAction,
                stubServer,
                new SynchronizationAction()
            );

            AssertExpenditureWasPassedToServer(insertExpenditureAction, stubServer);
            AssertThereAreNoUnsynchronizedItems();
            AssertExpenditureMatch(insertExpenditureAction, stubServer.ServerExpenditures.Single());
        }


        [Test]
        public void Server_expenditures_are_downloaded_when_available()
        {
            var initialServerExpenditures = _fixture.CreateMany<Expenditure>();
            _tc.Run(
                new SetupExpendituresServer().WithExpenditures(initialServerExpenditures),
                new SynchronizationAction());

            AssertExpendituresAreStoredLocally(initialServerExpenditures);
            AssertExpendituresAreStoredInCache(initialServerExpenditures);
        }

        [Test]
        public void When_server_and_local_state_differ_successful_synchronization_updates_both_of_them()
        {
            _tc.Run(new SetupUnavailableServer());
            var initialLocalExpenditures = InsertSampleExpenditures();

            var initialServerExpenditures = _fixture.CreateMany<Expenditure>();
            var setupStubServer = new SetupExpendituresServer().WithExpenditures(initialServerExpenditures);

            _tc.Run(
                setupStubServer, 
                new SynchronizationAction());


            var expectedAll = initialLocalExpenditures.Concat(initialServerExpenditures).ToArray();

            AssertExpendituresAreStoredLocally(expectedAll);
            AssertExpendituresAreStoredInCache(expectedAll);
            AssertExpendituresAreStoredInServer(expectedAll, setupStubServer);
        }

        private void AssertExpendituresAreStoredInServer(IEnumerable<Expenditure> initialServerExpenditures, SetupExpendituresServer server)
        {
            var expected = initialServerExpenditures.Select(e => e.Id).ToArray();
            var actualServerExpendituresIds = server.ServerExpenditures.Select(e => e.Id).ToArray();
            CollectionAssert.AreEquivalent(expected, actualServerExpendituresIds);
        }

        private void AssertExpendituresAreStoredInCache(IEnumerable<Expenditure> initialServerExpenditures)
        {
            var expected = initialServerExpenditures.Select(e => e.Id).ToArray();
            var actualCacheExpendituresIds =
                _tc.Kernel.Get<IExpendituresCache>().All().Select(e => e.Id).ToArray();
            CollectionAssert.AreEquivalent(expected, actualCacheExpendituresIds);
        }

        private void AssertExpendituresAreStoredLocally(IEnumerable<Expenditure> initialServerExpenditures)
        {
            var expected = initialServerExpenditures.Select(e => e.Id).ToArray();
            var actualLocalDbExpendituresIds =
                _tc.Kernel.Get<ILocalDatabase>().GetAllExpenditures().Select(e => e.Id).ToArray();
            CollectionAssert.AreEquivalent(expected, actualLocalDbExpendituresIds);
        }

        private void AssertExpenditureMatch(InsertExpenditureAction action, Expenditure actualExpenditure)
        {
            Assert.AreEqual(action.CategoryId, actualExpenditure.CategoryId);
            Assert.AreEqual(action.Sum, actualExpenditure.Sum);
        }

        private void AssertThereAreNoUnsynchronizedItems()
        {
            Assert.AreEqual(0, _tc.Kernel.Get<ILocalDatabase>().GetAllUnsynchronizedItems().Length);
        }

        private void AssertExpenditureWasPassedToServer(
            InsertExpenditureAction insertExpenditureAction, SetupExpendituresServer expendituresServer)
        {
            var actual = (expendituresServer.LastData as Expenditure[]).Single();
            Assert.AreEqual(insertExpenditureAction.CategoryId, actual.CategoryId);
            Assert.AreEqual(insertExpenditureAction.Sum, actual.Sum);
            Assert.AreEqual(_setupLocalSettings.CurrentCurrency, actual.CurrencyCode);
        }

        private void AssertTopStatementElement(InsertExpenditureAction insertExpenditureAction)
        {
            var actual = TopStatementElement;
            Assert.AreEqual(insertExpenditureAction.CategoryId, actual.CategoryId);
            Assert.AreEqual(insertExpenditureAction.Sum, actual.Sum);
            Assert.AreEqual(_setupLocalSettings.CurrentCurrency, actual.CurrencyCode);
        }

        /// todo piotr timezones when displaying the statement?

        private Expenditure TopStatementElement => _tc.Kernel.Get<StatementViewModel>().StatementElements.First();

        private Expenditure[] InsertSampleExpenditures()
        {
            for (var i = 0; i < 3; i++) new InsertExpenditureAction().Setup(_tc);
            return _tc.Kernel.Get<ILocalDatabase>().GetAllExpenditures();
        }
    }
}