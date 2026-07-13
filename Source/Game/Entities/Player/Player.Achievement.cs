using Framework.Constants;
using Framework.Database;
using Game.Networking;
using Game.Networking.Packets;

using System;
using System.Collections.Generic;

namespace Game.Entities
{
    public partial class Player
    {
        public void ResetAchievements() { }
        public void SendRespondInspectAchievements(Player player) { }
        public void StartCriteria(CriteriaStartEvent startEvent, int entry, TimeSpan timeLost = default) { }
        public void FailCriteria(CriteriaFailEvent failEvent, int failAsset) { }
        public void UpdateCriteria(CriteriaType type, long miscValue1 = 0, long miscValue2 = 0, long miscValue3 = 0, WorldObject refe = null) { }
        public void CompletedAchievement(object entry) { }
    }
}
namespace Game.Entities { public partial class Player { public bool HasAchieved(int achievementId) { return false; } public int GetAchievementPoints() { return 0; } public bool ModifierTreeSatisfied(int modifierTreeId) { return false; } } }
