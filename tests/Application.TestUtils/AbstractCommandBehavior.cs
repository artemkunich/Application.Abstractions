using System.Text;
using Akunich.Application.Abstractions;

namespace Application.TestUtils;

public abstract class AbstractCommandBehavior<TCommand> : IPipelineBehavior<TCommand,Unit> where TCommand : IRequest<Unit>
{
    private IList<int> _resultBuilder;
    private int _appendBefore;
    private int _appendAfter;
    
    public AbstractCommandBehavior(IList<int> resultBuilder, int appendBefore, int appendAfter)
    {
        _resultBuilder = resultBuilder;
        _appendBefore = appendBefore;
        _appendAfter = appendAfter;
    }

    public async Task<Result<Unit>> HandleAsync(TCommand request, CancellationToken cancellation, NextDelegate<Unit> nextAsync)
    {
        _resultBuilder.Add(_appendBefore);
        var result = await nextAsync();
        _resultBuilder.Add(_appendAfter);
        return result;
    }
}