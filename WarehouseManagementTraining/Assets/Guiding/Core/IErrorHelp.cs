namespace Guiding.Core
{
    public interface IErrorHelp :IShowHelp
    {
        string GetName();
        
        void Initialize(GuidingController guidingController);
    }
}
