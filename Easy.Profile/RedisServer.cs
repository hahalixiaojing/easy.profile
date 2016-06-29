using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.MyLog;
using StackExchange.Redis;

namespace Easy.Profile
{
    public class RedisServer
    {
        readonly ConnectionMultiplexer redis;

        public RedisServer(string redisServer)
        {
            try
            {

                redis = ConnectionMultiplexer.Connect(redisServer);
            }
            catch (System.Exception e)
            {
                LogManager.Error("Redis Connect Error", e.Message);
            }
        }
        public void Subscribe(Action<string> action, string profielName)
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe(profielName, (c, m) =>
            {
                action.Invoke(m);
            });
        }
        public void UnSubscribe(string channel)
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Unsubscribe(channel);
        }
    }
}
