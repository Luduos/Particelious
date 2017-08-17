using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ALogger{

    [System.Diagnostics.Conditional("ENABLE_LOGS")]
    public static void Debug(string logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }
}
