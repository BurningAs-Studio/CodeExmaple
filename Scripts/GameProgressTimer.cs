using UnityEngine;
using FAS.Save;
using System;

namespace FAS
{
    public class GameProgressTimer : MonoBehaviour
    {
        private static float _currentSessionTimeSeconds;
        private static float _timeSinceLastLog;
        private static float _pauseStartTime;
        private static bool _isGamePaused;

        private const float SECONDS_PER_MINUTE = 60f;

        public static event Action<int> OnTimerContinue;

        public void Awake()
        {
            _isGamePaused = false;
        }

        public static int GetCurrentSessionTimeInMinutes()
        {
            return Mathf.FloorToInt(_currentSessionTimeSeconds / SECONDS_PER_MINUTE);
        }

        public static int GetTotalPlayTimeInMinutes()
        {
            return SaveData.TotalPlayTimeInMinutes;
        }

        private void HandlePauseState(bool isPaused)
        {
            if (isPaused)
            {
                _isGamePaused = true;
                _pauseStartTime = Time.timeSinceLevelLoad;
            }
            else
            {
                if (_isGamePaused)
                {
                    var pauseDuration = Time.timeSinceLevelLoad - _pauseStartTime;
                    OnTimerContinue?.Invoke(Mathf.FloorToInt(pauseDuration));
                }
                _isGamePaused = false;
            }
        }

        private void Update()
        {
            if (!_isGamePaused)
            {
                _currentSessionTimeSeconds += Time.deltaTime;
                _timeSinceLastLog += Time.deltaTime;

                if (_timeSinceLastLog >= SECONDS_PER_MINUTE)
                {
                    SaveData.TotalPlayTimeInMinutes++;
                    //AnalyticEvents.CheckAndSendTimeSpendEvent(SaveData.TotalPlayTimeInMinutes);
                    _timeSinceLastLog = 0f;
                }
            }
        }

        private void OnApplicationPause(bool isPaused)
        {
            HandlePauseState(isPaused);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            HandlePauseState(!hasFocus);
        }
    }
}
