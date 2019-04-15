namespace Process.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Aspects.Validation;
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;

    public class FeatureBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
    {
        readonly IEnumerable<IValidator<TRequest>> validators;

        public FeatureBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            ValidationContext context = new ValidationContext(request);

            IEnumerable<ValidationFailure> failures = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .ToList();

            if(failures.Any())
            {
                throw new FeatureValidationException(failures);
            }

            return await next();
        }
    }
}
