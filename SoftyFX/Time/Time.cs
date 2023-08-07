using System;

namespace SoftyFX.Time
{
    public class Time
    {
        private DateTime _lastTime, _currentTime;
        private float _deltaTime;
        
        public Time()
        {
            _lastTime = DateTime.Now;
            _currentTime = DateTime.Now;
            _deltaTime = float.Epsilon;
        }

        public float GetDeltaTime()
        {
            return _deltaTime;
        }

        public void Begin()
        {
            _currentTime = DateTime.Now;
            _deltaTime = (_currentTime.Ticks - _lastTime.Ticks) / 10000000f; 
        }
        
        public void End()
        {
            _lastTime = _currentTime;
        }
    }
}