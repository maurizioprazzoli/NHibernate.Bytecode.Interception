using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using NHibernate;
using NHibernateSimpleProfiler;
using System;
using System.Collections.Generic;

namespace TestLeazyLoading
{
    public abstract class BaseTest
    {
        // Define unity repository
        protected ISessionFactory sessionFactory;

        public BaseTest()
        {
            // RegisterUnitOfwork
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            ComposeConfiguration(configuration);
            sessionFactory = NHConfiguration.ConfigureNHibernate(configuration);
        }

        [TestMethod]
        public void TestLeazyLoading_SaveEntityMustNotCallBytecodeProviderInterceptor()
        {
            // Generate Guid
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            // Create item and bids
            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";
            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            // Reset counter
            Profiler.TakeSnapshot();
            CustomBytecodeProviderInterceptorCounter.ResetCounter();
            EntityCreationCounterHelper.ResetCounter();

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    using (var sqlLogSpy = Profiler.LogSpy)
                    {
                        session.Save(item);
                        tx.Commit();
                    }
                }
            }

            var statics = Profiler.GetDifferenceFromLastSnapshot();

            // No call to CustomBytecodeProviderInterceptor
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 0);
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.ProxyTypeCreated == 0);

            // Test All Call has been Intercepted
            Assert.IsTrue(testAllCallIntercepted());
        }

        [TestMethod]
        public void TestLeazyLoading_SaveEntityMustPerformSQLInsertStatement()
        {
            // Generate Guid
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            // Create item and bids
            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";
            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            // Reset counter
            Profiler.TakeSnapshot();
            CustomBytecodeProviderInterceptorCounter.ResetCounter();
            EntityCreationCounterHelper.ResetCounter();

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    using (var sqlLogSpy = Profiler.LogSpy)
                    {
                        session.Save(item);
                        tx.Commit();
                    }
                }
            }

            var statics = Profiler.GetDifferenceFromLastSnapshot();

            // We expect two query one for insert Item and one for insert Bids
            Assert.IsTrue(statics.NumberOfSQLSelectStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLInsertStatement == 2);
            Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

            // Test All Call has been Intercepted
            Assert.IsTrue(testAllCallIntercepted());
        }

        [TestMethod]
        public void TestLeazyLoading_SaveEntityAndGetEntityMustCallCreateProxyInstanceForLazyClass()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            // Reset counter
            Profiler.TakeSnapshot();
            CustomBytecodeProviderInterceptorCounter.ResetCounter();
            EntityCreationCounterHelper.ResetCounter();

            Item itemRetrieved;
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    using (var sqlLogSpy = Profiler.LogSpy)
                    {
                        itemRetrieved = session.Get<Item>(item_guid);
                        tx.Commit();
                    }
                }
            }

            var statics = Profiler.GetDifferenceFromLastSnapshot();

            // Item is called be proxyed
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 1);
            // Item must be proxyed
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 1);

            // Test All Call has been Intercepted
            Assert.IsTrue(testAllCallIntercepted());
        }

        [TestMethod]
        public void TestLeazyLoading_SaveEntityAndGetEntityMustPerformSQLQueryForLazyClass()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            // Reset counter
            Profiler.TakeSnapshot();
            CustomBytecodeProviderInterceptorCounter.ResetCounter();
            EntityCreationCounterHelper.ResetCounter();

            Item itemRetrieved;
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    using (var sqlLogSpy = Profiler.LogSpy)
                    {
                        itemRetrieved = session.Get<Item>(item_guid);
                        tx.Commit();
                    }
                }
            }

            var statics = Profiler.GetDifferenceFromLastSnapshot();

            // Since Bid is Lazy we expect only one query for fetch Item
            Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
            Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

            // Test All Call has been Intercepted
            Assert.IsTrue(testAllCallIntercepted());

        }

        [TestMethod]
        public virtual void TestLeazyLoading_SaveEntityAndGetEntityClosedSessionExceptionForLeazyLoading()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            Item itemRetrieved;
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    itemRetrieved = session.Get<Item>(item_guid);
                    tx.Commit();
                }
            }

            Int32 numberOfLazyInitializationException = 0;
            try
            {
                Assert.IsTrue(itemRetrieved.Id == item_guid);
            }
            catch (LazyInitializationException ex)
            {
                numberOfLazyInitializationException++;
            }
            catch
            { }

            // itemRetrieved.Description - Lazy Property
            try
            {
                Assert.IsTrue(itemRetrieved.Description == "Item Description");
            }
            catch (LazyInitializationException ex)
            {
                numberOfLazyInitializationException++;
            }
            catch
            { }

            // itemRetrieved.Bids.Description - Lazy Collection
            try
            {
                foreach (Bid bid in itemRetrieved.Bids)
                {
                    Assert.IsTrue(bid.Id != default(Guid));
                    Assert.IsTrue(bid.Description != String.Empty);

                }

            }
            catch (LazyInitializationException ex)
            {
                numberOfLazyInitializationException++;
            }
            catch
            { }

            Assert.IsTrue(numberOfLazyInitializationException == 2);
        }

        [TestMethod]
        public virtual void TestLeazyLoading_SaveEntityAndGetEntityKeepSessionOpenNoExceptionForLeazyLoadingField()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            Item itemRetrieved;
            Int32 numberOfLazyInitializationException = 0;

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    itemRetrieved = session.Get<Item>(item_guid);
                    tx.Commit();
                }

                using (var sqlLogSpy = Profiler.LogSpy)
                {
                    try
                    {
                        Assert.IsTrue(itemRetrieved.Id == item_guid);
                    }
                    catch (LazyInitializationException ex)
                    {
                        numberOfLazyInitializationException++;
                    }
                    catch
                    { }
                }

                // itemRetrieved.Description - Lazy Property
                try
                {
                    Assert.IsTrue(itemRetrieved.Description == "Item Description");
                }
                catch (LazyInitializationException ex)
                {
                    numberOfLazyInitializationException++;
                }
                catch
                { }

                // itemRetrieved.Bids.Description - Lazy Collection
                try
                {
                    foreach (Bid bid in itemRetrieved.Bids)
                    {
                        Assert.IsTrue(bid.Id != default(Guid));
                        Assert.IsTrue(bid.Description != String.Empty);
                    }
                }
                catch (LazyInitializationException ex)
                {
                    numberOfLazyInitializationException++;
                }
                catch
                { }

            }
            Assert.IsTrue(numberOfLazyInitializationException == 0);
        }

        [TestMethod]
        public virtual void TestLeazyLoading_SaveEntityAndGetEntityKeepSessionOpenMustPerformSQLQueryForLazyProperty()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            Item itemRetrieved;
            Int32 numberOfLazyInitializationException = 0;

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    itemRetrieved = session.Get<Item>(item_guid);
                    tx.Commit();
                }

                // Reset counter
                Profiler.TakeSnapshot();
                CustomBytecodeProviderInterceptorCounter.ResetCounter();
                EntityCreationCounterHelper.ResetCounter();

                using (var sqlLogSpy = Profiler.LogSpy)
                {
                    try
                    {
                        Assert.IsTrue(itemRetrieved.Description == "Item Description");
                    }
                    catch (LazyInitializationException ex)
                    {
                        numberOfLazyInitializationException++;
                    }
                    catch
                    { }
                }
                var statics = Profiler.GetDifferenceFromLastSnapshot();

                // itemRetrieved.Description is lazy one SQL Query expected
                Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
                Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

                // Test All Call has been Intercepted
                Assert.IsTrue(testAllCallIntercepted());
            }
        }

        [TestMethod]
        public virtual void TestLeazyLoading_SaveEntityAndGetEntityKeepSessionOpenMustPerformSQLQueryForLazyCollection()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            Item itemRetrieved;
            Int32 numberOfLazyInitializationException = 0;

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {

                    using (var sqlLogSpy = Profiler.LogSpy)
                    {
                        itemRetrieved = session.Get<Item>(item_guid);
                        tx.Commit();
                    }
                }

                // Reset counter
                Profiler.TakeSnapshot();
                CustomBytecodeProviderInterceptorCounter.ResetCounter();
                EntityCreationCounterHelper.ResetCounter();

                using (var sqlLogSpy = Profiler.LogSpy)
                {
                    try
                    {
                        foreach (Bid bid in itemRetrieved.Bids)
                        {
                            Assert.IsTrue(bid.Id != default(Guid));
                            Assert.IsTrue(bid.Description != String.Empty);
                        }
                    }
                    catch (LazyInitializationException ex)
                    {
                        numberOfLazyInitializationException++;
                    }
                    catch
                    { }
                }

                var statics = Profiler.GetDifferenceFromLastSnapshot();

                // itemRetrieved.Bids is lazy one SQL Query for whole collection expected
                Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
                Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

                // Test All Call has been Intercepted
                Assert.IsTrue(testAllCallIntercepted());
            }
        }

        [TestMethod]
        public virtual void TestLeazyLoading_SaveEntityAndGetEntityKeepSessionOpenMustCallBytecodeProviderInterceptor()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }

            Item itemRetrieved;
            Int32 numberOfLazyInitializationException = 0;

            // Reset counter
            Profiler.TakeSnapshot();
            CustomBytecodeProviderInterceptorCounter.ResetCounter();

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {

                    using (var sqlLogSpy = Profiler.LogSpy)
                    {
                        itemRetrieved = session.Get<Item>(item_guid);
                        tx.Commit();
                    }
                }

                // Reset counter
                Profiler.TakeSnapshot();
                CustomBytecodeProviderInterceptorCounter.ResetCounter();
                EntityCreationCounterHelper.ResetCounter();

                using (var sqlLogSpy = Profiler.LogSpy)
                {
                    try
                    {
                        foreach (Bid bid in itemRetrieved.Bids)
                        {
                            Assert.IsTrue(bid.Id != default(Guid));
                            Assert.IsTrue(bid.Description != String.Empty);

                        }

                    }
                    catch (LazyInitializationException ex)
                    {
                        numberOfLazyInitializationException++;
                    }
                    catch
                    { }
                }

                var statics = Profiler.GetDifferenceFromLastSnapshot();

                // itemRetrieved.Bids is lazy one SQL Query for whole collection expected
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 2);
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 0);

                // Test All Call has been Intercepted
                Assert.IsTrue(testAllCallIntercepted());
            }
        }

        public abstract void ComposeConfiguration(Dictionary<string, string> configuration);

        private bool testAllCallIntercepted()
        {
            return 1 == 1
                   && EntityCreationCounterHelper.NewItemCall == CustomBytecodeProviderInterceptorCounter.NewItemCallIntecepted + CustomBytecodeProviderInterceptorCounter.NewItemProxyCallIntecepted
                   && EntityCreationCounterHelper.NewBidCall == CustomBytecodeProviderInterceptorCounter.NewBidCallIntecepted + CustomBytecodeProviderInterceptorCounter.NewBidProxyCallIntecepted;
        }
    }
}
