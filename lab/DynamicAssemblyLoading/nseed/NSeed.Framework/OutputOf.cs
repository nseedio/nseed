namespace NSeed
{
    /// <summary>
    /// Base class for all seed output classes.
    /// </summary>
    /// <typeparam name="T">Type of the seed for which the derived class represents the output.</typeparam>
    public abstract class OutputOf<T> where T : ISeed
    {
        /// <summary>
        /// The original seed object that has created the output.
        /// </summary>
        protected T Seed { get; set; }
    }
}
