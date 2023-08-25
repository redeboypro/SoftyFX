using System;
using System.Threading;
using System.Threading.Tasks;
using SoftyFX.Graphics;

namespace SoftyFX.Display
{
    public abstract class Display
    {
        private readonly Time.Time _time;

        protected Display()
        {
            _time = new Time.Time();
            SoftyContext.InitDevice();
        }

        public void Run()
        {
            OnLoad();
            SoftyRenderer.InitBuffer();
            while (!SoftyContext.ReadyToQuit)
            {
                _time.Begin();
                SoftyContext.UnlockDoubleBuffer();
                OnUpdateFrame(_time);
                SoftyContext.LockDoubleBuffer();
                _time.End();
            }
            SoftyContext.ReleaseDevice();
        }

        protected abstract void OnLoad();
        
        protected abstract void OnUpdateFrame(Time.Time time);
    }
}