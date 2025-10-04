using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;

namespace StartButtonLauncher
{
    public class StartButtonLauncher : GenericPlugin
    {
        private const string PlayniteTitle = "Playnite.DesktopApp";
        private static readonly ILogger logger = LogManager.GetLogger();
        private CancellationTokenSource cts;
        private bool wasStartPressed;

        public StartButtonLauncher(IPlayniteAPI playniteAPI) : base(playniteAPI) { }

        public override Guid Id => Guid.Parse("80e02e7f-57cd-43f0-b501-45d5ce16f10f");

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            logger.Info("StartButtonPlugin started.");
            this.cts = new CancellationTokenSource();
            Task.Run(() => this.PollGamepad(this.cts.Token));
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            this.cts?.Cancel();
        }

        private void LaunchPlaynite(bool fullscreen)
        {
            try
            {
                var fullscreenExe = Path.Combine(this.PlayniteApi.Paths.ApplicationPath, "Playnite.DesktopApp.exe");

                if (!File.Exists(fullscreenExe))
                {
                    this.PlayniteApi.Notifications.Add(new NotificationMessage(
                                                                               "executable_not_found",
                                                                               $"Executable not found: {fullscreenExe}",
                                                                               NotificationType.Error));
                    return;
                }

                var argumanets = new List<string> { "--hidesplashscreen" };

                if (fullscreen)
                {
                    argumanets.Add("--startfullscreen");
                }

                var psi = new ProcessStartInfo
                {
                    FileName = fullscreenExe,
                    Arguments = string.Join(" ", argumanets),
                    UseShellExecute = false,
                    WorkingDirectory = this.PlayniteApi.Paths.ApplicationPath
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                this.PlayniteApi.Notifications.Add(new NotificationMessage(
                                                                           "executable_launch_error",
                                                                           $"Error launching executable: {ex.Message}",
                                                                           NotificationType.Error));
            }
        }

        private async Task PollGamepad(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (XInput.GetState(0, out var state))
                {
                    var isStartPressed = state.Gamepad.wButtons.HasFlag(XInputButtons.Start);

                    if (isStartPressed && !this.wasStartPressed && this.PlayniteApi.ApplicationInfo.Mode != ApplicationMode.Fullscreen)
                    {
                        if (this.PlayniteApi.Database.Games.All(g => !g.IsRunning && !g.IsLaunching))
                        {
                            if (WindowHelper.IsWindowVisible(PlayniteTitle))
                            {
                                this.LaunchPlaynite(true);
                            }
                            else
                            {
                                WindowHelper.ShowWindow(PlayniteTitle);
                            }
                        }
                    }

                    this.wasStartPressed = isStartPressed;
                }

                await Task.Delay(100, token).ConfigureAwait(false);
            }
        }
    }
}