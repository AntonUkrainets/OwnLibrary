using System;
using System.IO;
using System.Text;
using Liba.Logger.Interfaces;

namespace Liba.Logger.Implements
{
    public class FileLogger : ILogger
    {
        private string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public void LogInformation(string message)
        {
            var messageArray = Encoding.ASCII.GetBytes($"{DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss")} {message}{Environment.NewLine}");

            var bufferSize = 4 * 1024;
            var blocks = Math.Ceiling((decimal)messageArray.Length / bufferSize);

            using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (var bufferedStream = new BufferedStream(fileStream))
                {
                    for (int i = 0; i < blocks; i++)
                    {
                        int writeCount = 0;

                        if (i < blocks - 1)
                        {
                            writeCount = bufferSize;
                        }
                        else
                        {
                            writeCount = messageArray.Length - bufferSize * i;
                        }

                        bufferedStream.Write(messageArray, 0, writeCount);
                    }
                }
            }
        }
    }
}