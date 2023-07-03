using Akunich.Application.Abstractions;

namespace Application.TestPipeline;

public class TestCommand : IRequest<Unit>
{
    public string Value { get; set; }
}