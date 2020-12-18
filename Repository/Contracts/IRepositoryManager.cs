namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        INoteRepository Note { get; }
        void Save();
    }
}