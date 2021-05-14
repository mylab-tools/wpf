using System.Collections.Generic;

namespace MyLab.Wpf
{
    public class VmCommandRegistry
    {
        private readonly List<VmCommand> _commands = new List<VmCommand>();

        public void RegisterCommand(VmCommand cmd)
        {
            _commands.Add(cmd);
        }

        public void UpdateStates()
        {
            foreach (var cmd in _commands)
                cmd.OnCanExecuteChanged();
        }
    }
}