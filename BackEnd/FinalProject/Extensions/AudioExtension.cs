namespace FinalProject.Extensions
{
    public static class AudioExtension
    {
        public static bool IsAudio(this IFormFile file)
        {
            return file.FileName.EndsWith(".mp3");
        }
        public static bool CheckAudioSize(this IFormFile file, int size)
        {
            return file.Length / 1024 > size;
        }
        public static string SaveAudio(this IFormFile file, IWebHostEnvironment env, string root, string fileName)
        {
            string fullPath = Path.Combine(env.WebRootPath, root, fileName);

            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
    }
}
