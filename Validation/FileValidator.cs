using System.IO;

namespace Liba.Validation.Implements
{
    public static class FileValidator
    {
        public static bool IsValid(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}