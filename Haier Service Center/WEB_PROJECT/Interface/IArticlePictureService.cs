namespace ServicePlatform.Interface
{

    public interface IArticlePictureService
    {
        Task<List<string>> GetArticlePictureUrlsAsync(List<string> blobFileNames);
    }

}
