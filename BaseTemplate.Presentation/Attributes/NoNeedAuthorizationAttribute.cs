namespace BaseTemplate.Presentation.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoNeedAuthorizationAttribute : Attribute
    {
    }
}
