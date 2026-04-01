using CopilotMultiAgent.Application.Common.Exceptions;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Interfaces.Repositories;

namespace CopilotMultiAgent.Application.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TodoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return items.Select(MapToDto);
    }

    public async Task<PagedResultDto<TodoDto>> GetAllAsync(TodoQueryDto query, CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetAllAsync(cancellationToken);

        var filtered = ApplySort(ApplyFilters(items, query), query).ToList();

        var totalCount = filtered.Count;
        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);
        var pageItems = filtered
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResultDto<TodoDto>(pageItems, totalCount, query.Page, query.PageSize, totalPages);
    }

    private static IEnumerable<TodoItem> ApplyFilters(IEnumerable<TodoItem> items, TodoQueryDto query)
    {
        if (query.IsCompleted.HasValue)
            items = items.Where(t => t.IsCompleted == query.IsCompleted.Value);
        if (query.Priority.HasValue)
            items = items.Where(t => t.Priority == query.Priority.Value);
        if (query.Tag is not null)
            items = items.Where(t => t.Tags.Contains(query.Tag));
        return items;
    }

    private static IEnumerable<TodoItem> ApplySort(IEnumerable<TodoItem> items, TodoQueryDto query) =>
        (query.SortBy?.ToLower(), query.SortDirection?.ToLower()) switch
        {
            ("duedate", "desc") => items.OrderByDescending(t => t.DueDate),
            ("duedate", _) => items.OrderBy(t => t.DueDate),
            ("priority", "desc") => items.OrderByDescending(t => t.Priority),
            ("priority", _) => items.OrderBy(t => t.Priority),
            _ => items
        };

    public async Task<TodoDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), id);

        return MapToDto(item);
    }

    public async Task<TodoDto> CreateAsync(CreateTodoDto dto, CancellationToken cancellationToken = default)
    {
        var item = TodoItem.Create(dto.Title, dto.Description, dto.DueDate, dto.Priority, dto.Tags);
        await _repository.AddAsync(item, cancellationToken);
        return MapToDto(item);
    }

    public async Task<TodoDto> UpdateAsync(Guid id, UpdateTodoDto dto, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), id);

        item.UpdateTitle(dto.Title);

        if (dto.IsCompleted && !item.IsCompleted)
            item.Complete();
        else if (!dto.IsCompleted && item.IsCompleted)
            item.Reopen();

        item.UpdateDescription(dto.Description);
        item.SetDueDate(dto.DueDate);
        item.SetPriority(dto.Priority);
        if (dto.Tags is not null)
            item.SetTags(dto.Tags);

        await _repository.UpdateAsync(item, cancellationToken);
        return MapToDto(item);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), id);

        item.SoftDelete();
        await _repository.UpdateAsync(item, cancellationToken);
    }

    public async Task<TodoDto> RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdIncludingDeletedAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), id);

        item.Restore();
        await _repository.UpdateAsync(item, cancellationToken);
        return MapToDto(item);
    }

    private static TodoDto MapToDto(TodoItem item) =>
        new(item.Id, item.Title, item.IsCompleted, item.CreatedAt,
            item.Description, item.DueDate, item.Priority,
            item.UpdatedAt, item.CompletedAt, item.Tags, item.IsDeleted);
}
