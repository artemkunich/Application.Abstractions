using Akunich.Application.Abstractions;

namespace Application.TestPipelineWithNotification;

public class TestCommand : IRequest<Unit>
{
    public string Value { get; set; }
}