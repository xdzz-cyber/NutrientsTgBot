namespace FileProvider
{
    public class FileSystemProvider : IFileSystemProvider
    {
        public bool Exists(string filename)
        {
            return File.Exists(filename);
        }

        public Stream Read(string filename)
        {
            return !Exists(filename)
                ? throw new FileNotFoundException()
                : File.OpenRead(filename);
        }

        public async Task WriteAsync(string filename, Stream stream)
        {
            await using var writer = new StreamWriter(filename, false);
            await stream.CopyToAsync(writer.BaseStream);
        }
    }
}
