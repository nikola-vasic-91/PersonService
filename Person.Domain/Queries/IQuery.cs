using MediatR;

namespace PersonService.Domain.Queries
{
    /// <summary>
    /// Interface describing the query for the desired return type
    /// </summary>
    /// <typeparam name="TResult">Type of the result object</typeparam>
    public interface IQuery<out TResult> : IRequest<TResult>
    {
        public Guid CorrelationId { get;}
    }

    /// <summary>
    /// Interface describing the query handler for the desired return type and specific input
    /// </summary>
    /// <typeparam name="TQuery">Input type</typeparam>
    /// <typeparam name="TResult">Return type</typeparam>
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public Guid CorrelationId { get; }
    }
}
