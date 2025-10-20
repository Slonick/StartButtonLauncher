using System;
using System.Globalization;
using SDL2;
using StartButtonLauncher.Converters.Base;

namespace StartButtonLauncher.Converters
{
    public class SdlButtonToStringConverter : BaseValueConverter<SdlButtonToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SDL.SDL_GameControllerButton button)
            {
                switch (button)
                {
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A:
                        return "A Button";
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B:
                        return "B Button";
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X:
                        return "X Button";
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y:
                        return "Y Button";
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE:
                        return "Guide / Xbox Button";
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START:
                        return "Start / Options";
                    case SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK:
                        return "Back / Select";
                    default:
                        return button.ToString();
                }
            }

            return "Unknown";
        }
    }
}