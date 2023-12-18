using System;
using System.Threading;
using System.Threading.Tasks;
using SoftyFX.Graphics;

namespace SoftyFX.Display
{
    public abstract class Display
    {
        private readonly Time.Time _updateTime, _renderTime;
        private readonly bool _useMultithreading;

        protected Display(string title, int width, int height, bool useMultithreading = false)
        {
            Console.Title = title;
            
            _updateTime = new Time.Time();
            _renderTime = new Time.Time();

            _useMultithreading = useMultithreading;
            SoftyContext.InitDevice(width, height);
        }

        public void Run()
        {
            SoftyRenderer.GenBuffers();
            OnLoad();

            if (_useMultithreading)
            {
                StartRenderingInOtherThread();
            }

            while (!SoftyContext.ReadyToQuit)
            {
                _updateTime.Begin();
                
                OnUpdateFrame(_updateTime);
                SoftyRenderer.UpdateTriangleBuffer();
                RenderFrame(_updateTime, false);

                _updateTime.End();
            }
            
            SoftyContext.ReleaseDevice();
        }

        private async void StartRenderingInOtherThread()
        {
            await Task.Run(() =>
            {
                while (!SoftyContext.ReadyToQuit)
                {
                    _renderTime.Begin();
                    RenderFrame(_renderTime, true);
                    _renderTime.End();
                }
            });
        }

        private void RenderFrame(Time.Time time, bool inOtherThread)
        {
            if (_useMultithreading && !inOtherThread)
            {
                return;
            }
            
            SoftyContext.UnlockDoubleBuffer();

            OnRenderFrame(time);

            SoftyContext.LockDoubleBuffer();
        }


        protected abstract void OnLoad();
        
        protected abstract void OnUpdateFrame(Time.Time time);
        
        protected abstract void OnRenderFrame(Time.Time time);
    }
}