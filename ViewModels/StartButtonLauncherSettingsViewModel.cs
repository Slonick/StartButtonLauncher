using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Playnite.SDK;
using Playnite.SDK.Data;
using SDL2;
using StartButtonLauncher.Helpers;
using StartButtonLauncher.Models;
using StartButtonLauncher.Services;

namespace StartButtonLauncher.ViewModels
{
    public class StartButtonLauncherSettingsViewModel : ObservableObject, ISettings
    {
        private static readonly ILogger Logger = LogManager.GetLogger(nameof(StartButtonLauncher));
        
        private readonly SDL.SDL_GameControllerButton[] allowedButtons =
        {
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE,
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START,
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK,
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A,
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B,
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X,
            SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y
        };

        private readonly GamepadService gamepadService;
        private readonly StartButtonLauncher plugin;

        private StartButtonLauncherSettings editingClone;

        public StartButtonLauncherSettingsViewModel(StartButtonLauncher plugin, GamepadService gamepadService)
        {
            this.plugin = plugin;
            this.gamepadService = gamepadService;

            this.ButtonLogs = new ObservableCollection<SDL.SDL_GameControllerButton>();
            var savedSettings = plugin.LoadPluginSettings<StartButtonLauncherSettings>();
            this.Settings = savedSettings ?? new StartButtonLauncherSettings();
        }

        public ObservableCollection<SDL.SDL_GameControllerButton> ButtonLogs { get; }

        public StartButtonLauncherSettings Settings { get; }

        public void BeginEdit()
        {
            this.ButtonLogs.Clear();
            this.gamepadService.IsSuspended = true;
            this.gamepadService.ButtonPressed += this.OnButtonPressed;

            this.editingClone = Serialization.GetClone(this.Settings);
        }

        public void CancelEdit()
        {
            this.gamepadService.IsSuspended = false;
            this.gamepadService.ButtonPressed -= this.OnButtonPressed;

            this.Settings.CopyFrom(this.editingClone);
        }

        public void EndEdit()
        {
            this.gamepadService.IsSuspended = false;
            this.gamepadService.ButtonPressed -= this.OnButtonPressed;

            this.plugin.SavePluginSettings(this.Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();
            return true;
        }

        private void OnButtonPressed(SDL.SDL_GameControllerButton button)
        {
            if (this.allowedButtons.Contains(button))
            {
                UIDispatcher.Invoke(() => this.ButtonLogs.Insert(0, button));
            }

            Logger.Debug($"Button pressed: {button}");
        }
    }
}