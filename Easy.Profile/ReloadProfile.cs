using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Profile
{
    /// <summary>
    /// 配置文件重新加载对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReloadProfile<T>: Profile, IGetConfigData<T>
    {
        private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
      
        private T data;
        private readonly Func<string, T> reload;
        public ReloadProfile(string application,string profileName,string filePath,RedisServer redis, Func<string,T> reload)
        {
            this.Application = application;
            this.ProfileName = profileName;
            this.FilePath = filePath;
            this.reload = reload;
            this.data = reload(this.FilePath);
            redis.Subscribe(UpdateLocalProfile, this.SubscribeKey);
        }

        
        private void UpdateLocalProfile(string content)
        {
            SaveProfileHelper.Save(this.FilePath, content);
            this.IsChanged = true;
        }

        private T ReloadAndGetData()
        {
            rwlock.EnterWriteLock();
            try
            {
                if (!this.IsChanged)
                {
                    return data;
                }
                else
                {
                    data = this.reload.Invoke(this.FilePath);
                    this.IsChanged = false;
                    return data;
                }
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        private T GetData()
        {
            rwlock.EnterReadLock();
            try
            {
                return data;
            }
            finally
            {
                rwlock.ExitReadLock();
            }
        }

        /// <summary>
        /// 获得配置文件信息，如果配置文件修改了，重新加载
        /// </summary>
        /// <returns></returns>
        public T GetProfileData()
        {
            if (this.IsChanged)
            {
                return this.ReloadAndGetData();
            }
            else
            {
                return this.GetData();
            }
        }
    }
}
