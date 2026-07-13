using Game.Entities;
using Framework.Constants;

namespace Game.Networking.Adapters.V1_14_0
{
    /// <summary>
    /// Maps CypherCore's modern structured update fields (UnitData, PlayerData, etc.)
    /// to the 1.14.0 flat integer array format expected by the Classic Era client.
    /// </summary>
    public static class UpdateFieldsMapper1140
    {
        public static void MapObjectFields(UpdateFieldsArray1140 updateArray, WorldObject obj, Player target)
        {
            // ─── Object fields (common to all types) ───
            var objData = obj.GetObjectData();
            updateArray.SetUpdateField((int)ObjectField.OBJECT_FIELD_GUID, obj.GetGUID());
            updateArray.SetUpdateField((int)ObjectField.OBJECT_FIELD_ENTRY, (int)objData.EntryId.GetValue());
            updateArray.SetUpdateField((int)ObjectField.OBJECT_DYNAMIC_FLAGS, objData.DynamicFlags.GetValue());
            updateArray.SetUpdateField((int)ObjectField.OBJECT_FIELD_SCALE_X, objData.Scale.GetValue());

            // ─── Unit fields ───
            Unit unit = obj.ToUnit();
            if (unit != null)
            {
                var ud = unit.GetUnitData();

                // GUIDs (each occupies 4 uint32 slots in the flat array)
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_CHARM, ud.Charm.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SUMMON, ud.Summon.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_CHARMEDBY, ud.CharmedBy.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SUMMONEDBY, ud.SummonedBy.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_CREATEDBY, ud.CreatedBy.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DEMON_CREATOR, ud.DemonCreator.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_LOOK_AT_CONTROLLER_TARGET, ud.LookAtControllerTarget.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_TARGET, ud.Target.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BATTLE_PET_COMPANION_GUID, ud.BattlePetCompanionGUID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BATTLE_PET_DB_ID, (ulong)ud.BattlePetDBID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SUMMONED_BY_HOME_REALM, ud.SummonedByHomeRealm.GetValue());

                // UNIT_FIELD_BYTES_0: Race | ClassId<<8 | PlayerClassId<<16 | Sex<<24
                // (The display power is separate in 1.14.0)
                uint bytes0 = (uint)(byte)ud.Race | ((uint)(byte)ud.ClassId << 8) | ((uint)(byte)ud.PlayerClassId << 16) | ((uint)(byte)ud.Sex << 24);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BYTES_0, bytes0);

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DISPLAY_POWER, (uint)(byte)ud.DisplayPower);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_OVERRIDE_DISPLAY_POWER_ID, (uint)ud.OverrideDisplayPowerID.GetValue());

                // Health (1.14.0 uses 2 uint32 slots = int64)
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_HEALTH, (ulong)ud.Health.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MAXHEALTH, (ulong)ud.MaxHealth.GetValue());

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_LEVEL, (uint)ud.Level.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_EFFECTIVE_LEVEL, (uint)ud.EffectiveLevel.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_CONTENT_TUNING_ID, (uint)ud.ContentTuningID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_LEVEL_MIN, (uint)ud.ScalingLevelMin.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_LEVEL_MAX, (uint)ud.ScalingLevelMax.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_LEVEL_DELTA, (uint)ud.ScalingLevelDelta.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_FACTION_GROUP, (uint)ud.ScalingFactionGroup.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FACTIONTEMPLATE, (uint)ud.FactionTemplate.GetValue());

                // Flags
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FLAGS, ud.Flags.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FLAGS_2, ud.Flags2.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FLAGS_3, ud.Flags3.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_AURASTATE, ud.AuraState.GetValue());

                // Attack times
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BOUNDINGRADIUS, ud.BoundingRadius.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_COMBATREACH, ud.CombatReach.GetValue());

                // Display
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DISPLAYID, (uint)ud.DisplayID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DISPLAY_SCALE, ud.DisplayScale.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_NATIVEDISPLAYID, (uint)ud.NativeDisplayID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_NATIVE_X_DISPLAY_SCALE, ud.NativeXDisplayScale.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOUNTDISPLAYID, (uint)ud.MountDisplayID.GetValue());

                // Combat
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MINDAMAGE, ud.MinDamage.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MAXDAMAGE, ud.MaxDamage.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MINOFFHANDDAMAGE, ud.MinOffHandDamage.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MAXOFFHANDDAMAGE, ud.MaxOffHandDamage.GetValue());

                // UNIT_FIELD_BYTES_1: StandState | PetLoyaltyIndex(unused) | VisFlags | AnimTier
                uint bytes1 = (uint)(byte)ud.StandState | ((uint)(byte)ud.PetTalentPoints << 8) | ((uint)(byte)ud.VisFlags << 16) | ((uint)(byte)ud.AnimTier << 24);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BYTES_1, bytes1);

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_PETNUMBER, (uint)ud.PetNumber.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_MOD_CAST_SPEED, ud.ModCastingSpeed.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_MOD_CAST_HASTE, ud.ModSpellHaste.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_HASTE, ud.ModHaste.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_RANGED_HASTE, ud.ModRangedHaste.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_HASTE_REGEN, ud.ModHasteRegen.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_TIME_RATE, ud.ModTimeRate.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_CREATED_BY_SPELL, (uint)ud.CreatedBySpell.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_NPC_EMOTESTATE, (uint)ud.EmoteState.GetValue());

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BASE_MANA, (uint)ud.BaseMana.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BASE_HEALTH, (uint)ud.BaseHealth.GetValue());

                // UNIT_FIELD_BYTES_2: SheatheState | PvpFlags | PetFlags | ShapeshiftForm
                uint bytes2 = (uint)(byte)ud.SheatheState | ((uint)(byte)ud.PvpFlags << 8) | ((uint)(byte)ud.PetFlags << 16) | ((uint)(byte)ud.ShapeshiftForm << 24);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BYTES_2, bytes2);

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_ATTACK_POWER, (uint)ud.AttackPower.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_ATTACK_POWER_MOD_POS, (uint)ud.AttackPowerModPos.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_ATTACK_POWER_MOD_NEG, (uint)ud.AttackPowerModNeg.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_ATTACK_POWER_MULTIPLIER, ud.AttackPowerMultiplier.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_RANGED_ATTACK_POWER, (uint)ud.RangedAttackPower.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS, (uint)ud.RangedAttackPowerModPos.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG, (uint)ud.RangedAttackPowerModNeg.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER, ud.RangedAttackPowerMultiplier.GetValue());

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MAXHEALTHMODIFIER, ud.MaxHealthModifier.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_HOVERHEIGHT, ud.HoverHeight.GetValue());
            }

            // ─── Player fields (only for Player objects) ───
            Player player = obj.ToPlayer();
            if (player != null)
            {
                // TODO: Map PlayerData and ActivePlayerData fields
                // For now, the minimum needed to not crash the client on login.
            }
        }
    }
}
