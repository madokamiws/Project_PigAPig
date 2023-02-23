using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logs
{
    public enum COLOR_TYPE
    {
        RED,
        BLUE,
    }

    // Color
    // Start is called before the first frame update
    public static void Log(string msg)
    {

#if UNITY_EDITOR
        UnityEngine.Debug.Log(string.Format("<color=red>{0}</color>", msg));
#endif

    }

    // ,Color color = Color.blue
    public static void Log(string msg, COLOR_TYPE color = COLOR_TYPE.BLUE)
    {

#if UNITY_EDITOR
        UnityEngine.Debug.Log(string.Format("<color=yellow>{0}</color>", msg));
#else

#endif

    }


    public static void D(string message, object obj)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.Log(string.Format("{0}-{1}-{2}", System.DateTime.Now, message, obj));
#else
#endif


    }
}
