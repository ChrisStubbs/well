using System.Diagnostics;

namespace PH.Well.Domain.ValueObjects
{
    [DebuggerDisplay("{Server}:{Port} @{Rfs} {Username}:{Password} {EmailAddress}")]
    public class AdamSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Rfs { get; set; }
        public int Port { get; set; }
        public string EmailAddress { get; set; }
    }
}
