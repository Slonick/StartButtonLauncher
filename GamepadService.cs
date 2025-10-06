using System;
using System.Threading;
using Playnite.SDK;
using SDL2;

namespace StartButtonLauncher
{
    public class GamepadService : IDisposable
    {
        private static readonly ILogger Logger = LogManager.GetLogger(nameof(StartButtonLauncher));

        public event Action<SDL.SDL_GameControllerButton> ButtonPressed;
        public event Action<SDL.SDL_GameControllerButton> ButtonReleased;
        private IntPtr controller = IntPtr.Zero;
        private bool running;

        private Thread thread;

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
        {
            if (this.running)
            {
                return;
            }

            if (SDL.SDL_Init(SDL.SDL_INIT_GAMECONTROLLER) < 0)
            {
                throw new Exception("SDL init failed: " + SDL.SDL_GetError());
            }

            SDL.SDL_SetHint(SDL.SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS, "1");

            this.running = true;
            this.thread = new Thread(this.EventLoop) { IsBackground = true };
            this.thread.Start();

            Logger.Info("GamepadService started.");
        }

        public void Stop()
        {
            if (!this.running)
            {
                return;
            }

            this.running = false;
            this.thread?.Join();

            if (this.controller != IntPtr.Zero)
            {
                SDL.SDL_GameControllerClose(this.controller);
                this.controller = IntPtr.Zero;
            }

            SDL.SDL_Quit();
            Logger.Info("GamepadService stopped and SDL cleaned up.");
        }

        private void EventLoop()
        {
            try
            {
                while (this.running)
                {
                    while (SDL.SDL_PollEvent(out var e) != 0)
                    {
                        switch (e.type)
                        {
                            case SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                                this.HandleControllerAdded(e.cdevice.which);
                                break;

                            case SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                                this.HandleControllerRemoved(e.cdevice.which);
                                break;

                            case SDL.SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
                                var pressed = (SDL.SDL_GameControllerButton)e.cbutton.button;
                                Logger.Debug($"Button down: {pressed}");
                                this.ButtonPressed?.Invoke(pressed);
                                break;

                            case SDL.SDL_EventType.SDL_CONTROLLERBUTTONUP:
                                var released = (SDL.SDL_GameControllerButton)e.cbutton.button;
                                Logger.Debug($"Button up: {released}");
                                this.ButtonReleased?.Invoke(released);
                                break;
                        }
                    }

                    Thread.Sleep(16);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unhandled exception in GamepadService loop.");
            }
            finally
            {
                Logger.Info("GamepadService stopped.");
            }
        }

        private void HandleControllerAdded(int deviceIndex)
        {
            if (this.controller != IntPtr.Zero)
            {
                return;
            }

            this.controller = SDL.SDL_GameControllerOpen(deviceIndex);
            if (this.controller != IntPtr.Zero)
            {
                var name = SDL.SDL_GameControllerName(this.controller);
                Logger.Info($"Controller connected: {name}");
            }
            else
            {
                Logger.Warn($"Failed to open controller index {deviceIndex}: {SDL.SDL_GetError()}");
            }
        }

        private void HandleControllerRemoved(int instanceId)
        {
            if (this.controller == IntPtr.Zero)
            {
                return;
            }

            var currentId = SDL.SDL_JoystickInstanceID(SDL.SDL_GameControllerGetJoystick(this.controller));
            if (currentId == instanceId)
            {
                SDL.SDL_GameControllerClose(this.controller);
                this.controller = IntPtr.Zero;
                Logger.Info("Controller disconnected.");
            }
        }
    }
}