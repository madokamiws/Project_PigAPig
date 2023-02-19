
//using UnityEngine;
//using UnityEngine.UI;
//using System.Runtime.InteropServices;
//using System.Collections;
//namespace Yes.Game.Chicken
//{
//    public class CopyDebug : MonoBehaviour
//    {
//        public InputField input;

//#if UNITY_IOS
//        [DllImport("__Internal")]
//        private static extern void _copyTextToClipboard(string text);
//#endif

//        public static void OnClickCopyText(string input_text)
//        {
//#if UNITY_ANDROID
//            AndroidJavaObject androidObject = new AndroidJavaObject("com.androidclicp.ClipboardTools");
//            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
//            if (activity == null)
//                return;
//            // 复制到剪贴板
//            androidObject.Call("copyTextToClipboard", activity, input_text);

//            // 从剪贴板中获取文本
//            string text = androidObject.Call<string>("getTextFromClipboard");
//#elif UNITY_IOS
//        _copyTextToClipboard(input.text);
//#elif UNITY_EDITOR
//        TextEditor te = new TextEditor();
//        te.content = new GUIContent(input.text);
//        te.SelectAll();
//        te.Copy();
//#endif
//        }

//    }
//}