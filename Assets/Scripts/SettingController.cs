using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Yes.Game.Chicken
{
    public class SettingController : BaseDialogController
    {
        public Button btn_BGMOff;
        public Image img_BGMOff;
        public Button btn_BGMOn;

        public Button btn_SoundOff;
        public Image img_SoundOff;
        public Button btn_SoundOn;
        public static SettingController _instance;
        public static SettingController Get
        {
            get
            {
                if (_instance == null)
                {
                    string path = "SettingController";
                    _instance = (SettingController)SettingController.InitBase(_instance, path);
                }
                return (SettingController)_instance;
            }
        }
        public override void Awake()
        {
            _instance = this;
            // 检查BGM状态
            if (AudioManager.Instance.isBGMEnabled)
            {
                btn_BGMOn.gameObject.SetActive(true);
                img_BGMOff.gameObject.SetActive(false);
            }
            else
            {
                btn_BGMOn.gameObject.SetActive(false);
                img_BGMOff.gameObject.SetActive(true);
            }

            // 检查音效状态
            if (AudioManager.Instance.is2DSoundEnabled)
            {
                btn_SoundOn.gameObject.SetActive(true);
                img_SoundOff.gameObject.SetActive(false);
            }
            else
            {
                btn_SoundOn.gameObject.SetActive(false);
                img_SoundOff.gameObject.SetActive(true);
            }
        }

        public void OnClickBGMOff()
        {
            btn_BGMOn.gameObject.SetActive(true);
            img_BGMOff.gameObject.SetActive(false);
            AudioManager.Instance.isBGMEnabled = true;
            AudioManager.Instance.UnPauseBGM();

        }
        public void OnClickBGMOn()
        {
            btn_BGMOn.gameObject.SetActive(false);
            img_BGMOff.gameObject.SetActive(true);
            AudioManager.Instance.isBGMEnabled = false;
            AudioManager.Instance.PauseBGM();

        }
        public void OnClickSoundOff()
        {
            btn_SoundOn.gameObject.SetActive(true);
            img_SoundOff.gameObject.SetActive(false);

            AudioManager.Instance.is2DSoundEnabled = true;
        }
        public void OnClickSoundOn()
        {
            btn_SoundOn.gameObject.SetActive(false);
            img_SoundOff.gameObject.SetActive(true);

            AudioManager.Instance.is2DSoundEnabled = false;
        }

        public override void OnClose()
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}