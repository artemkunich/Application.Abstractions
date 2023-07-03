using Akunich.Application.Abstractions;

namespace Application.TestPipelineWithNotification;

public class TestCommandAfterNotification : IRequest<Unit>
{
    public int ValueFromNotification { get; set; }

    public string Value { get; set; }
}