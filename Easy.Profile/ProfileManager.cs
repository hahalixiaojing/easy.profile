using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.MyLog;

namespace Easy.Profile
{
    public class ProfileManager
    {
        private static object lockObj = new object();
        private static ProfileManager manager;
        private readonly IDictionary<string, Profile> profiles = new Dictionary<string, Profile>();

        public static ProfileManager Instance
        {
            get
            {
                if (manager == null)
                {
                    lock (lockObj)
                    {
                        if (manager == null)
                        {
                            manager = new ProfileManager();
                        }
                    }
                }
                return manager;
            }
        }

        public void Register(string profileServiceName,Profile profile)
        {
            try
            {
                string content = ProfileServiceFactory.Profile.GetProfileContent(profileServiceName, profile.Application, profile.ProfileName);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    bool ischange = SaveProfileHelper.Save(profile.FilePath, content);
                    if (ischange)
                    {
                        profile.IsChanged = true;
                    }
                }
            }
            catch (Exception e) { LogManager.Error("拉取配置文件异常", e.Message); }

            profiles.Add(profile.SubscribeKey, profile);
        }
        public IGetConfigData<T> GetConfigData<T>(string applcation,string profile)
        {
            if(profiles.ContainsKey(applcation+"_"+ profile))
            {
                return profiles[applcation + "_" + profile] as IGetConfigData<T>;
            }
            return null;
        }
    }
}
