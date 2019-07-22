using Compute.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compute.Infrastructure.Repositories
{
    public interface IOperationRepository
    {
        Task<bool> SaveChangesAsync();

        Operation CreateOp(Operation operation);

        Task<IEnumerable<Operation>> GetAllOpsAsync();

        Task<Operation> GetOpAsync(int operationId);

    }
}
