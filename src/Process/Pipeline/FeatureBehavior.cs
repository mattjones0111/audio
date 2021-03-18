namespace Process.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Aspects.Validation;
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class FeatureBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
    {
        readonly IEnumerable<IValidator<TRequest>> validators;
        readonly ILogger logger;

        public FeatureBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<FeatureBehavior<TRequest, TResponse>> logger)
        {
            this.validators = validators;
            this.logger = logger;
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

            try
            {
                TResponse response = await next();

                return response;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Exception executing feature.");
                throw;
            }
        }
    }
}
