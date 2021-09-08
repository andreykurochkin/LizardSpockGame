using LizardSpockGame.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace LizardSpockGame.Core {
    public class Key {
        private readonly byte[] _key;
        private readonly HMAC _hmac;
        public Key() {
            _key = CreateRandomBytes();
            _hmac = new HMACSHA256(_key);
        }
        public override string ToString() => _key.ToHexString();
        public byte[] ComputeHmac(string input) => _hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        private static byte[] CreateRandomBytes() {
            byte[] bytes = new byte[16];
            RandomNumberGenerator.Fill(bytes);
            return bytes;
        }
    }
}
