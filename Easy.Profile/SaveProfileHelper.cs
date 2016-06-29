using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.MyLog;

namespace Easy.Profile
{
    /// <summary>
    /// 保存配置文件帮助类
    /// </summary>
    static class SaveProfileHelper
    {
        public static bool Save(string filePath,string content)
        {
            string remoteContent = content.Trim();
            string fileContent = GetFileContent(filePath).Trim();
            if (!string.IsNullOrWhiteSpace(remoteContent) && StringSameCheckhelper.Md5CheckIsChange(fileContent, remoteContent))
            {
                try
                {
                    string path = filePath;
                    File.WriteAllText(path, content, Encoding.UTF8);
                    return true;
                }
                catch (Exception e)
                {
                    LogManager.Error("update file error", filePath + e.Message);
                }
            }
            return false;
        }
        private static string GetFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            return File.ReadAllText(filePath, Encoding.UTF8);
        }
    }
}
