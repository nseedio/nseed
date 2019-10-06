using System.Collections.Generic;

namespace NSeed.Cli.Abstractions
{
    internal interface IOperationResponse
    {
        bool IsSuccessful { get; }

        IEnumerable<string> Messages { get; }

        string Message { get; }
    }

#pragma warning disable SA1402
#pragma warning disable SA1618
    internal interface IOperationResponse<out TPayload> : IOperationResponse
        where TPayload : class
    {
        TPayload? Payload { get; }
    }

#pragma warning restore SA1618
#pragma warning restore SA1402

}
