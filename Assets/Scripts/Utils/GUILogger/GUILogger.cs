namespace Utils.GUILogger
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    public static class GUILogger
    {
        public static GUIStyle titleStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            fontSize = 18,
            alignment = TextAnchor.MiddleCenter
        };
        
        public static GUIStyle LogStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        
        public static GUIStyle ErrorStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.red
            }
        };
        
        public static GUIStyle WarningStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.yellow
            }
        };
        
        
        public static void Log(this IGUILogInvoker guiLogInvoker, LogType logType, string message)
        {
            var log = new GUILogData()
            {
                invoker = guiLogInvoker,
                logType = logType,
                message = message
            };
            guiLogInvoker.Logs.Add(log);
            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Exception:
                    Debug.LogException(new Exception(message));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        public static void DrawLogs(object obj, Rect buttonRect, float verticalSpacing)
        {
            #if UNITY_EDITOR
            if(obj.GetType().GetProperty("Logs") != null)
            {
                var logs = obj.GetType().GetProperty("Logs").GetValue(obj);
                var logsProperty = (List<GUILogData>)logs;
                if(logsProperty.Count > 0)
                {
                    if(GUI.Button(buttonRect, "Clear Logs"))
                    {
                        logsProperty.Clear();
                    }
                }
                for (int i = 0; i < logsProperty.Count; i++)
                {
                    GUILayout.Label(logsProperty[i].message, GetLogStyle(logsProperty[i].logType));
                    GUILayout.Space(verticalSpacing);
                }
            }
            #endif
        }

        private static GUIStyle GetLogStyle(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return ErrorStyle;
                case LogType.Assert:
                    return LogStyle;
                case LogType.Warning:
                    return WarningStyle;
                case LogType.Log:
                    return LogStyle;
                case LogType.Exception:
                    return ErrorStyle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }
    }
}
