---
description: Coding Conventions
applyTo: **/*.cs
---

## C# Style

**Không dùng primary constructor:**
```csharp
// ĐÚNG
public class CreateTodoHandler : ICommandHandler<CreateTodoCommand>
{
    private readonly ITodoRepository _repository;

    public CreateTodoHandler(ITodoRepository repository)
    {
        _repository = repository;
    }
}

// SAI
public class CreateTodoHandler(ITodoRepository repository) : ICommandHandler<CreateTodoCommand>
```

**Không dùng arrow method (expression-bodied method):**
```csharp
// ĐÚNG
public async Task<Todo> GetByIdAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// SAI
public async Task<Todo> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
```

**Dùng `var` khi type rõ ràng từ right-hand side:**
```csharp
var handler = new CreateTodoHandler(repository); // OK
ITodoRepository repo = GetRepository();          // OK — interface type cần explicit
```

**Async/await:**
- Mọi method async đều có suffix `Async`
- Không dùng `.Result` hoặc `.Wait()`
- CancellationToken là parameter cuối nếu method là public

## Naming

- Commands: `CreateTodoCommand`, `UpdateTodoCommand`, `DeleteTodoCommand`
- Queries: `GetTodoByIdQuery`, `GetAllTodosQuery`
- Handlers: `CreateTodoCommandHandler`, `GetTodoByIdQueryHandler`
- Tests: `[Method]_[Scenario]_[ExpectedResult]` — ví dụ: `Handle_WhenTitleIsEmpty_ThrowsValidationException`

## Test Style

```csharp
[Fact]
public async Task Handle_WhenTitleIsEmpty_ThrowsValidationException()
{
    // Arrange
    var repository = Substitute.For<ITodoRepository>();
    var command = new CreateTodoCommand { Title = string.Empty };
    var handler = new CreateTodoCommandHandler(repository);

    // Act
    var act = async () => await handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<ValidationException>()
        .WithMessage("*Title*");
}

[Fact]
public async Task Handle_WhenValid_CallsRepositoryOnce()
{
    // Arrange
    var repository = Substitute.For<ITodoRepository>();
    var command = new CreateTodoCommand { Title = "Buy milk" };
    var handler = new CreateTodoCommandHandler(repository);

    // Act
    await handler.Handle(command, CancellationToken.None);

    // Assert
    await repository.Received(1).AddAsync(Arg.Is<Todo>(t => t.Title == "Buy milk"));
}
```

- Dùng xUnit + NSubstitute + FluentAssertions
- Arrange-Act-Assert comments bắt buộc
- Không test private methods — test qua public interface
