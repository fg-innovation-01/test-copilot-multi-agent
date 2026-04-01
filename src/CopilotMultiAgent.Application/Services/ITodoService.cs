using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResultDto<TodoDto>> GetAllAsync(TodoQueryDto query, CancellationToken cancellationToken = default);
    Task<TodoDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TodoDto> CreateAsync(CreateTodoDto dto, CancellationToken cancellationToken = default);
    Task<TodoDto> UpdateAsync(Guid id, UpdateTodoDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TodoDto> RestoreAsync(Guid id, CancellationToken cancellationToken = default);
}
