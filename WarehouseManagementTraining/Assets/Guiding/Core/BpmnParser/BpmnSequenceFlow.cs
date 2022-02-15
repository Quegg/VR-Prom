namespace Guiding.Core.BpmnParser
{
    public class BpmnSequenceFlow
    {
        public string sourceId;
        public string targetId;
        public string id;
        public string name;
        public BpmnElement source;
        public BpmnElement target;
    }
}