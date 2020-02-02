using System.Security.Cryptography;

namespace System
{
    public sealed class RandomNumberGenerator
    {
        public int GetRandomNumber(uint min, uint max)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[5];
                rng.GetBytes(data);
                var randomSeed = (int)BitConverter.ToUInt32(data, 0);
                return new Random(randomSeed).Next((int)min, (int)max);
            }
        }
    }
}
