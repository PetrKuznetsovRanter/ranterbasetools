using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BummerTools.Base
{


    public class HashMD5
    {
        public static string GetHashMD5(string text)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(text);
            return GetHashMD5(bytes);
        }

        public static string GetHashMD5(byte[] data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(data);
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            return hashString.PadLeft(32, '0');
        }
    }


}