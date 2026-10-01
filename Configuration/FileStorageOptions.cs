namespace GPACARICOMAPI.Configuration
{
    public class FileStorageOptions
    {
        public string RootPath { get; set; } = "uploads";

        public int MaxFileSizeMB { get; set; } = 10;
    }
}
