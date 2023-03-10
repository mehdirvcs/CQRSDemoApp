using DataAccess.Models;

namespace DataAccess.DbAccess
{
    public interface ICosmosDataAccess
    {
        Task AddItemToContainerAsync(UserModel user);
        Task DeleteFamilyItemAsync(string partitionKeyValue, string userId);
        Task<List<UserModel>> QueryItemsAsync(string sqlQueryText);
        Task ReplaceFamilyItemAsync(UserModel user);
    }
}