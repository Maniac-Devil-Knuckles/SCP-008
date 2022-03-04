# SCP-008
This Qurre plugin for SCP:SL that adds SCP-008 into the game. This is fundamentally for server hosts that want to add more a enganging SCP-049-2 experience for their players.

It will give *`SCP-049-2`* the ability to infect it's targets on hit. The targets will receive a custom component that drains their health over time. In order to cure the infection, you must either use `SCP-500` for a guaranteed success or gamble with a `Medkit`'s 50% chance cure rate. Players that die while infected will spawn as SCP-049-2 as well.

## Config in %appdata%\Qurre\Configs\Custom\SCP008-7777.yaml on Windows or in ~/.config/Qurre/Configs/Custom/SCP008-7777.yaml 
Config : 
```yaml
Name: SCP008
IsEnabled: true
InfectionChance: 100
CureChance: 50
# Allow SCP-049 to instantly revive targets?
BuffDoctor: false
ZombieHealth: 300
# How much AHP should be given to Zombies?
Scp008Buff: 10
# How much AHP should zombies spawn with?
StartingAhp: 100
# How much AHP should zombies stop earning at?
MaxAhp: 100
CassieAnnounce: true
Announcement: SCP 0 0 8 containment breach detected . Allremaining
ZombieDamage: 24
# Text displayed to players after they've been infected
InfectionAlert: You've been infected! Use SCP-500 or a medkit to be cured!
SendConsoleMessageOnJoin: This server uses SCP-008-X, all zombies have been reworked!
# Wait n seconds to damage human
WaitTimeBeforeDamageScp008: 2
```
