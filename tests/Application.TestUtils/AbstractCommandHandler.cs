using System.Text;
using Akunich.Application.Abstractions;

namespace Application.TestUtils;

public abstract class AbstractCommandHandler<TCommand> : IRequestHandler<TCommand,Unit> where TCommand : IRequest<Unit>
{
    private IList<int> _resultBuilder;
    private int _appendToResult;
    
    public AbstractCommandHandler(IList<int> resultBuilder, int appendToResult)
    {
        _resultBuilder = resultBuilder;
        _appendToResult = appendToResult;
    }
    
    public virtual Task<Result<Unit>> HandleAsync(TCommand request, CancellationToken cancellation)
    {
        _resultBuilder.Add(_appendToResult);
        return Task.FromResult<Result<Unit>>(Unit.Value);
    }
}