using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.Security.Cryptography;

namespace Easy.Profile
{
    public static class StringSameCheckhelper
    {
        public static bool Md5CheckIsChange(string fileContent, string remoteContent)
        {
            var fileContentBuilder = new StringBuilder(fileContent);
            fileContentBuilder.Replace("\n", "")
                .Replace(" ", "")
                .Replace("\t", "")
                .Replace("\r", "")
                .Replace(" ", "");
            string newfileContentString = fileContentBuilder.ToString().ToUpperInvariant();

            var remoteContentBuilder = new StringBuilder(remoteContent);
            remoteContentBuilder.Replace("\n", "")
                .Replace(" ", "")
                .Replace("\t", "")
                .Replace("\r", "")
                .Replace(" ", "");

            string newRemoteContentString = remoteContentBuilder.ToString().ToUpperInvariant();

            return MD5Helper.Encrypt(newfileContentString) != MD5Helper.Encrypt(newRemoteContentString);
        }
    }
}
