using AonFreelancing.Models;

namespace AonFreelancing.Repositories.IRepos
{
    public interface IUserRepo
    {
        public Task<object> GetUserDtoByTypeAsync(string userTypem, User user);
    }
}
