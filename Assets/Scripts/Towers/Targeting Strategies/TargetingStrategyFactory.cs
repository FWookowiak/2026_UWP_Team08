public static class TargetingStrategyFactory
{
    public static ITargetingStrategy Create(TargetingMode mode)
    {
        return mode switch
        {
            TargetingMode.Closest    => new ClosestTargetingStrategy(),
            TargetingMode.Strongest  => new StrongestTargetingStrategy(),
            TargetingMode.Weakest    => new WeakestTargetingStrategy(),
            TargetingMode.First      => new FirstTargetingStrategy(),
            _                        => new ClosestTargetingStrategy()
        };
    }
}