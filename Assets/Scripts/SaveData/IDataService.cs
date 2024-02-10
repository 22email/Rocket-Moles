// Interface which defines how data is saved
// Useful for different data formatting types: XML, JSON, etc.
// Only use JSON in this project
public interface IDataService
{
    bool SaveData<T>(string relativePath, T data);
    T LoadData<T>(string relativePath);
}
