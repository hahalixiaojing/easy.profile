using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easy.Profile.Test
{
    [TestClass]
    public class ProfileHttpPullMonitor
    {
        [TestMethod]
        public void Md5CheckIsChangeTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upgradeformysql.xml");

            string content = File.ReadAllText(path);

            StringSameCheckhelper.Md5CheckIsChange(content, content);
        }
    }
}
