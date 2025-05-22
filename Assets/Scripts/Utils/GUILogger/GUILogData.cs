namespace Utils.GUILogger
{
    using System;
    using UnityEngine;

    [Serializable]
    public class GUILogData
    {
        public IGUILogInvoker invoker;
        public LogType logType;
        public string message;
    }
}
