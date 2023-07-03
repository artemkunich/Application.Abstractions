using Akunich.Application.Abstractions;

namespace Application.TestPipelineWithNotification;

public class TestNotification : INotification
{
    public int HandlerOrder { get; set; }
}