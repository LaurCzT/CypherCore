CypherCore is an open source server project for World of Warcraft written in C#.

This branch (`WipClassic`) retargets it to **WoW Classic Era 1.14.0, client build 40618**.
Upstream CypherCore targets 3.4.3.54261.

### Prerequisites
* [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
* [MariaDB 10.6 or higher](https://mariadb.org/download/)
* Optional: Visual Studio 2022, Visual Studio Code or Jetbrains Rider
* Optional: [boost_1_78_0](https://www.boost.org/releases/1.78.0/) (for TrinityCore extractors compiling, This is a proven version)
* Optional: [CMake 3.31.4](https://cmake.org/download/) (for TrinityCore extractors compiling, This is a proven version)
* Optional: [MySQL Server 8.0.35](https://downloads.mysql.com/archives/community/) (instead of MariaDB, This is a proven version)

### Server Setup
* ~~Download and Complie the Extractor [Download](https://github.com/CypherCore/Tools)~~ Use TrinityCore extractors for now: [Project for compilation](https://github.com/TrinityCoreLegacy/TrinityCore/tree/3.4.3)
* Run all extractors in the wow directory
* Copy all created folders into server directory (ex: C:\CypherCore\Data)
* Make sure Conf files are updated and point the the correct folders

### Installing the database
* Download the full Trinity Core database [(TDB_full_343.24081_2024_08_17)](https://github.com/TrinityCore/TrinityCore/releases/tag/TDB343.24081)
* Extract the sql files into the core sql folder (ex: C:\CypherCore\sql)
* Make sure Conf files are updated and point the the correct folders and sql user and databases

### Playing
* Must use [Arctium WoW Client Launcher](https://arctium.io/wow)
* Create link with next parameters (example for Windows): "<path>\World of Warcraft Classic\Arctium WoW Launcher.exe" --version=Classic
* Modify your "<path>\World of Warcraft\_classic_\WTF\Config.wtf"  ->  SET portal "127.0.0.1"

### Account creating
* To create your account:
    - Type: bnetaccount create
    - Example: bnetaccount create test@test test
* To set your account level:
    - Type: account set gmlevel <user#realm> 3 -1
    - Example: account set gmlevel 1#1 3 -1
* Note1:
    - The username used for setting your gmlevel is not the same as the username you create with bnetaccount.
    - You must manually find the username in auth.account.username. These are formatted as 1#1, 2#1, etc.
* Note2:
    - if you have connected before using this command you will need to relog.

### Support / General Info
* Check out our [Discord](https://discord.gg/3skVwCay7z)
* Check out [Trinity Core Wiki](https://trinitycore.atlassian.net/wiki/spaces/tc/pages/2130077/Installation+Guide) as a few steps are the same
* The project is currently under development and a lot of things have not been implemented. Updated according to updates in the appropriate branch of [TrinityCore](https://github.com/TrinityCoreLegacy/TrinityCore/tree/3.4.3)

### Notes
* The version of the Mmap/Vmap/etc extractor itself must support the current client version. The version of the extractor's output files must match the version of the processor for these files in the solution. Unfortunately, when working with outdated TC branches, the supported version of the extractor can only be determined from the commit history. To avoid errors, simply follow Server Setup section.
* To run the emulator in debug mode, you need to configure configuration files directly in the final build folder.
* It is recommended to reassign the path to the SQL files to a trusted folder whose contents will not change each time you work with different branches in version control systems (e.g. Git) to avoid database corruption due to automatic DB updates. And copy updates from the solution manually to the trusted folder (or control the automatic update settings in the configuration files in the all final build folders of all branches of the solution).

### Legal
* Blizzard, Battle.net, World of Warcraft, and all associated logos and designs are trademarks or registered trademarks of Blizzard Entertainment.
* All other trademarks are the property of their respective owners. This project is **not** affiliated with Blizzard Entertainment or any of their family of sites.

### Classic Era 1.14.0 fork - current status

This branch speaks the 1.14.0 protocol **natively**; there is no proxy at runtime. The retail
packet layouts and update-field structures are replaced by a 1.14 adapter layer.

#### Where the 1.14 code lives

`Source/Game/Networking/Adapters/V1_14_0/`

| File | Responsibility |
| --- | --- |
| `UpdateObjectBuilder1140.cs` | create / values blocks, and the ActivePlayer block (the action bar rides inside it) |
| `UpdateFields1140.cs` | 1.14 update-field offsets |
| `UpdateFieldsMapper1140.cs` | maps live object state into the flat field array |
| `UpdateFieldsArray1140.cs`, `UpdateMask1140.cs` | mask + value serialisation |
| `InventorySlots1140.cs` | inbound client -> internal inventory slot translation |

#### Reference material

Layouts were verified against these rather than guessed, in priority order:

1. **HermesProxy** - builds the modern packets a 1.14 client reads, so it is the ground truth for
   field order and update-field offsets. Note that build 40618 maps to its
   `World/Enums/V2_5_2_39570/Opcode.cs` table, **not** `V1_14_1_40688`; a raw opcode-value diff
   against the wrong table is all noise.
2. **vmangos with 1.14 support** - a different architecture (a 1.12 core plus an in-process
   translation layer), so its packet code is not portable here, but its notes on 1.14 client
   quirks are the best available writeup.

#### Working (verified in game)

* Login and entering the world; movement
* Character appearance, equipped gear and the paper doll
* Quest log, quest text, accepting and abandoning quests
* NPC interaction: gossip, quest givers, vendors (buy **and** sell)
* Inventory: equipping, swapping, splitting, destroying, using items
* Chat (say / yell / channels / whisper)
* Combat, creature death, loot
* Spell casting
* Auction house: browse, search, post
* Action bars, including saved buttons across relogs
* Experience bar and levelling

#### Fixed but not yet verified

* Quest giver `!` / `?` status markers refreshing without leaving view
* Quest objective items landing in the bags, and the quest log counter advancing
* World map exploration / fog of war
* Trainers and professions, gathering (skinning, herbalism, mining) - wire support is in, untested

#### Not implemented / untriaged

* Battlegrounds, guilds, mail, parties, social lists and pets - packet layouts in these areas have
  not been reviewed against 1.14 yet
* An NPC carrying both the Trainer and QuestGiver flags may not open its gossip menu

#### Diagnosing a 1.14 layout problem

Diagnose from the logs and the packet dump first - several layouts that *look* wrong are correct.

* `Build/<cfg>/AnyCPU/Logs/WorldServer.log` - set `Logger.Network = 2` to log every packet sent
* `Build/<cfg>/AnyCPU/Logs/CypherCore.pkt` - a full PKT 3.1 dump of both directions. This is the
  fastest way to confirm whether a value actually reached the client, and the header records the
  client build.
* `CMSG_OBJECT_UPDATE_FAILED` arriving from the client means it could not parse an
  `SMSG_UPDATE_OBJECT`; the block before it is the malformed one.

The recurring bug shape is a **retail field or bit that 1.14 does not send**: extra presence bits,
wider bit counts (chat text length 11 vs 9, spell target flags 28 vs 26, `ChatFlags` 15 vs 14),
fields widened from 32 to 64 bits, and retail-only trailing fields. The second recurring shape is
a **legitimate value of zero being mistaken for "no change"**, which silently drops it from the
update mask.

#### Database

The world database is a CypherCore/TrinityCore-schema Classic database. Two retail behaviours in
that data are wrong for Classic and are overridden in code rather than relied upon:

* `quest_objectives.Flags2` bit 1 (`QuestBoundItem`) is stripped at load. In retail a quest item is
  tracked in the quest log and never enters the bags; in Classic it is a real item and the client
  counts what it finds there.
* Vanilla and CypherCore disagree on `creature_template.npcflag` bit values, so a database
  converted from a vanilla source needs those translated.

