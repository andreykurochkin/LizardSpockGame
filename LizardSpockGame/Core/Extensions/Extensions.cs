using System;
namespace LizardSpockGame.Core.Extensions {
    public static class Extensions {
        public static string ToHexString(this byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "");
    }
}
