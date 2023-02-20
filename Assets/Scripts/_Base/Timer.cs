using UnityEngine;
using System.Collections;
using System;
namespace Yes.Game.Chicken
{
    public class Timer
    {
        private static MonoBehaviour behaviour;
        public delegate void Task();

        public static void Schedule(MonoBehaviour _behaviour, float delay, Task task)
        {
            if (_behaviour)
            {
                behaviour = _behaviour;
                behaviour.StartCoroutine(DoTask(task, delay));
            }

        }

        /*
        public static void Schedule ( MonoBehaviour _behaviour, GameObject obj , float delay, Task task)
        {
            if (_behaviour) {
                behaviour = _behaviour;
                behaviour.StartCoroutine( DoTask(task, delay) ) ;
            }

        }
        */

        public static void Schedule(MonoBehaviour _behaviour, GameObject obj, float delay, Task task)
        {
            if (_behaviour)
            {
                behaviour = _behaviour;
                behaviour.StartCoroutine(DoTaskWithObj(task, obj, delay));
            }

        }

        private static IEnumerator DoTaskWithObj(Task task, GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            task();
        }

        private static IEnumerator DoTask(Task task, float delay)
        {
            yield return new WaitForSeconds(delay);
            task();
        }

        public static IEnumerator DelayToInvokeDo(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        //public static void Schedule(float delay, Task task)
        //{
        //    behaviour = TimerController.Get;
        //    if (behaviour)
        //        behaviour.StartCoroutine(DoTask(task, delay));

        //    // behaviour = _behaviour ;
        //    // TaskControoller.instance.StartCoroutine(DoTask(task, delay));
        //}

        public static IEnumerator DelayTask(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }



    }
}