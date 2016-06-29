using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.Utilities
{
    /// <summary>
    /// SponeBob refrence! Which I think is better
    /// than a hashbrown joke so....yeah there is that
    /// </summary>
    static class HashSlingingSlasher
    {

        internal static string HashString(string sString)
        {
            StringBuilder sBuilder = new StringBuilder();
            string sInput = sString;
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sInput));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.


                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        internal static string HashStrings(params string[] strings)
        {
            StringBuilder sBuilder = new StringBuilder();
            string sInput = string.Join("", strings);
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sInput));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
               

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        internal static string HashObjects(params object[] objects)
        {
            StringBuilder sBuilder = new StringBuilder();
            string sInput = "";
            foreach (object o in objects)
            {
                sInput += o.ToString();
            }
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sInput));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.


                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
