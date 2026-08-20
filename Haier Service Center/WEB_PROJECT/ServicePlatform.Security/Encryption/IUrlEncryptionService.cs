
namespace ServicePlatform.Security.Encryption
{

    public interface IUrlEncryptionService
    {
        string Encrypt(string value);

        string Encrypt(long value);

        string Encrypt(int value);

        string Encrypt(Guid value);

        string Decrypt(string encryptedValue);

        long DecryptToLong(string encryptedValue);

        int DecryptToInt(string encryptedValue);

        Guid DecryptToGuid(string encryptedValue);
    }


}