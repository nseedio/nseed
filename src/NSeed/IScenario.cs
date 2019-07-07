namespace NSeed
{
    /// <summary>
    /// A single scenario. Scenarios are grouping of <see cref="ISeed"/>s and other <see cref="IScenario"/>s.
    /// <br/>
    /// For example, in an online store application by combining existing seeds we can define different
    /// scenarios to simulate different sales seasons.
    /// </summary>
    /// <remarks>
    /// Scenarios do not have their own implementations. A concrete scenario is an
    /// empty non-abstract class marked with this interface. The seeds and other
    /// scenarios that are part of a scenario are defined by annotating that empty
    /// scenario class with the <see cref="RequiresAttribute"/>.
    /// </remarks>
    public interface IScenario { }
}