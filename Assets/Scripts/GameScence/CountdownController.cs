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

        private float countdownTime; // Set the countdown time
        private float currentTime;
        private float totalTime;
        private bool isPaused = true;

        public void SetupTimer(DeckModel deckmodel = null)
        {
            if (deckmodel != null)
            {
                float time_3 = deckmodel.time_3;
                this.countdownTime = time_3;
                ResetTimer();

                // Calculate the ratios and set the positions of the gold ranks
                float ratio1 = 0;
                float ratio2 = (float)deckmodel.time_1 / deckmodel.time_3;
                float ratio3 = (float)deckmodel.time_2 / deckmodel.time_3;
                SetPosition(goldRank1, ratio1);
                SetPosition(goldRank2, ratio2);
                SetPosition(goldRank3, ratio3);

            }
            else
            {
                countdownTime = 90f;
                ResetTimer();

                float ratio1 = 0;
                float ratio2 = (float)30 / 90;
                float ratio3 = (float)60 / 90;

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
                    timerSlider.value = Math.Max(currentTime / countdownTime, 0); // Ensure the slider value won't go below 0
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