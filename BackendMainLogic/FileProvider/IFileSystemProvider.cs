namespace FileProvider
{
    public interface IFileSystemProvider
    {
        bool Exists(string filename);
        Stream Read(string filename);
        Task WriteAsync(string filename, Stream stream);
    }
}
