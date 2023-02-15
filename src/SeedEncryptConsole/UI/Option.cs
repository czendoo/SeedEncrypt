using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncryptConsole.UI
{
    public class Option
    {
        public object? Id { get; set; }

        public string Name { get; }

        Action? _action;
        public Action Action
        {
            get => _action ?? (() => { });
            set => _action = value;
        }

        Func<bool>? _enabled;
        public Func<bool> Enabled
        {
            get => _enabled ?? (() => true);
            set => _enabled = value;
        }

        Func<bool>? _confirm;
        public Func<bool> Confirm
        {
            get => _confirm ?? (() => false);
            set => _confirm = value;
        }

        public bool IsInvoked { get; protected set; }

        public Option(string name, object? id = null)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
