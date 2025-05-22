namespace Utils.GUILogger
{
    using System.Collections.Generic;
    public interface IGUILogInvoker
    {
        List<GUILogData> Logs { get; }
    }
}
