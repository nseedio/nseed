using System.Collections.Generic;
using System.Linq;

namespace NSeed.Cli.Abstractions
{
    internal class OperationResponse : IOperationResponse
    {
        protected OperationResponse() { }

        public bool IsSuccessful { get; protected set; } = false;

        public IEnumerable<string> Messages { get; protected set; } = Enumerable.Empty<string>();

        public string Message { get; protected set; } = string.Empty;

        public static IOperationResponse Success()
        {
            return new OperationResponse() { IsSuccessful = true };
        }

        public static IOperationResponse Error(params string[] messages)
        {
            return new OperationResponse { Messages = messages };
        }

        public static IOperationResponse Error(string message)
        {
            var response = new OperationResponse
            {
                Message = message
            };
            return response;
        }
    }

#pragma warning disable SA1402
#pragma warning disable SA1618
    internal class OperationResponse<TPayload> : OperationResponse, IOperationResponse<TPayload>
        where TPayload : class
    {
        public static IOperationResponse<TPayload> Success(TPayload payload)
        {
            return new OperationResponse<TPayload> { Payload = payload, IsSuccessful = true };
        }

        public static IOperationResponse<IEnumerable<TPayload>> Success(IEnumerable<TPayload> payload)
        {
            return new OperationResponse<IEnumerable<TPayload>> { Payload = payload, IsSuccessful = true };
        }

        public static new IOperationResponse<TPayload> Error(params string[] messages)
        {
            return new OperationResponse<TPayload> { IsSuccessful = false, Messages = messages };
        }

        public static new IOperationResponse<TPayload> Error(string message)
        {
            return new OperationResponse<TPayload> { IsSuccessful = false, Message = message };
        }

        public TPayload? Payload { get; private set; } = default;
    }
#pragma warning restore SA1618
#pragma warning restore SA1402
}
