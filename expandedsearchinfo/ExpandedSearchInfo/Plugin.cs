using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ExpandedSearchInfo {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Plugin : IDalamudPlugin {
        public string Name => "Expanded Search Info";

        [PluginService]
        internal DalamudPluginInterface Interface { get; init; };

        [PluginService]
        internal CommandManager CommandManager { get; init; };

        [PluginService]
        internal GameGui GameGui { get; init; };

        [PluginService]
        internal ObjectTable ObjectTable { get; init; };

        [PluginService]
        internal SigScanner SigScanner { get; init; };

        internal PluginConfiguration Config { get; init; }
        internal GameFunctions Functions { get; }
        internal SearchInfoRepository Repository { get; }
        private PluginUi Ui { get; }

        public Plugin() {
            this.Config = this.Interface!.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            this.Config.Initialise(this);

            this.Functions = new GameFunctions(this);
            this.Repository = new SearchInfoRepository(this);
            this.Ui = new PluginUi(this);

            this.CommandManager.AddHandler("/esi", new CommandInfo(this.OnCommand) {
                HelpMessage = "Toggles Expanded Search Info's configuration window",
            });
        }

        public void Dispose() {
            this.CommandManager.RemoveHandler("/esi");
            this.Ui?.Dispose();
            this.Repository?.Dispose();
            this.Functions?.Dispose();
        }

        private void OnCommand(string command, string arguments) {
            this.Ui.ConfigVisible = !this.Ui.ConfigVisible;
        }
    }
}
