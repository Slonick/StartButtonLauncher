using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StartButtonLauncher
{
    public class StartButtonLauncher : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        private CancellationTokenSource cts;

        public override Guid Id => Guid.Parse("80e02e7f-57cd-43f0-b501-45d5ce16f10f");

        public StartButtonLauncher(IPlayniteAPI playniteAPI) : base(playniteAPI) 
        {
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            logger.Info("StartButtonPlugin started.");
            this.cts = new CancellationTokenSource();
            Task.Run(() => PollGamepad(cts.Token));
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            this.cts?.Cancel();
        }

        private async Task PollGamepad(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                
                if (XInput.GetState(0, out var state))
                {
                    bool isStartPressed = state.Gamepad.wButtons.HasFlag(XInputButtons.Start);

                    if (isStartPressed && this.PlayniteApi.ApplicationInfo.Mode != ApplicationMode.Fullscreen)
                    {
                        if (PlayniteApi.Database.Games.All(g => g.IsRunning == false && g.IsRunning == false))
                        {
                            this.LaunchFullscreen();
                        }
                    }
                }

                await Task.Delay(100, token);
            }
        }

        private void LaunchFullscreen()
        {
            try
            {
                var fullscreenExe = Path.Combine(PlayniteApi.Paths.ApplicationPath, "Playnite.FullscreenApp.exe");

                if (!File.Exists(fullscreenExe))
                {
                    PlayniteApi.Notifications.Add(new NotificationMessage(
                        "fullscreen_not_found",
                        $"Fullscreen executable not found: {fullscreenExe}",
                        NotificationType.Error));
                    return;
                }

                var psi = new ProcessStartInfo
                {
                    FileName = fullscreenExe,
                    Arguments = "--startfullscreen --hidesplashscreen",
                    UseShellExecute = false,
                    WorkingDirectory = PlayniteApi.Paths.ApplicationPath
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                PlayniteApi.Notifications.Add(new NotificationMessage(
                    "fullscreen_launch_error",
                    $"Error launching fullscreen: {ex.Message}",
                    NotificationType.Error));
            }
        }
    }
}
