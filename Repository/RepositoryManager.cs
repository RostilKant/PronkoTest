using System.Threading.Tasks;
using Entities;
using Repository.Contracts;

namespace Repository
{
    public class RepositoryManager: IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private INoteRepository _noteRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        
        public INoteRepository Note => _noteRepository ??= new NoteRepository(_repositoryContext);

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}