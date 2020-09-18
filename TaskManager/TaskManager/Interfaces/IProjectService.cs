using System;
using System.Threading.Tasks;

namespace TaskManager.Interfaces
{
    public interface IProjectService
    {
        Task ThrowIfProjectDoesNotExists(Guid projectId);
    }
}
