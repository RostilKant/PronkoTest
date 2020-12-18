using System;
using System.Linq;
using System.Linq.Expressions;
using Entities;
using Entities.Models;
using Repository.Contracts;

namespace Repository
{
    public class NoteRepository: RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
        
    }
}