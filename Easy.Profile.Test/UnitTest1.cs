using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Easy.Rpc.LoadBalance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easy.Profile.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            string value1= ConfigurationManager.AppSettings["test"];
            value1= ConfigurationManager.AppSettings["test"];

            ConfigurationManager.RefreshSection("appSettings");

            value1 = ConfigurationManager.AppSettings["test"];


            Easy.Rpc.directory.DirectoryFactory.Register("profileMonitor", new TestDirectory());

            RedisServer server = new RedisServer("127.0.0.1,abortConnect=false");

            ProfileManager.Instance.Register("profileMonitor", new ReloadProfile<string>("aaa", "aaaa", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.txt"), server, (filePath) =>
            {
                return File.ReadAllText(filePath);
            }));


            var configdata = ProfileManager.Instance.GetConfigData<string>("aaa", "aaaa");

            string value = configdata.GetProfileData();

            Thread.Sleep(30000);

            value = configdata.GetProfileData();

        }

        class TestDirectory : Easy.Rpc.directory.IDirectory
        {
            public IList<Node> GetNodes()
            {
                return new List<Node>() { new Node("profileMonitor", "http://127.0.0.1:8002/", 10, true, "") };
            }

            public string Name()
            {
                return "profileMonitor";
            }

            public void Refresh()
            {
                throw new NotImplementedException();
            }
        }

    }
}
