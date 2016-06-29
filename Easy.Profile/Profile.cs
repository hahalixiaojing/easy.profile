using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Profile
{
    public abstract class Profile
    {
        /// <summary>
        /// 配置文件是发生变化
        /// </summary>
        public volatile bool IsChanged;
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string ProfileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 订阅KEY
        /// </summary>
        public string SubscribeKey
        {
            get
            {
                return this.Application + "_" + this.ProfileName;
            }
        }
    }
}
