using System.Collections.Generic;
using SDL2;

namespace StartButtonLauncher.Helpers
{
    public static class ButtonOptions
    {
        public static List<ButtonOption> All { get; } = new List<ButtonOption>
        {
            new ButtonOption("Guide", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE),
            new ButtonOption("Start", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START),
            new ButtonOption("Back", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK),
            new ButtonOption("A", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A),
            new ButtonOption("B", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B),
            new ButtonOption("X", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X),
            new ButtonOption("Y", SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y)
        };
    }

    public class ButtonOption
    {
        public ButtonOption(string label, SDL.SDL_GameControllerButton value)
        {
            this.Label = label;
            this.Value = value;
        }

        public string Label { get; }
        public SDL.SDL_GameControllerButton Value { get; }
    }
}