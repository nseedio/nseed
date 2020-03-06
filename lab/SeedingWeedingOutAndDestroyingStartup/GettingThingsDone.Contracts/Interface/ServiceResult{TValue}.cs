using System.Collections.Generic;
using System.Linq;

namespace GettingThingsDone.Contracts.Interface
{
    /// <summary>
    /// Represent a service result that returns an additional value of type <typeparamref name="TValue"/>.
    /// By convention, if the status is not <see cref="ServiceCallStatus.Ok"/> the <see cref="Value"/>
    /// should be the default value of <typeparamref name="TValue"/>.
    /// </summary>
    public sealed class ServiceResult<TValue> : IServiceResult
    {
        public TValue Value { get; }

        public IEnumerable<string> Messages { get; }

        public ServiceCallStatus Status { get; }

        internal ServiceResult(TValue value, ServiceCallStatus status, string message)
            : this(value, status, new[] { message })
        {
        }

        internal ServiceResult(TValue value, ServiceCallStatus status, IEnumerable<string> messages = null)
        {
            Value = value;
            Status = status;
            Messages = messages ?? Enumerable.Empty<string>();
        }
    }
}
