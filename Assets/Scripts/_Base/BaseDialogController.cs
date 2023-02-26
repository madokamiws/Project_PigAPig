using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yes.Game.Chicken
{
    public class BaseDialogController : MonoBehaviour
    {
        public static Dictionary<BaseDialogController, string> controllers = new Dictionary<BaseDialogController, string>();

        public Animator animator;


        public virtual void Start()
        {
            if (animator)
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;


        }

        public virtual void PlayAnimator(string trigger, float speed = 1)
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (animator)
            {
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                animator.SetTrigger(trigger);
                animator.speed = speed;
            }

            // if (animator) animator.speed = speed;
            // if (animator) animator.SetTrigger(trigger);
        }

        public static GameObject GetObj(string path)
        {
            // string path = "WalletController";
            GameObject _prototype = Instantiate(UnityEngine.Resources.Load(path)) as GameObject;
            return _prototype;
        }

        public static BaseDialogController InitBase(BaseDialogController _instance, string path)
        {

            if (_instance == null)
            {
                // string path = "WalletController";
                GameObject obj = GetObj(path);
                if (obj)
                {
                    _instance = obj.GetComponent<BaseDialogController>();
                    GameObject canvas = GameObject.Find("Canvas");
                    if (canvas)
                    {
                        obj.transform.SetParent(canvas.transform, false);
                    }
                }
            }

            return _instance;
        }


        public static BaseDialogController InitBaseToSence(BaseDialogController _instance, string path)
        {
            if (_instance == null)
            {
                // string path = "WalletController";
                GameObject obj = GetObj(path);
                if (obj)
                {
                    _instance = obj.GetComponent<BaseDialogController>();
                }
            }

            return _instance;
        }

        public virtual void Awake()
        {

        }



        public static bool IsDialogShowed()
        {

            if (controllers.Count > 0)
            {

                foreach (var item in controllers)
                {

                    BaseDialogController obj = item.Key;
                    if (obj == null || obj.gameObject == null)
                    {
                        controllers.Clear();
                        return false;
                    }
                    // Console.WriteLine(item.Key + item.Value);

                }
            }


            return controllers.Count > 0;
        }

        public virtual void OnGet()
        {

        }

        public virtual void OnApplicationPause(bool pause)
        {

        }


        public static bool OnExitDialog()
        {
            if (controllers.Count > 0)
            {
                foreach (var item in controllers)
                {
                    item.Key.OnClose();
                    // Console.WriteLine(item.Key + item.Value);
                }
                return true;
            }
            return false;
        }

        public virtual void Show()
        {

            gameObject.SetActive(true);
            //GameController.Status = GameStatus.Pause;

            //Utils.TimeScale = 0;
            // Time.timeScale = 0 ;
            // PlayAnimator("show");
            if (!controllers.ContainsKey(this))
                controllers.Add(this, this.name);

            //CameraController.OnDragingStart();
            // Logs.Log(string.Format("******************* Show() controllers.count={0}, Time.timeScale={1}", controllers.Count, Time.timeScale ));
            // Loading.Hide() ;

            PlayAnimator("show");
        }

        public virtual void OnClose()
        {

            //GameController.Status = GameStatus.Playing;

            if (controllers.ContainsKey(this))
                controllers.Remove(this);

            if (controllers.Count > 0)
            {
                List<BaseDialogController> lists = null;

                int max = 0;
                int count = controllers.Count;
                while (controllers.Count > 0)
                {
                    // BaseDialogController baseDialog = controllers[0] ;
                    foreach (var item in controllers)
                    {
                        BaseDialogController obj = item.Key;
                        if (obj == null || obj.gameObject == null)
                        {
                            // Logs.Log( "错误的类名" + item.Value  ) ;
                            controllers.Remove(obj);
                            // controllers.Clear() ;
                            break;
                        }
                        // Console.WriteLine(item.Key + item.Value);
                    }

                    max += 1;
                    if (max > count)
                        break;
                }

            }

            //CameraController.OnDragEnd();
            //gameObject.SetActive(false);

            //if (controllers.Count > 0)
            //    Utils.TimeScale = 0;
            //// Time.timeScale = 1 ;
            //else
            //    Utils.TimeScale = 1;

            //// Time.timeScale = 0 ;
            //// DelayRemove() ;

        }

        public virtual void OnClosePlaying()
        {
            if (controllers.ContainsKey(this))
                controllers.Remove(this);

            if (controllers.Count > 0)
            {
                List<BaseDialogController> lists = null;
                foreach (var item in controllers)
                {
                    BaseDialogController obj = item.Key;
                    if (obj == null || obj.gameObject == null)
                    {
                        // controllers.Clear();
                        // return false;
                    }
                    else
                    {

                        if (lists == null)
                            lists = new List<BaseDialogController>();
                        lists.Add(obj);
                    }
                    // Console.WriteLine(item.Key + item.Value);
                }

                if (lists == null || lists.Count == 0)
                {
                }
                else
                {
                    controllers.Clear();
                    for (int i = 0; i < lists.Count; i++)
                        controllers.Add(lists[i], this.name);
                }
            }
            gameObject.SetActive(false);
            OnClose();
        }

        public void DelayRemove()
        {

            //// Logs.Log("--DelayRemove()(001)--" + this  + "--controllers.Count--" + controllers.Count);
            //Timer.Schedule(0.5f, () => {
            //    // Logs.Log("--DelayRemove()(002)--" + this);
            //    if (controllers.ContainsKey(this))
            //        controllers.Remove(this);

            //});


        }

        public static void DestoryChilds(Transform content)
        {

            if (content == null)
                return;

            int num = 0;
            while (content.childCount > 0)
            {
                DestroyImmediate(content.GetChild(0).gameObject);
                if (num++ > 256)
                    break;
            }
        }




    }

}

