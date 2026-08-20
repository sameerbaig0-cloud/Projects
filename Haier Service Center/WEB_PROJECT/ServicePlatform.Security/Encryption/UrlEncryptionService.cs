using Microsoft.AspNetCore.DataProtection;
using System;

namespace ServicePlatform.Security.Encryption
{
    public class UrlEncryptionService : IUrlEncryptionService
    {
        private readonly IDataProtector _protector;

        public UrlEncryptionService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("ServicePlatform.Security.RouteEncryption");
        }

        public string Encrypt(string value)
        {
            return _protector.Protect(value);
        }

        public string Encrypt(long value)
        {
            return Encrypt(value.ToString());
        }

        public string Encrypt(int value)
        {
            return Encrypt(value.ToString());
        }

        public string Encrypt(Guid value)
        {
            return Encrypt(value.ToString());
        }

        public string Decrypt(string encryptedValue)
        {
            return _protector.Unprotect(encryptedValue);
        }

        public long DecryptToLong(string encryptedValue)
        {
            return long.Parse(Decrypt(encryptedValue));
        }

        public int DecryptToInt(string encryptedValue)
        {
            return int.Parse(Decrypt(encryptedValue));
        }

        public Guid DecryptToGuid(string encryptedValue)
        {
            return Guid.Parse(Decrypt(encryptedValue));
        }
    }
}