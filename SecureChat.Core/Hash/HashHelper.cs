using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SecureChat.Core.Hash
{
    /// <summary>
    /// Various Hashsum static functions.
    /// </summary>
    public static class HashHelper
    {

        /// <summary>
        /// Converts to a MD5 Hashsum.
        /// </summary>
        /// <param name="text">Text to Hashsum.</param>
        /// <returns>Hash Value in HEX.</returns>
        public static string ToHashMD5(this string text)
        {
            return FormatByteToHex(DoTheHash(new MD5CryptoServiceProvider(), text));
        }

        /// <summary>
        /// Converts to a SHA1 Hashsum.
        /// </summary>
        /// <param name="text">Text to Hashsum.</param>
        /// <returns>Hash Value in HEX.</returns>
        public static string ToHashSHA1(this string text)
        {
            return FormatByteToHex(DoTheHash(new SHA1Managed(), text));
        }

        /// <summary>
        /// Converts to a SHA256 Hashsum.
        /// </summary>
        /// <param name="text">Text to Hashsum.</param>
        /// <returns>Hash Value in HEX.</returns>
        public static string ToHashSHA256(this string text)
        {
            return FormatByteToHex(DoTheHash(new SHA256Managed(), text));
        }

        /// <summary>
        /// Generic function to do Hash Algorithm.ComputeHash.
        /// </summary>
        /// <param name="generichash">Hash Algorithm.</param>
        /// <param name="text">Text To Hash.</param>
        /// <returns>The Hash in a byte array.</returns>
        private static byte[] DoTheHash(HashAlgorithm generichash, string text)
        {
            return generichash.ComputeHash(ConvertTextASCIIandToByte(text));
        }

        /// <summary>
        /// Turns Text to ASCII and return as a byte[].
        /// </summary>
        /// <param name="text">Text to Turn ASCII and Byte[].</param>
        /// <returns>The Text in byte[].</returns>
        private static byte[] ConvertTextASCIIandToByte(string text)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetBytes(text);
        }

        /// <summary>
        /// Converts a byte[] to Hex (string).
        /// </summary>
        /// <param name="byteToConvert">Byte Array to convert to Hex.</param>
        /// <returns>String of the ByteArray in HEX.</returns>
        public static string FormatByteToHex(byte[] byteToConvert)
        {
            StringBuilder hex = new StringBuilder(byteToConvert.Length);
            foreach (byte x in byteToConvert)
            {
                hex.AppendFormat("{0:X2}", x);
            }

            return hex.ToString();
        }
    }
}
