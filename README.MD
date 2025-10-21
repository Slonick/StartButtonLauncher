# 🎮 StartButtonLauncher (Playnite Plugin)

**StartButtonLauncher** is a [Playnite](https://playnite.link/) plugin that lets you start **Fullscreen Mode** by pressing a configurable button on your gamepad.

## ✨ Features

- **Customizable Button**: Choose which button launches Fullscreen Mode (Guide, Start, Back, A, B, X, or Y).
- **SDL2-Based**: Works with a wide range of gamepad types, not just XInput controllers.
- **Configurable Focus Behavior**: Optionally bring Playnite Desktop to the foreground before switching to Fullscreen Mode.
- **Live Button Testing**: Test button presses directly in the settings to verify your configuration.
- **Multi-Language Support**: Includes localizations for English, German, French, and Spanish.
- Hides the splash screen for a smoother startup.
- Displays in-app notifications if something goes wrong (e.g., Playnite executable not found).
- Lightweight and efficient background monitoring.

## 🔧 Installation

1. [Download the latest `.pext`](https://github.com/Slonick/StartButtonLauncher/releases/latest/download/StartButtonLauncher.pext).
2. Open the `.pext` file — Playnite will install the plugin automatically.
3. Restart Playnite.

The plugin should now appear in the extensions list.

## ⚙️ Configuration

1. Open Playnite **Desktop Mode**.
2. Go to **Settings** → **Extensions** → **StartButtonLauncher**.
3. Select your preferred button from the dropdown menu.
4. Optionally enable **"Focus Playnite before fullscreen"** to ensure Playnite is in the foreground before launching Fullscreen Mode.
5. Test your button configuration using the live input tester.

**Note**: The Guide/Xbox button may not always be detected depending on your controller and operating system. Please test if it works with your setup.

## 🎮 Supported Controllers

Thanks to SDL2, this plugin supports a wide variety of gamepads including:
- Xbox controllers (360, One, Series X|S)
- PlayStation controllers (DualShock 4, DualSense)
- Nintendo Switch Pro Controller
- Steam Controller
- And many other DirectInput and XInput devices

## 📝 Changelog

### Version 0.4.0+
- Replaced XInput with SDL2 for broader gamepad compatibility
- Added configurable button selection
- Added settings UI with live button testing
- Added multi-language support (English, German, French, Spanish)
- Added option to focus Playnite before switching to Fullscreen Mode

### Earlier versions
- Initial release with XInput support
- Start button monitoring
- Automatic Fullscreen Mode launch
