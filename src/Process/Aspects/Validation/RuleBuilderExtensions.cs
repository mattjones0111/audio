namespace Process.Aspects.Validation
{
    using System.Net;
    using FluentValidation;

    public static class RuleBuilderOptionsExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithHttpStatusCode<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder,
            HttpStatusCode httpStatusCode)
        {
            return ruleBuilder.WithErrorCode(((int)httpStatusCode).ToString());
        }
    }
}