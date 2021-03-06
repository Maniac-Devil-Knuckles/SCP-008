using System.ComponentModel;
using Qurre.API.Addons;

namespace SCP_008
{
    public sealed class Config : IConfig
    {
        public string Name { get; set; } = "SCP008";

        public bool IsEnabled { get; set; } = true;

        public int InfectionChance { get; set; } = 100;
        public int CureChance { get; set; } = 50;

        [Description("Allow SCP-049 to instantly revive targets?")]
        public bool BuffDoctor { get; set; } = false;

        public int ZombieHealth { get; set; } = 300;

        [Description("How much AHP should be given to Zombies?")]
        public ushort Scp008Buff { get; set; } = 10;

        [Description("How much AHP should zombies spawn with?")]
        public ushort StartingAhp { get; set; } = 100;

        [Description("How much AHP should zombies stop earning at?")]
        public ushort MaxAhp { get; set; } = 100;

        public bool CassieAnnounce { get; set; } = true;
        public string Announcement { get; set; } = "SCP 0 0 8 containment breach detected . Allremaining";
        public int ZombieDamage { get; set; } = 24;

        [Description("Text displayed to players after they've been infected")]
        public string InfectionAlert { get; set; } = "You've been infected! Use SCP-500 or a medkit to be cured!";

        public string SendConsoleMessageOnJoin { get; set; } = "This server uses SCP-008-X, all zombies have been reworked!";

        [Description("Wait n seconds to damage human")]
        public float WaitTimeBeforeDamageScp008 { get; set; } = 2f;
    }
}