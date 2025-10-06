using System.Collections.Generic;
using SDL2;

namespace StartButtonLauncher.Models
{
    public class StartButtonLauncherSettings : ObservableObject
    {
        private bool focusBeforeFullscreen = true;
        private SDL.SDL_GameControllerButton selectedButton = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE;

        public bool FocusBeforeFullscreen
        {
            get => this.focusBeforeFullscreen;
            set => this.SetValue(ref this.focusBeforeFullscreen, value);
        }

        public SDL.SDL_GameControllerButton SelectedButton
        {
            get => this.selectedButton;
            set => this.SetValue(ref this.selectedButton, value);
        }

        public void CopyFrom(StartButtonLauncherSettings other)
        {
            this.SelectedButton = other.SelectedButton;
            this.FocusBeforeFullscreen = other.FocusBeforeFullscreen;
        }
    }
}