using System;

namespace AppLuncher.Models
{
    public sealed class LaunchAction
    {
        public LaunchAction()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string ProgramPath { get; set; }

        public string Arguments { get; set; }

        public string WorkingDirectory { get; set; }

        public bool WaitForExit { get; set; }

        public int DelayAfterMs { get; set; }

        public int Order { get; set; }
    }
}
