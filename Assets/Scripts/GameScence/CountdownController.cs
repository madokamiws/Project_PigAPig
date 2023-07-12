using UnityEngine;
using UnityEngine.UI;
using System;
namespace Yes.Game.Chicken
{
    public class CountdownController : SingletonPatternMonoBase<CountdownController>
    {
        public Slider timerSlider; // Drag your Slider component here in Inspector

        public RectTransform goldRank1;
        public RectTransform goldRank2;
        public RectTransform goldRank3;

        public Text tx_gold1;//1等段奖励

        public Text tx_gold2;//2等段奖励

        public Text tx_gold3;//3等段奖励

        public GameObject overgold1;

        public GameObject overgold2;

        public GameObject overgold3;

        private float countdownTime; // Set the countdown time
        private float currentTime;
        private float totalTime;
        private bool isPaused = true;

        private float time_cover2;
        private float time_cover3;

        public void SetupTimer(DeckModel deckmodel = null)
        {
            overgold3.SetActive(false);
            overgold2.SetActive(false);
            overgold1.SetActive(false);

            if (deckmodel != null)
            {
                float total_time = deckmodel.time_3;
                this.countdownTime = total_time;
                ResetTimer();

                float _time1 = total_time - deckmodel.time_1;
                float _time2 = total_time - deckmodel.time_2;
                float _time3 = 0;

                time_cover2 = _time1;
                time_cover3 = _time2;

                // Calculate the ratios and set the positions of the gold ranks
                float ratio1 = _time1 / total_time;
                float ratio2 = _time2 / total_time;
                float ratio3 = 0;

                tx_gold3.text = deckmodel.gold_3.ToString();
                tx_gold2.text = deckmodel.gold_2.ToString();
                tx_gold1.text = deckmodel.gold_1.ToString();

                SetPosition(goldRank1, ratio1);
                SetPosition(goldRank2, ratio2);
                SetPosition(goldRank3, ratio3);

            }
            else
            {
                countdownTime = 20f;
                ResetTimer();

                float ratio1 = (float)15 / 20;
                float ratio2 = (float)5 / 20;
                float ratio3 = 0;
                time_cover2 = 15f;
                time_cover3 = 5f;


                tx_gold1.text = "30";
                tx_gold2.text = "50";
                tx_gold3.text = "90";

                SetPosition(goldRank1, ratio1);
                SetPosition(goldRank2, ratio2);
                SetPosition(goldRank3, ratio3);
            }
        }
        private void SetPosition(RectTransform rectTransform, float ratio)
        {
            float sliderWidth = timerSlider.GetComponent<RectTransform>().rect.width;
            float actualPos = ratio * sliderWidth - sliderWidth / 2;
            rectTransform.anchoredPosition = new Vector2(actualPos, 0f);

        }

        private void Update()
        {
            if (!isPaused)
            {
                totalTime += Time.deltaTime;
                if (currentTime > 0)
                {

                    currentTime -= Time.deltaTime;
                    //Debug.Log("currentTime = " + currentTime);
                    if (currentTime < time_cover2)
                    {
                        overgold1.SetActive(true);
                    }
                    if (currentTime < time_cover3)
                    {
                        overgold2.SetActive(true);
                    }

                    timerSlider.value = Math.Max(currentTime / countdownTime, 0); // Ensure the slider value won't go below 0
                }
                else
                {
                    overgold3.SetActive(true);
                }
            }
        }

        public void StartTimer()
        {
            isPaused = false;
        }

        public void PauseTimer()
        {
            isPaused = true;
        }

        public void ResetTimer()
        {
            currentTime = countdownTime;
            totalTime = 0;
            timerSlider.value = 1; // Set Slider to full value
            isPaused = false;
        }

        public float GetTotalTime()
        {
            PauseTimer();
            return totalTime;
        }
    }
}