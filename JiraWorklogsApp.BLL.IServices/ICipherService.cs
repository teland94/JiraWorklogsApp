namespace JiraWorklogsApp.BLL.IServices
{
    public interface ICipherService
    {
        string Encrypt(string plainText);

        string Decrypt(string encryptedText);
    }
}
