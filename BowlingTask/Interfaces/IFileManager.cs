namespace BowlingTask
{
    interface IFileManager
    {
        int GetArrayLength();
        string GetFileNameFromArray(int position);
        string ReadFile(int position);
        int CalculateScore();
    }
}
