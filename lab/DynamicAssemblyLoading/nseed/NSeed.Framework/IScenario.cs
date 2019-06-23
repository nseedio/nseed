namespace NSeed
{
    /// <summary>
    /// A scenario. Scenarios are collections of seeds and other scenarios.
    /// Scenarios enable easy definition of a particular combination of
    /// individual seeds that has to be seeded. For example, in an online store
    /// application by combining existing seeds we can define different scenarios to
    /// simulate different sales seasons.
    /// </summary>
    /// <remarks>
    /// Scenarios do not have their own implementation. A concrete scenario is an
    /// empty class marked with this interface. The seeds and eventually other
    /// scenarios that are the part of the scenario are defined via the <see cref="RequiresAttribute"/>
    /// applied to the empty scenario class.
    /// </remarks>
    public interface IScenario
    {
    }
}