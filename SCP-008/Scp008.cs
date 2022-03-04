using System;
using Qurre;
using Qurre.Events;

namespace SCP_008
{
    public class SCP008 : Plugin
    {
        public override string Developer { get; } = "Maniac Devil Knuckles#8455";

        public override string Name { get; } = "SCP-008";

        public override Version NeededQurreVersion { get; } = new Version(1, 12);

        public override Version Version { get; } = new Version(1, 0, 0);
        
        public override int Priority { get; } = int.MaxValue;

        public static Config CustomConfig { get; internal set; } = new Config();

        internal static EventHandlers EventHandlers { get; private set; }

        public override void Enable()
        {
            CustomConfigs.Add(CustomConfig);
            if (!CustomConfig.IsEnabled)
            {
                Log.Info("Scp-008 is disabled!");
                return;
            }

            EventHandlers = new EventHandlers(CustomConfig);

            Player.Damage += EventHandlers.OnDamage;
            Round.Start += EventHandlers.OnRoundStart;
            Player.Join += EventHandlers.OnJoin;
            Player.ItemUsed += EventHandlers.OnUsingItem;
            Player.RoleChange += EventHandlers.OnChangeRole;
            Scp049.StartRecall += EventHandlers.OnStarRecall;
            Player.Dies += EventHandlers.OnDying;
        }

        public override void Disable()
        {
            Player.Damage -= EventHandlers.OnDamage;
            Round.Start -= EventHandlers.OnRoundStart;
            Player.Join -= EventHandlers.OnJoin;
            Player.ItemUsed -= EventHandlers.OnUsingItem;
            Player.RoleChange -= EventHandlers.OnChangeRole;
            Scp049.StartRecall -= EventHandlers.OnStarRecall;
            Player.Dies -= EventHandlers.OnDying;
        }

    }
}
