using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace MediatR
{
    public sealed class RequestValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            if (validators is null)
                throw new ArgumentNullException(nameof(validators));

            _validators = validators.ToList();
        }

        public Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (next is null)
                throw new ArgumentNullException(nameof(next));

            var context = new ValidationContext(request);

            var validationFailures = _validators
                .Select(validation => validation.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (validationFailures.Count != 0)
                throw new ValidationFailuresException(validationFailures);

            return next();
        }
    }
}
