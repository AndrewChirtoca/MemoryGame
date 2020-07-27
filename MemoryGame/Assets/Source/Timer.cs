//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Creator: Chirtoca Andrei <andrewchirtoca@gmail.com>
//
//===----------------------------------------------------------------------===//


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;



namespace MemoryGame
{
    /// <summary>
    /// Timer class.
    /// </summary>
    public class Timer
    {
#region Public serialized variables
        public UnityEvent onCountdownEnd = new UnityEvent();
        public UnityEvent onTimerTick = new UnityEvent();
#endregion



#region Private variables
        private float _timeLeft;
        private float _tickInterval;
        private MonoBehaviour _runner;
        private Coroutine _countdownRoutine;
#endregion



#region Public methods and properties
        public Timer(MonoBehaviour runner, float tickInterval)
        {
            _runner = runner;
            _tickInterval = tickInterval;
        }

        ~Timer()
        {
            onCountdownEnd.RemoveAllListeners();
            onTimerTick.RemoveAllListeners();
        }

        public void StartCountdown(float initialTime)
        {
            StopCountdown();
            _countdownRoutine = _runner.StartCoroutine(TimerTick(initialTime));
        }

        public void StopCountdown()
        {
            if(_countdownRoutine != null)
            {
                _runner.StopCoroutine(_countdownRoutine);
            }
        }

        public float TimeLeft
        {
            get
            {
                return _timeLeft;
            }
        }
#endregion


        private IEnumerator TimerTick(float initialTime)
        {
            _timeLeft = initialTime;
            var yieldTime = new WaitForSeconds(_tickInterval);

            while(_timeLeft > 0)
            {
                yield return yieldTime;
                _timeLeft -= _tickInterval;
                onTimerTick.Invoke();
            }

            onCountdownEnd.Invoke();
        }
    }
}