namespace DataAccess.DbAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedures, U parameters, string connectionId = "Default");
        Task SaveData<T>(string storedProcedures, T parameters, string connectionId = "Default");
    }
}