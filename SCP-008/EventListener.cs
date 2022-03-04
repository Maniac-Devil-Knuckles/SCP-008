using System;
using System.Collections.Generic;
using MEC;
using Qurre;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;

namespace SCP_008
{
    public class EventHandlers
    {

        public readonly Config CustomConfig;
        internal EventHandlers(Config CustomConfig) => this.CustomConfig = CustomConfig;

        public void OnRoundStart()
        {
            if (CustomConfig.CassieAnnounce && !string.IsNullOrEmpty(CustomConfig.Announcement)) Cassie.Send(CustomConfig.Announcement, instant: true);
        }

        public void OnJoin(JoinEvent ev)
        {
            ev.Player.SendConsoleMessage(CustomConfig.SendConsoleMessageOnJoin, "yellow");
        }

        public void OnDamage(DamageEvent ev)
        {
            if (ev.Target.Tag.Contains(" scp343-knuckles") || ev.Target.Tag.Contains(" scp035")) return;
            if (ev.Attacker.Role == RoleType.Scp0492 && ev.Attacker.Id != ev.Target.Id)
            {
                ev.Amount = CustomConfig.ZombieDamage;
                ev.Attacker.Ahp +=
                    CustomConfig.Scp008Buff > 0 && ev.Attacker.Ahp + CustomConfig.Scp008Buff < CustomConfig.MaxAhp
                        ? CustomConfig.Scp008Buff
                        : 0;
                var chance = Extensions.Random.Next(1, 100);
                if (chance >= CustomConfig.InfectionChance ||
                    ev.Target.GetEffect(EffectType.Poisoned).IsEnabled) return;
                ev.Target.EnableEffect(EffectType.Poisoned);
                ev.Target.ShowHint($"<color=yellow><b>SCP-008</b></color>\n{CustomConfig.InfectionAlert}");
                Scp008(ev.Target).RunCoroutine("scp-008" + ev.Target.UserId);
            }

            if (ev.Target.Role == RoleType.Scp0492 && ev.Target.Id != ev.Attacker.Id && ev.Target.Ahp >= 0)
            {
                ev.Allowed = false;
                if (ev.Target.Ahp<= ev.Amount)
                {
                    var leftover = ev.Amount - ev.Target.Ahp;
                    ev.Target.Ahp = 0;
                    ev.Target.Damage(leftover, $"Hit by {ev.Attacker.DisplayNickname}");
                }
                else
                {
                    ev.Target.Ahp -= ev.Amount;
                }
            }
        }

        public void OnUsingItem(ItemUsedEvent ev)
        {
            if (!ev.Player.GetEffect(EffectType.Poisoned).IsEnabled) return;

            var chance = Extensions.Random.Next(1, 100);
            switch (ev.Item.TypeId)
            {
                case ItemType.Medkit:
                    if (chance > CustomConfig.CureChance)
                    {
                        ev.Player.DisableEffect(EffectType.Poisoned);
#if DEBUG
                        Log.Info($"{ev.Player.Nickname} cured themselves with {chance}% probability.");
#endif
                        return;
                    }
#if DEBUG
                    Log.Info($"{ev.Player.Nickname} failed to cure themselves with {chance}% probability.");
#endif
                    break;
                case ItemType.SCP500:
                    ev.Player.DisableEffect(EffectType.Poisoned);
#if DEBUG
                    Log.Info($"{ev.Player.Nickname} cured themselves with SCP-500.");
#endif
                    break;
            }
        }

        public void OnChangeRole(RoleChangeEvent ev)
        {
            Timing.CallDelayed(0.6f, () =>
            {
                if (ev.NewRole == RoleType.Scp0492)
                {
                    if (ev.Player.GetEffect(EffectType.Scp207).IsEnabled) ev.Player.DisableEffect(EffectType.Scp207);
                    ev.Player.Hp = CustomConfig.ZombieHealth;
                    ev.Player.Ahp = CustomConfig.StartingAhp;
                }
                else if (ev.NewRole.GetTeam() != Team.SCP)
                {
                    ev.Player.Ahp = 0;
                }
            });
        }

        public void OnStarRecall(StartRecallEvent ev)
        {
            if (!CustomConfig.BuffDoctor || ev.Target.Team != Team.RIP) return;
            ev.Allowed = false;

            ev.Target.SetRole(RoleType.Scp0492); 
            Timing.CallDelayed(0.4f, () =>
            {
                ev.Target.Position = ev.Scp049.Position;
                ev.Target.Hp = CustomConfig.ZombieHealth;
                ev.Target.Ahp = CustomConfig.StartingAhp;
            });
        }

        public void OnDying(DiesEvent ev)
        {
            if (ev.Target.Team != Team.RIP && ev.Target.GetEffect(EffectType.Poisoned).IsEnabled)
            {
                ev.Allowed = false;
                ev.Target.DisableEffect(EffectType.Poisoned);
                ev.Target.DropItems();
            }
        }

        public IEnumerator<float> Scp008(Player player)
        {
            yield return 1f;
            for (;player.GetEffect(EffectType.Poisoned).IsEnabled;)
            {
                Timing.WaitForSeconds(CustomConfig.WaitTimeBeforeDamageScp008);
                if (player.Hp - CustomConfig.ZombieDamage > 0) player.Damage(CustomConfig.ZombieDamage, "Hit by scp-008");
                else
                {
                    player.DisableEffect(EffectType.Poisoned);
                    player.DropItems();
                    Vector3 position = player.Position;
                    player.ChangeBody(RoleType.Scp0492,false, position, player.Rotation);
                    yield return Timing.WaitForSeconds(0.5f);
                    player.Hp = CustomConfig.ZombieHealth;
                    player.Ahp = CustomConfig.StartingAhp;
                    yield break;
                }
            }
        }
    }
}
