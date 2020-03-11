namespace Liba.FilesManagers.Interfaces
{
    public interface IFileManager
    {
        string ReadText();
        void WriteText(string text);
    }
}