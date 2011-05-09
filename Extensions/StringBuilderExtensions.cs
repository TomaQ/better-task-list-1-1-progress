using System.Security.Cryptography;
using System.Text;

namespace BetterTaskList.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Generates a random sequence of letters and numbers
        /// </summary>
        /// <param name="builder">The builder that is being extended</param>
        /// <param name="len">The number of random characters to append to the string</param>
        /// <returns>The string builder</returns>
        public static StringBuilder AppendRandomString(this StringBuilder builder, int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[len];

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);

            foreach (byte b in data)
            {
                builder.Append(chars[b % (chars.Length - 1)]);
            }

            return builder;
        }

    }
}