using System;
using System.IO;
using System.Text;
using Liba.FilesManagers.Interfaces;

namespace Liba.FilesManagers.Implements
{
    public class FileManager : IFileManager
    {
        private readonly string filePath;

        public FileManager(string filePath)
        {
            this.filePath = filePath;
        }

        public string ReadText()
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var fileSize = (int)fileStream.Length;

                var bufferSize = 4 * 1024;
                var blocks = Math.Ceiling((decimal)fileSize / bufferSize);

                using (var bufferedStream = new BufferedStream(fileStream))
                {
                    byte[] array = new byte[bufferSize];
                    var stringBuilder = new StringBuilder();

                    for (int i = 0; i < blocks; i++)
                    {
                        int readCount;

                        if (i < blocks - 1)
                        {
                            readCount = bufferSize;
                        }
                        else
                        {
                            readCount = fileSize - bufferSize * i;
                        }

                        bufferedStream.Read(array, (int)bufferedStream.Position, readCount);

                        byte[] textArray = readCount == array.Length
                            ? array
                            : array[..readCount];

                        var text = Encoding.UTF8.GetString(textArray);

                        stringBuilder.Append(text);
                    }

                    return stringBuilder.ToString();
                }
            }
        }

        public void WriteText(string text)
        {
            var bufferSize = 4 * 1024;
            var blocks = Math.Ceiling((decimal)text.Length / bufferSize);

            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var bufferedStream = new BufferedStream(fileStream))
                {
                    var textArray = Encoding.UTF8.GetBytes(text);

                    for (int i = 0; i < blocks; i++)
                    {
                        int writeCount;

                        if (i < blocks - 1)
                        {
                            writeCount = bufferSize;
                        }
                        else
                        {
                            writeCount = textArray.Length - bufferSize * i;
                        }

                        bufferedStream.Write(textArray, 0, writeCount);
                    }
                }
            }
        }
    }
}