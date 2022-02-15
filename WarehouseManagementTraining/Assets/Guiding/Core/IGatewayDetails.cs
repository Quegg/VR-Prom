using System.Collections.Generic;

namespace Guiding.Core
{
    public interface IGatewayDetails
    {
        string GetName();
        string CheckConditions(List<string> possibleNextElements);
    }
}
