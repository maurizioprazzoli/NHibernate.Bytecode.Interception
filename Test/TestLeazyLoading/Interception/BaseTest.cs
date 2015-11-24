using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using NHibernate;
using NHibernateSimpleProfiler;
using System;
using System.Collections.Generic;
using TestInterceptionLeazyLoading.Interception;

namespace TestInterceptionLeazyLoading
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
        public void SaveObject()
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

            // No call
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 0);

            Assert.IsTrue(statics.NumberOfSQLSelectStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLInsertStatement == 2);
            Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);
        }

        [TestMethod]
        public void SaveObjectAndGetObject()
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

            // No Instance Bid since is lazy
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
            // Item must be proxyed
            Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 1);

            Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
            Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
            Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

        }

        [TestMethod]
        public virtual void SaveObjectAndGetObjectClosedSessionExceptionForLeazyLoading()
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
        public virtual void SaveObjectAndGetObjectKeepSessionOpenNoExceptionForLeazyLoadingField()
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

                var statics = Profiler.GetDifferenceFromLastSnapshot();

                // No Instance Bid since is lazy
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
                // Item must be proxyed
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 1);

                Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
                Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

                // Reset counter
                Profiler.TakeSnapshot();
                CustomBytecodeProviderInterceptorCounter.ResetCounter();

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
                statics = Profiler.GetDifferenceFromLastSnapshot();

                // No Instance Bid since is lazy
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 0);

                Assert.IsTrue(statics.NumberOfSQLSelectStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

                // Reset counter
                Profiler.TakeSnapshot();
                CustomBytecodeProviderInterceptorCounter.ResetCounter();

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
                statics = Profiler.GetDifferenceFromLastSnapshot();

                // No Instance Bid since is lazy
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 0);

                Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
                Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);

                // Reset counter
                Profiler.TakeSnapshot();

                CustomBytecodeProviderInterceptorCounter.ResetCounter();
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
                // Two Instance Bid
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateInstance == 0);
                Assert.IsTrue(CustomBytecodeProviderInterceptorCounter.CreateProxyInstance == 0);

                Assert.IsTrue(statics.NumberOfSQLSelectStatement == 1);
                Assert.IsTrue(statics.NumberOfSQLInsertStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLUpdateStatement == 0);
                Assert.IsTrue(statics.NumberOfSQLDeleteStatement == 0);
            }
            Assert.IsTrue(numberOfLazyInitializationException == 0);
        }

        public abstract void ComposeConfiguration(Dictionary<string, string> configuration);
    }
}
