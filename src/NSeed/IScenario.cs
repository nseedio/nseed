namespace NSeed
{
    /// <summary>
    /// A single scenario. Scenarios are grouping of <see cref="ISeed"/>s and other <see cref="IScenario"/>s.
    /// <br/>
    /// For example, in an online store application by combining existing seeds we can define different
    /// scenarios to simulate different sales seasons.
    /// </summary>
    /// <remarks>
    /// Scenarios do not have their own implementation. A concrete scenario is an
    /// empty non-abstract class marked with this interface.
    /// </remarks>
    public interface IScenario { }
}