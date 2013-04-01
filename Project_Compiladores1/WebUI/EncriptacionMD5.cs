using System;
using System.Security.Cryptography;
using System.Text;

namespace WebUI
{
    public class EncriptacionMD5
    {
        //private const string Key = "SegConUserKey";
        public static string CreateMd5Hash(string rawData)
        {
            var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(rawData));
            var str = "";
            var numArray = hash;
            var index = 0;
            while (index < numArray.Length)
            {
                var num = numArray[index];
                str = str + num.ToString("x2");
                checked { ++index; }
            }
            return str;
        }
    }
}