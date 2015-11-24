
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestNoLeazyLoading
{
    [TestClass]
    public class TestNoInterceptionNoSecondLevelCache : BaseTest
    {
        public override void ComposeConfiguration(Dictionary<string, string> configuration)
        {
            configuration.Add("DataSource", @"MAURIZIO-WRK\SQLEXPRESS");
            configuration.Add("UserId", "sa");
            configuration.Add("Password", "maurizio");
            configuration.Add("Database", "NHibernate_UnityBytecodeProvider");
            configuration.Add("ConnectTimeout", "30");
            configuration.Add("DropAndCreateDatabaseSchema", "True");
            configuration.Add("UseSecondLevelCache", "False");
            configuration.Add("UseNHibernateSimpleProfiler", "True");
            configuration.Add("ConfigurationAssembly", "TestNoLeazyLoading");
        }
    }
}
