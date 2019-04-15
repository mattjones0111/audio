namespace Process.Aspects.Validation
{
    using System;
    using System.Collections.Generic;
    using FluentValidation.Results;

    public class FeatureValidationException : Exception
    {
        public IEnumerable<ValidationFailure> Failures { get; }

        public FeatureValidationException(
            IEnumerable<ValidationFailure> failures)
        {
            Failures = failures;
        }
    }
}