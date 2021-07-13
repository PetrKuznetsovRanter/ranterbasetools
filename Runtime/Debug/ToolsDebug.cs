using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RanterTools.Base
{


    public class ToolsDebug
    {
        public static void Log(object log)
        {
#if RANTER_TOOLS_DEBUG_BASE
            Debug.Log(log);
#endif
        }
        public static void Log(object log, Object context)
        {
#if RANTER_TOOLS_DEBUG_BASE
            Debug.Log(log,context);
#endif
        }

        public static void LogError(object log)
        {
#if RANTER_TOOLS_DEBUG_BASE
            Debug.LogError(log);
#endif
        }
        public static void LogError(object log, Object context)
        {
#if RANTER_TOOLS_DEBUG_BASE
            Debug.LogError(log,context);
#endif
        }

        public static void LogWarning(object log)
        {
#if RANTER_TOOLS_DEBUG_BASE
            Debug.Log(log);
#endif
        }
        public static void LogWarning(object log, Object context)
        {
#if RANTER_TOOLS_DEBUG_BASE
            Debug.Log(log,context);
#endif
        }
    }
}
