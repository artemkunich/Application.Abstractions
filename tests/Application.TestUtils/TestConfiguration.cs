namespace Application.TestUtils;

public class TestConfiguration
{
    public int BehaviorsCount { get; set; }

    public int GetBehaviorBefore(int order) => order;
    
    public int GetBehaviorAfter(int order) => 2 * BehaviorsCount + 2 - order;

    public int GetHandlerValue() => BehaviorsCount + 1;
}