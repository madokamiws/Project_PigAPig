using UnityEngine;
using UnityEngine.UI;
using System;
namespace Yes.Game.Chicken
{
    public class CountdownController : SingletonPatternMonoBase<CountdownController>
    {
        public Slider timerSlider; // Drag your Slider component here in Inspector

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
            }
            else
            {
                countdownTime = 60f;
                ResetTimer();
            }
            

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