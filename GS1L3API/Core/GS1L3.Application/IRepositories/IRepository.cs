using GS1L3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GS1L3.Application.IRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
