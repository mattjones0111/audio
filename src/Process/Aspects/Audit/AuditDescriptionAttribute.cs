namespace Process.Aspects.Audit
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class AuditDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public AuditDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}