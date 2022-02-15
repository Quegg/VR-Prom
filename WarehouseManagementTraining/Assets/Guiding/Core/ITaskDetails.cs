namespace Guiding.Core
{
    public interface ITaskDetails: IShowHelp
    {
        string GetNameRaw();
      
        bool IsDone();
        void TaskStarted();
        void TaskEnded();
        void Initialize(GuidingController guidingController);
    }
}
