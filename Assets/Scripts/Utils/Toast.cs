﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Yes.Game.Chicken
{
    public class Toast : MonoBehaviour
    {
        public GameObject loadingImage;
        public Text tx_messages;
        public RectTransform background;
        public RectTransform rt_text;

        public Animator animator;

        public static GameObject getShow()
        {
            string path = "Toast";
            GameObject _prototype = Instantiate(UnityEngine.Resources.Load(path)) as GameObject;
            return _prototype;
        }

        public static Toast _instance;
        public static Toast Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject canvas = GameObject.Find("Canvas");
                    if (canvas)
                    {
                        GameObject loading = getShow();
                        // new GameObject( "UILoading" ) ; // 创建一个新的GameObject
                        if (loading)
                        {
                            DontDestroyOnLoad(loading);  // 防止被销毁
                            _instance = loading.GetComponent<Toast>();
                            loading.transform.SetParent(canvas.transform);
                            loading.transform.localScale = Vector3.one;
                            loading.transform.localPosition = Vector3.zero;

                            RectTransform rect = loading.GetComponent<RectTransform>();
                            if (rect != null)
                            {

                            }
                        }
                    }
                }
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
        }

        static Vector3 posOri = new Vector3(0, -3, 0);
        void Start()
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            // posOri = transform.position ;
            // Logs.Log ( " posOri = " + posOri ) ;
        }

        public void PlayAnimator(string trigger, float speed = 1)
        {
            animator.speed = speed;
            animator.SetTrigger(trigger);
        }

        public void OnShow(string message, int time = 2)
        {
            transform.SetAsLastSibling();
            PlayAnimator("playup");

            tx_messages.text = message;
            tx_messages.fontSize = 50;

            float width = tx_messages.preferredWidth;
            float height = tx_messages.preferredHeight;

            background.sizeDelta = new Vector2(width + 120f, height + 50f);

            gameObject.SetActive(true);
            StartCoroutine(HideAfterDelay(time));
        }
        private IEnumerator HideAfterDelay(int delay)
        {
            // Wait for `delay` seconds.
            yield return new WaitForSeconds(delay);

            // Then hide (and destroy) the toast.
            hide();
        }

        public void hide()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            Destroy(this);
            _instance = null;

        }
        public void OnClose()
        {
            hide();
        }

        public static void Show(string message, int time = 2)
        {
            try
            {
                if (string.IsNullOrEmpty(message) || Toast.Instance == null)
                {
                    return;
                }

                Toast.Instance.OnShow(message, time);

            }
            catch
            {

            }
        }
        public static void Show(string message, int time, float offset)
        {

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (Toast.Instance)
            {

                Instance.transform.position = posOri;
                Instance.transform.position += new Vector3(0, offset);
                Toast.Instance.OnShow(message, time);

            }
        }
    }
}

