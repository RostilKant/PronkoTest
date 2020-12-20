using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        INoteRepository Note { get; }
        Task SaveAsync();
    }
}