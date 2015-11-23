using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using NHibernate;
using System;
using System.Collections.Generic;

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
        public void SaveObjectNoInterceptionConfiguration()
        {
            Guid item_guid = Guid.NewGuid();
            Guid bid1_guid = Guid.NewGuid();
            Guid bid2_guid = Guid.NewGuid();

            Item item = new Item();
            item.Id = item_guid;
            item.Description = "Item Description";

            item.AddBid(bid1_guid, "Bid1 Description");
            item.AddBid(bid2_guid, "Bid2 Description");

            Assert.IsTrue(item.Id == item_guid);
            Assert.IsTrue(item.Description == "Item Description");

            Assert.IsTrue(item.Bids[0].Id == bid1_guid);
            Assert.IsTrue(item.Bids[0].Description == "Bid1 Description");

            Assert.IsTrue(item.Bids[1].Id == bid2_guid);
            Assert.IsTrue(item.Bids[1].Description == "Bid2 Description");

            StaticCounterHelper.ResetCounter();

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(item);
                    tx.Commit();
                }
            }
        }

        [TestMethod]
        public void SaveObjectAndGetObjectNoInterceptionConfiguration()
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

        }

        [TestMethod]
        public virtual void SaveObjectAndGetObjectNoInterceptionConfigurationExceptionForLeazyLoadingField()
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

            Int32 numberOfException = 0;
            try
            {
                Assert.IsTrue(itemRetrieved.Id == item_guid);
            }
            catch
            {
                numberOfException++;
            }

            try
            {
                Assert.IsTrue(itemRetrieved.Description == "Item Description");
            }
            catch
            {
                numberOfException++;
            }

            try
            {
                foreach (Bid bid in itemRetrieved.Bids)
                {
                    Assert.IsTrue(bid.Id != default(Guid));
                    Assert.IsTrue(bid.Description != String.Empty);

                }

            }
            catch
            {
                numberOfException++;
            }

            Assert.IsTrue(numberOfException == 2);
        }

        [TestMethod]
        public virtual void SaveObjectAndGetObjectNoInterceptionConfigurationNoExceptionForLeazyLoadingField()
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
            Int32 numberOfException = 0;

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    itemRetrieved = session.Get<Item>(item_guid);
                    tx.Commit();
                }

                try
                {
                    Assert.IsTrue(itemRetrieved.Id == item_guid);
                }
                catch
                {
                    numberOfException++;
                }

                try
                {
                    Assert.IsTrue(itemRetrieved.Description == "Item Description");
                }
                catch
                {
                    numberOfException++;
                }

                try
                {
                    foreach (Bid bid in itemRetrieved.Bids)
                    {
                        Assert.IsTrue(bid.Id != default(Guid));
                        Assert.IsTrue(bid.Description != String.Empty);

                    }

                }
                catch
                {
                    numberOfException++;
                }
            }
            Assert.IsTrue(numberOfException == 0);
        }

        public abstract void ComposeConfiguration(Dictionary<string, string> configuration);
    }
}
