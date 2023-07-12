using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yes.Game.Chicken
{
    public enum LoadingStyle
    {
        clear,
        white,
        gray,
        green,
        red,
        yellow,
        blue
    }

    public class Loading : MonoBehaviour {

        public GameObject loadingImage;
        public Text loadingText;
        public Image back ;

        public Sprite[] background ;

        public Animator animator;


        public   void Start()
        { 
            if (animator)
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        }

        // Use this for initialization
        // GameObject.Find("Canvas"）.GetComponet<Canvas>()

        public static GameObject GetObj() {
            string path = "Loading";
            GameObject _prototype = Instantiate(UnityEngine.Resources.Load(path)) as GameObject;
            return _prototype ;
        }

        public static Loading _instance;
        public static Loading Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject canvas = GameObject.Find("Canvas");
                    if (canvas)
                    {
                        GameObject loading = GetObj() ;
                        // new GameObject( "UILoading" ) ; // 创建一个新的GameObject
                        if (loading)
                        {
                            DontDestroyOnLoad(loading);  // 防止被销毁
                            _instance = loading.GetComponent<Loading>(); // 将实例挂载到GameObject上
                                                                         // loading.transform.parent = canvas.transform ;

                            loading.transform.SetParent(canvas.transform, false);
                            loading.transform.localScale = Vector3.one;

                            RectTransform rect = loading.GetComponent<RectTransform>();
                            if (rect != null)
                            {
                                Vector2 anchor = rect.pivot;
                                rect.pivot = new Vector2(0, anchor.y + 0.50f);
                            }

                        }
                    }
                }
                return _instance;
            }
        }

        void Awake() {
            _instance = this;
        }

        static float mDataRefreshLeftTime = 5f ;
        // Update is called once per frame
        void __Update() {

            mDataRefreshLeftTime -= Time.deltaTime;
            if ( mDataRefreshLeftTime <= 0 ) {
                Hide() ;
            }

            if (loadingImage) {
                //以模型Z轴旋转，单位为2.
                loadingImage.transform.Rotate(0, 0, -6.0f);
            }

        }
 
        private void _hide()
        {

            gameObject.SetActive(false);
            Destroy(this) ;
            Destroy(gameObject);
            _instance = null ;
        }


        public static void Hide() {

            if (Loading._instance)
                _instance._hide();

            // Logs.Log(" loading....Hide() ") ;
            // gameObject.SetActive ( false ) ;
        }

        public static void Show ( LoadingStyle style= LoadingStyle.clear ) {

            mDataRefreshLeftTime = 5 ;
            Loading.Instance._show(style) ;
            // gameObject.SetActive ( false ) ;
        }

        public void _setMessage ( string message ) {

            if ( loadingText ) {
                loadingText.text = message ;
            }
            // Loading.Instance._show();
            // gameObject.SetActive ( false ) ;
        }

        public static void SetMessage ( string message ) {
            Loading.Instance._setMessage ( message ) ;
        }

        private void _show(LoadingStyle style = LoadingStyle.clear) {
 
            if ( style == LoadingStyle.clear ) {
            }
            else  {

                if( background== null || background.Length < 5 ) { }
                else  {
                    back.sprite = background[Random.Range(2, background.Length)];
                }
            }

            gameObject.SetActive(true);

            Timer.Schedule(this, 8, () => {
                Hide();
            });

            if ( loadingText ) {
                loadingText.text = " 加载中";
            }

        }

        private void _show ( string message ) {

            gameObject.SetActive(true);

            Timer.Schedule(this, 6, () => {
                Hide();
            });

            if (loadingText){
                loadingText.text = message;
            }


            if (loadingText)
            {
                if (string.IsNullOrEmpty(message)) {
                    loadingText.text = "loading...";
                }
                else
                {
                    loadingText.text = message;
                }

            }

        }

        public static void Show(string message)
        {
            Loading.Instance._show(message);

        }

        // loadingText
        // blockCoin
        // levelText
    



    }

}