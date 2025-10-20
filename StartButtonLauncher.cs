using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using SDL2;
using StartButtonLauncher.Helpers;
using StartButtonLauncher.Services;
using StartButtonLauncher.ViewModels;
using StartButtonLauncher.Views;

namespace StartButtonLauncher
{
    public class StartButtonLauncher : GenericPlugin
    {
        private const string PlayniteTitle = "Playnite.DesktopApp";
        private static readonly ILogger Logger = LogManager.GetLogger();
        private readonly GamepadService gamepadService;
        private readonly StartButtonLauncherSettingsViewModel model;

        public StartButtonLauncher(IPlayniteAPI playniteApi) : base(playniteApi)
        {
            this.gamepadService = new GamepadService();
            this.gamepadService.ButtonPressed += this.OnButtonPressed;

            this.model = new StartButtonLauncherSettingsViewModel(this, this.gamepadService);
            this.Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }

        public override Guid Id => Guid.Parse("80e02e7f-57cd-43f0-b501-45d5ce16f10f");

        public override ISettings GetSettings(bool firstRunSettings) => this.model;

        public override UserControl GetSettingsView(bool firstRunSettings)
            => new StartButtonLauncherSettingsView { DataContext = this.model };

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            Logger.Info("StartButtonPlugin started.");

            if (this.PlayniteApi.ApplicationInfo.Mode == ApplicationMode.Fullscreen)
            {
                return;
            }

            this.gamepadService.Start();
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            this.gamepadService.Stop();
        }

        private void LaunchPlaynite(bool fullscreen)
        {
            try
            {
                var fullscreenExe = Path.Combine(this.PlayniteApi.Paths.ApplicationPath, "Playnite.DesktopApp.exe");

                if (!File.Exists(fullscreenExe))
                {
                    this.PlayniteApi.Notifications.Add(new NotificationMessage("executable_not_found",
                                                                               $"Executable not found: {fullscreenExe}",
                                                                               NotificationType.Error));

                    return;
                }

                var arguments = new List<string> { "--hidesplashscreen" };

                if (fullscreen)
                {
                    arguments.Add("--startfullscreen");
                }

                var psi = new ProcessStartInfo
                {
                    FileName = fullscreenExe,
                    Arguments = string.Join(" ", arguments),
                    UseShellExecute = false,
                    WorkingDirectory = this.PlayniteApi.Paths.ApplicationPath
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                this.PlayniteApi.Notifications.Add(new NotificationMessage("executable_launch_error",
                                                                           $"Error launching executable: {ex.Message}",
                                                                           NotificationType.Error));
            }
        }

        private void OnButtonPressed(SDL.SDL_GameControllerButton button)
        {
            if (!this.gamepadService.IsSuspended && button == this.model.Settings.SelectedButton)
            {
                var isFullscreen = !this.model.Settings.FocusBeforeFullscreen || WindowHelper.IsWindowVisible(PlayniteTitle);
                this.LaunchPlaynite(isFullscreen);
            }
        }
    }
}