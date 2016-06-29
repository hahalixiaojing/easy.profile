using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.HttpRequestService;
using Easy.Public.MyLog;
using Easy.Rpc;
using Easy.Rpc.Cluster;
using Easy.Rpc.LoadBalance;

namespace Easy.Profile
{
   public  interface IProfileService
    {
        string GetProfileContent(string profileServiceName, string application, string profile);
    }

    public class ProfileService : IProfileService
    {
        public string GetProfileContent(string profileServiceName,string application, string profile)
        {
            var invoker = new ProfileInvoker<string>((i, node, path) =>
            {
                string url = i.JoinURL(node, path);
                string query = $"?application={application}&profile={profile}";
                try
                {
                    var result = HttpRequestClient.Request(url + query, "GET", false).Send().GetBodyContent(true);
                    return result;
                }
                catch (Exception e)
                {
                    LogManager.Error("GetProfileContent error", e.Message);
                }
                return string.Empty;
            });

            var context = new InvokerContext(
                    new DirectoryContext("Profile/Pull", profileServiceName),
                    new ClusterContext(FailoverCluster.NAME),
                    new LoadBalanceContext(RandomLoadBalance.NAME));

            return ClientInvoker.Invoke(invoker, context);
        }
    }

    class ProfileInvoker<T> : BaseInvoker<T>
    {
        public ProfileInvoker(Func<BaseInvoker<T>, Node, string, T> func) 
            : base(func)
        {

        }
        public override string JoinURL(Node node, string path)
        {
            return node.Url.Trim() + path.Trim();
        }
    }

    static class ProfileServiceFactory
    {
        static readonly IProfileService service = new ProfileService();

        public static IProfileService Profile
        {
            get
            {
                return service;
            }
        }
    }

}
