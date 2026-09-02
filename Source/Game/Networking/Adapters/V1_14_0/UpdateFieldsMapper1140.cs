using Game.Entities;
using Framework.Constants;

namespace Game.Networking.Adapters.V1_14_0
{
    /// <summary>
    /// Maps CypherCore's modern structured update fields (UnitData, PlayerData, Item, Bag, etc.)
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

            // ─── Item fields ───
            Item item = obj as Item;
            if (item != null)
            {
                var id = item.m_itemData;
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_OWNER, id.Owner.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_CONTAINED, id.ContainedIn.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_CREATOR, id.Creator.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_GIFTCREATOR, id.GiftCreator.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_STACK_COUNT, (uint)id.StackCount.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_DURATION, (uint)id.Expiration.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_FLAGS, (uint)id.DynamicFlags.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_DURABILITY, (uint)id.Durability.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_MAXDURABILITY, (uint)id.MaxDurability.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_CREATE_PLAYED_TIME, (uint)id.CreatePlayedTime.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_MODIFIERS_MASK, (uint)0);
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_CONTEXT, (int)id.Context.GetValue());
                updateArray.SetUpdateField((int)ItemField.ITEM_FIELD_APPEARANCE_MOD_ID, (uint)id.ItemAppearanceModID.GetValue());

                // Container / Bag fields
                Bag bag = obj as Bag;
                if (bag != null)
                {
                    updateArray.SetUpdateField((int)ContainerField.CONTAINER_FIELD_NUM_SLOTS, (uint)bag.GetBagSize());
                    for (byte i = 0; i < bag.GetBagSize(); i++)
                    {
                        Item bagItem = bag.GetItemByPos(i);
                        if (bagItem != null)
                        {
                            int startIndex = (int)ContainerField.CONTAINER_FIELD_SLOT_1;
                            updateArray.SetUpdateField(startIndex + i * 4, bagItem.GetGUID());
                        }
                    }
                }
            }

            // ─── GameObject fields ───
            GameObject go = obj.ToGameObject();
            if (go != null)
            {
                var god = go.m_gameObjectData;
                updateArray.SetUpdateField((int)GameObjectField.GAMEOBJECT_DISPLAYID, (uint)god.DisplayID.GetValue());
                updateArray.SetUpdateField((int)GameObjectField.GAMEOBJECT_FLAGS, (uint)god.Flags.GetValue());
                updateArray.SetUpdateField((int)GameObjectField.GAMEOBJECT_PARENTROTATION, god.ParentRotation.GetValue());
                updateArray.SetUpdateField((int)GameObjectField.GAMEOBJECT_FACTION, (uint)god.FactionTemplate.GetValue());
                updateArray.SetUpdateField((int)GameObjectField.GAMEOBJECT_LEVEL, (uint)god.Level.GetValue());

                uint goBytes1 = (uint)(byte)god.State.GetValue() | ((uint)(byte)god.TypeID.GetValue() << 8) | ((uint)(byte)god.ArtKit.GetValue() << 16) | ((uint)(byte)god.PercentHealth.GetValue() << 24);
                updateArray.SetUpdateField((int)GameObjectField.GAMEOBJECT_BYTES_1, goBytes1);
            }

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
                uint bytes0 = (uint)(byte)ud.Race | ((uint)(byte)ud.ClassId << 8) | ((uint)(byte)ud.PlayerClassId << 16) | ((uint)(byte)ud.Sex << 24);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BYTES_0, bytes0);

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DISPLAY_POWER, (uint)(byte)ud.DisplayPower);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_OVERRIDE_DISPLAY_POWER_ID, (uint)ud.OverrideDisplayPowerID.GetValue());

                // Health (1.14.0 uses 2 uint32 slots = int64)
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_HEALTH, (ulong)ud.Health.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MAXHEALTH, (ulong)ud.MaxHealth.GetValue());

                // Powers (Mana, Rage, Focus, Energy, ComboPoints, Runes)
                for (int i = 0; i < 6 && i < ud.Power.GetSize(); ++i)
                {
                    updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_POWER + i, ud.Power[i]);
                    updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MAXPOWER + i, ud.MaxPower[i]);
                    updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_POWER_REGEN + i, 1.0f);
                }

                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_LEVEL, (uint)ud.Level.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_EFFECTIVE_LEVEL, (uint)ud.EffectiveLevel.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_CONTENT_TUNING_ID, (uint)ud.ContentTuningID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_LEVEL_MIN, (uint)ud.ScalingLevelMin.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_LEVEL_MAX, (uint)ud.ScalingLevelMax.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_LEVEL_DELTA, (uint)ud.ScalingLevelDelta.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALING_FACTION_GROUP, (uint)ud.ScalingFactionGroup.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FACTIONTEMPLATE, (uint)ud.FactionTemplate.GetValue());

                // Flags (1.14 client expects UNIT_FIELD_FLAGS_2 to include 2048)
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FLAGS, ud.Flags.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FLAGS_2, ud.Flags2.GetValue() | 2048);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_FLAGS_3, ud.Flags3.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_AURASTATE, ud.AuraState.GetValue());

                // Attack times
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_BOUNDINGRADIUS, ud.BoundingRadius.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_COMBATREACH, ud.CombatReach.GetValue());

                // Display
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DISPLAYID, (uint)ud.DisplayID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_DISPLAY_SCALE, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_NATIVEDISPLAYID, (uint)ud.NativeDisplayID.GetValue());
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_NATIVE_X_DISPLAY_SCALE, 1.0f);
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
                updateArray.SetUpdateField((int)UnitField.UNIT_MOD_CAST_SPEED, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_MOD_CAST_HASTE, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_HASTE, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_RANGED_HASTE, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_HASTE_REGEN, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_MOD_TIME_RATE, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_CREATED_BY_SPELL, (uint)ud.CreatedBySpell.GetValue());

                // UNIT_NPC_FLAGS (Size: 2)
                uint npcFlags0 = (uint)ud.NpcFlags[0];
                uint npcFlags1 = (uint)ud.NpcFlags[1];

                Creature creature = unit.ToCreature();
                if (creature != null)
                {
                    // In modern 1.14 (see vmangos ConvertV2NpcFlags):
                    // If Trainer flag (0x10) is set, 1.14 requires TrainerClass (0x20) or TrainerProfession (0x40).
                    // Without it, the client refuses interaction with ERR_REQUIRES_EXPANSION_S.
                    if ((npcFlags0 & (uint)NPCFlags1.Trainer) != 0)
                    {
                        if ((npcFlags0 & ((uint)NPCFlags1.TrainerClass | (uint)NPCFlags1.TrainerProfession)) == 0)
                        {
                            if (creature.GetCreatureTemplate().TrainerClass != Class.None)
                                npcFlags0 |= (uint)NPCFlags1.TrainerClass;
                            else
                                npcFlags0 |= (uint)NPCFlags1.TrainerProfession;
                        }
                    }
                }

                updateArray.SetUpdateField((int)UnitField.UNIT_NPC_FLAGS, npcFlags0);
                updateArray.SetUpdateField((int)UnitField.UNIT_NPC_FLAGS + 1, npcFlags1);

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
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_HOVERHEIGHT, 1.0f);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_SCALE_DURATION, (uint)100);
                updateArray.SetUpdateField((int)UnitField.UNIT_FIELD_LOOK_AT_CONTROLLER_ID, uint.MaxValue);
            }

            // ─── Player fields (only for Player objects) ───
            Player player = obj.ToPlayer();
            if (player != null)
            {
                var pd = player.GetPlayerData();
                updateArray.SetUpdateField((int)PlayerField.PLAYER_WOW_ACCOUNT, player.GetSession().GetAccountGUID());
                updateArray.SetUpdateField((int)PlayerField.PLAYER_FLAGS, (uint)pd.PlayerFlags.GetValue());
                updateArray.SetUpdateField((int)PlayerField.PLAYER_FLAGS_EX, (uint)pd.PlayerFlagsEx.GetValue());
                
                updateArray.SetUpdateField((int)PlayerField.PLAYER_GUILDRANK, (uint)0);
                updateArray.SetUpdateField((int)PlayerField.PLAYER_GUILDDELETE_DATE, (uint)0);
                updateArray.SetUpdateField((int)PlayerField.PLAYER_GUILDLEVEL, (uint)0);
                
                // PLAYER_BYTES: PartyType (0) | NumBankSlots (1) | NativeSex (2) | Inebriation (3)
                uint playerBytes = ((uint)(byte)player.GetUnitData().Sex << 16);
                updateArray.SetUpdateField((int)PlayerField.PLAYER_BYTES, playerBytes);
                updateArray.SetUpdateField((int)PlayerField.PLAYER_BYTES_2, (uint)0);

                // 1.14 Player placeholders
                updateArray.SetUpdateField((int)PlayerField.PLAYER_FIELD_VIRTUAL_PLAYER_REALM, (uint)1);
                updateArray.SetUpdateField((int)PlayerField.PLAYER_FIELD_AVG_ITEM_LEVEL + 3, (uint)1);
                updateArray.SetUpdateField((int)PlayerField.PLAYER_FIELD_HONOR_LEVEL, (uint)1);

                // Visible items (19 equipment slots, 2 uints each: ItemID + ItemAppearanceModID)
                for (byte i = 0; i < EquipmentSlot.End; ++i)
                {
                    Item eqItem = player.GetItemByPos(i);
                    int startIndex = (int)PlayerField.PLAYER_VISIBLE_ITEM + i * 2;
                    if (eqItem != null)
                    {
                        updateArray.SetUpdateField(startIndex, (uint)eqItem.GetEntry());
                        updateArray.SetUpdateField(startIndex + 1, (uint)eqItem.m_itemData.ItemAppearanceModID.GetValue());
                    }
                }

                // Quest Log (25 slots, 16 uints each = 400 fields)
                if (player == target)
                {
                    int qlBaseIndex = (int)PlayerField.PLAYER_QUEST_LOG;
                    for (ushort slot = 0; slot < SharedConst.MaxQuestLogSize; ++slot)
                    {
                        var ql = pd.QuestLog[slot];
                        int dst = qlBaseIndex + slot * 16;
                        updateArray.SetUpdateField(dst + 0, (uint)ql.QuestID.GetValue());
                        updateArray.SetUpdateField(dst + 1, ql.StateFlags.GetValue());
                        for (int j = 0; j < 12; ++j)
                        {
                            ushort a = ql.ObjectiveProgress[j * 2];
                            ushort b = ql.ObjectiveProgress[j * 2 + 1];
                            updateArray.SetUpdateField(dst + 2 + j, (uint)(a | (b << 16)));
                        }
                        updateArray.SetUpdateField(dst + 14, (uint)(ulong)ql.EndTime.GetValue());
                        // +15 is AcceptTime in the 1.14.0 layout (QuestID, StateFlags,
                        // 12 packed objective words, EndTime, AcceptTime). The modern
                        // QuestLog update-field struct dropped that field, so there is
                        // nothing to source it from here; 0 keeps the entry the right
                        // SIZE, which is what matters for the fields after it.
                        updateArray.SetUpdateField(dst + 15, 0u);
                    }
                }

                // Customizations (matching HermesProxy: 2 uints per choice)
                var customizations = pd.Customizations.GetValues();
                for (int i = 0; i < customizations.Count; ++i)
                {
                    int startIndex = (int)PlayerField.PLAYER_FIELD_CUSTOMIZATION_CHOICES + i * 2;
                    updateArray.SetUpdateField(startIndex, (uint)customizations[i].ChrCustomizationOptionID);
                    updateArray.SetUpdateField(startIndex + 1, (uint)customizations[i].ChrCustomizationChoiceID);
                }

                // ─── ActivePlayer fields (only when player is the local target) ───
                if (player == target)
                {
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_COINAGE, (ulong)player.GetMoney());
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_XP, (uint)player.GetXP());
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_NEXT_LEVEL_XP, (uint)player.GetXPForNextLevel());
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MAX_LEVEL, (int)60);
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_CHARACTER_POINTS, (int)player.m_activePlayerData.CharacterPoints.GetValue());

                    // Placeholders required by 1.14 client
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_REST_INFO, (uint)1);

                    for (int i = 0; i < 7; ++i)
                        updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_PCT + i, 1.0f);

                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MOD_HEALING_PCT, 1.0f);
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_PCT, 1.0f);
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MOD_PERIODIC_HEALING_DONE_PERCENT, 1.0f);

                    for (int i = 0; i < 3; ++i)
                    {
                        updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_WEAPON_DMG_MULTIPLIERS + i, 1.0f);
                        updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_WEAPON_ATK_SPEED_MULTIPLIERS + i, 1.0f);
                    }

                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MOD_SPELL_POWER_PCT, 1.0f);
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_MOD_PET_HASTE, 1.0f);

                    // MultiActionBars (7) in byte 1 of ACTIVE_PLAYER_FIELD_BYTES
                    updateArray.SetUpdateField<byte>(ActivePlayerField.ACTIVE_PLAYER_FIELD_BYTES, (byte)7, 1);

                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_HONOR_NEXT_LEVEL, (uint)5500);
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_PVP_TIER_MAX_FROM_WINS, uint.MaxValue);
                    updateArray.SetUpdateField((int)ActivePlayerField.ACTIVE_PLAYER_FIELD_PVP_LAST_WEEKS_TIER_MAX_FROM_WINS, uint.MaxValue);

                    // Known Titles (12 uint32s = 6 ulongs)
                    int titlesStartIndex = (int)ActivePlayerField.ACTIVE_PLAYER_FIELD_KNOWN_TITLES;
                    for (int i = 0; i < 6 && i < player.m_activePlayerData.KnownTitles.Size(); i++)
                    {
                        ulong titles = player.m_activePlayerData.KnownTitles[i];
                        updateArray.SetUpdateField(titlesStartIndex + i * 2, (uint)(titles & 0xFFFFFFFF));
                        updateArray.SetUpdateField(titlesStartIndex + i * 2 + 1, (uint)(titles >> 32));
                    }

                    // Bytes 6: offset 2 = NumBackpackSlots (16)
                    updateArray.SetUpdateField<byte>(ActivePlayerField.ACTIVE_PLAYER_FIELD_BYTES_6, (byte)16, 2);

                    // Inventory slots (Equipment + Bags + Items + Bank + Keyring) mapped to 1.14 Classic offsets
                    int invBaseIndex = (int)ActivePlayerField.ACTIVE_PLAYER_FIELD_INV_SLOT_HEAD;
                    
                    // Equipment (0..18)
                    for (byte i = 0; i < 19; ++i)
                    {
                        Item eq = player.GetItemByPos(i);
                        if (eq != null)
                            updateArray.SetUpdateField(invBaseIndex + i * 4, eq.GetGUID());
                    }
                    // Equipped Bags (30..33 -> Classic 19..22)
                    for (byte i = InventorySlots.BagStart; i < InventorySlots.BagEnd; ++i)
                    {
                        Item bag = player.GetItemByPos(i);
                        if (bag != null)
                        {
                            int classicSlot = 19 + (i - InventorySlots.BagStart);
                            updateArray.SetUpdateField(invBaseIndex + classicSlot * 4, bag.GetGUID());
                        }
                    }
                    // Backpack Items (35..58 -> Classic 23..46)
                    for (byte i = InventorySlots.ItemStart; i < InventorySlots.ItemEnd; ++i)
                    {
                        Item invItem = player.GetItemByPos(i);
                        if (invItem != null)
                        {
                            int classicSlot = 23 + (i - InventorySlots.ItemStart);
                            updateArray.SetUpdateField(invBaseIndex + classicSlot * 4, invItem.GetGUID());
                        }
                    }
                    // Bank Items (59..86 -> Classic 47..74)
                    for (byte i = InventorySlots.BankItemStart; i < InventorySlots.BankItemEnd; ++i)
                    {
                        Item bankItem = player.GetItemByPos(i);
                        if (bankItem != null)
                        {
                            int classicSlot = 47 + (i - InventorySlots.BankItemStart);
                            updateArray.SetUpdateField(invBaseIndex + classicSlot * 4, bankItem.GetGUID());
                        }
                    }
                    // Bank Bags (87..93 -> Classic 75..81)
                    for (byte i = InventorySlots.BankBagStart; i < InventorySlots.BankBagEnd; ++i)
                    {
                        Item bankBag = player.GetItemByPos(i);
                        if (bankBag != null)
                        {
                            int classicSlot = 75 + (i - InventorySlots.BankBagStart);
                            updateArray.SetUpdateField(invBaseIndex + classicSlot * 4, bankBag.GetGUID());
                        }
                    }
                    // Buyback (94..105 -> Classic 82..93)
                    for (byte i = InventorySlots.BuyBackStart; i < InventorySlots.BuyBackEnd; ++i)
                    {
                        Item bbItem = player.GetItemByPos(i);
                        if (bbItem != null)
                        {
                            int classicSlot = 82 + (i - InventorySlots.BuyBackStart);
                            updateArray.SetUpdateField(invBaseIndex + classicSlot * 4, bbItem.GetGUID());
                        }
                    }
                    // Keyring (106..137 -> Classic 94..125)
                    for (byte i = InventorySlots.KeyringStart; i < InventorySlots.KeyringEnd; ++i)
                    {
                        Item krItem = player.GetItemByPos(i);
                        if (krItem != null)
                        {
                            int classicSlot = 94 + (i - InventorySlots.KeyringStart);
                            updateArray.SetUpdateField(invBaseIndex + classicSlot * 4, krItem.GetGUID());
                        }
                    }

                    // Skills (mapped into 7 128-ushort blocks)
                    var skillData = player.m_activePlayerData.Skill.GetValue();
                    if (skillData != null)
                    {
                        int skillStartIndex = (int)ActivePlayerField.ACTIVE_PLAYER_FIELD_SKILL_LINEID;
                        for (int i = 0; i < 256 && i < skillData.SkillLineID.GetSize(); i++)
                        {
                            ushort lineId = (ushort)skillData.SkillLineID[i];
                            if (lineId != 0)
                            {
                                updateArray.SetUpdateField<ushort>(skillStartIndex + i / 2, lineId, (byte)(i & 1));
                                updateArray.SetUpdateField<ushort>(skillStartIndex + 128 + i / 2, (ushort)skillData.SkillStep[i], (byte)(i & 1));
                                updateArray.SetUpdateField<ushort>(skillStartIndex + 256 + i / 2, (ushort)skillData.SkillRank[i], (byte)(i & 1));
                                updateArray.SetUpdateField<ushort>(skillStartIndex + 384 + i / 2, (ushort)skillData.SkillStartingRank[i], (byte)(i & 1));
                                updateArray.SetUpdateField<ushort>(skillStartIndex + 512 + i / 2, (ushort)skillData.SkillMaxRank[i], (byte)(i & 1));
                                updateArray.SetUpdateField<ushort>(skillStartIndex + 640 + i / 2, (ushort)skillData.SkillTempBonus[i], (byte)(i & 1));
                                updateArray.SetUpdateField<ushort>(skillStartIndex + 768 + i / 2, (ushort)skillData.SkillPermBonus[i], (byte)(i & 1));
                            }
                        }
                    }

                    // Completed Quests
                    int qcStartIndex = (int)ActivePlayerField.ACTIVE_PLAYER_FIELD_QUEST_COMPLETED;
                    for (int i = 0; i < 875 && i < player.m_activePlayerData.QuestCompleted.GetSize(); i++)
                    {
                        ulong val = player.m_activePlayerData.QuestCompleted[i];
                        if (val != 0)
                            updateArray.SetUpdateField<ulong>(qcStartIndex + i * 2, val);
                    }
                }
            }
        }
    }
}

