using System.Collections.Generic;
using Playnite.SDK;
using Playnite.SDK.Data;
using StartButtonLauncher.Models;

namespace StartButtonLauncher.ViewModels
{
    public class StartButtonLauncherSettingsViewModel : ObservableObject, ISettings
    {
        private readonly StartButtonLauncher plugin;
        private StartButtonLauncherSettings editingClone;

        public StartButtonLauncherSettingsViewModel(StartButtonLauncher plugin)
        {
            this.plugin = plugin;

            var savedSettings = plugin.LoadPluginSettings<StartButtonLauncherSettings>();
            this.Settings = savedSettings ?? new StartButtonLauncherSettings();
        }

        public StartButtonLauncherSettings Settings { get; }

        public void BeginEdit()
        {
            this.editingClone = Serialization.GetClone(this.Settings);
        }

        public void CancelEdit()
        {
            this.Settings.CopyFrom(this.editingClone);
        }

        public void EndEdit()
        {
            this.plugin.SavePluginSettings(this.Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();
            return true;
        }
    }
}