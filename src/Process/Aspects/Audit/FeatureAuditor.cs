namespace Process.Aspects.Audit
{
    using System.Reflection;
    using System.Threading.Tasks;
    using MediatR.Pipeline;

    public class FeatureAuditor<TRequest, TResponse> :
        IRequestPostProcessor<TRequest, TResponse>
    {
        public Task Process(TRequest request, TResponse response)
        {
            AuditDescriptionAttribute attribute = request
                .GetType()
                .GetCustomAttribute<AuditDescriptionAttribute>();

            if(attribute == null)
            {
                return Task.CompletedTask;
            }

            // TODO audit

            return Task.CompletedTask;
        }
    }
}