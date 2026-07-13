using Game.Entities;
using Game.Networking;
using Game.Networking.Packets;
using Framework.Constants;
using Framework.GameMath;
using System.Collections.Generic;

namespace Game.Networking.Adapters.V1_14_0
{
    /// <summary>
    /// Builds SMSG_UPDATE_OBJECT blocks using the 1.14.0 (Classic Era) flat-array update field format
    /// instead of the modern (10.x/3.4.3) structured update field format.
    /// </summary>
    public static class UpdateObjectBuilder1140
    {
        /// <summary>
        /// Gets the total update field count for a given TypeId in the 1.14.0 protocol.
        /// </summary>
        static uint GetFieldCount(TypeId typeId)
        {
            return typeId switch
            {
                TypeId.Object => (uint)ObjectField.OBJECT_END,
                TypeId.Item => (uint)ItemField.ITEM_END,
                TypeId.Container => (uint)ContainerField.CONTAINER_END,
                TypeId.Unit => (uint)UnitField.UNIT_END,
                TypeId.Player => (uint)PlayerField.PLAYER_END,
                TypeId.ActivePlayer => (uint)ActivePlayerField.ACTIVE_PLAYER_END,
                TypeId.GameObject => (uint)GameObjectField.GAMEOBJECT_END,
                TypeId.DynamicObject => (uint)DynamicObjectField.DYNAMICOBJECT_END,
                TypeId.Corpse => (uint)CorpseField.CORPSE_END,
                TypeId.AreaTrigger => (uint)AreaTriggerField.AREATRIGGER_END,
                TypeId.SceneObject => (uint)SceneObjectField.SCENEOBJECT_END,
                _ => (uint)ObjectField.OBJECT_END,
            };
        }

        /// <summary>
        /// Gets the dynamic field count for a given TypeId in the 1.14.0 protocol.
        /// </summary>
        static uint GetDynamicFieldCount(TypeId typeId)
        {
            return typeId switch
            {
                TypeId.Object => (uint)ObjectDynamicField.OBJECT_DYNAMIC_END,
                TypeId.Item => (uint)ItemDynamicField.ITEM_DYNAMIC_END,
                TypeId.Container => (uint)ContainerDynamicField.CONTAINER_DYNAMIC_END,
                TypeId.Unit => (uint)UnitDynamicField.UNIT_DYNAMIC_END,
                TypeId.Player => (uint)PlayerDynamicField.PLAYER_DYNAMIC_END,
                TypeId.ActivePlayer => (uint)ActivePlayerDynamicField.ACTIVE_PLAYER_DYNAMIC_END,
                TypeId.GameObject => (uint)GameObjectDynamicField.GAMEOBJECT_DYNAMIC_END,
                TypeId.DynamicObject => (uint)DynamicObjectDynamicField.DYNAMICOBJECT_DYNAMIC_END,
                TypeId.Corpse => (uint)CorpseDynamicField.CORPSE_DYNAMIC_END,
                TypeId.AreaTrigger => (uint)AreaTriggerDynamicField.AREATRIGGER_DYNAMIC_END,
                TypeId.SceneObject => (uint)SceneObjectDynamicField.SCENEOBJECT_DYNAMIC_END,
                _ => (uint)ObjectDynamicField.OBJECT_DYNAMIC_END,
            };
        }

        /// <summary>
        /// Gets the 1.14.0-compatible ObjectTypeMask for a given TypeId.
        /// </summary>
        static int GetObjectTypeMask(TypeId typeId)
        {
            int mask = 0x0001; // Object
            switch (typeId)
            {
                case TypeId.Item:
                    mask |= 0x0002;
                    break;
                case TypeId.Container:
                    mask |= 0x0002 | 0x0004;
                    break;
                case TypeId.Unit:
                    mask |= 0x0008;
                    break;
                case TypeId.Player:
                case TypeId.ActivePlayer:
                    mask |= 0x0008 | 0x0010;
                    break;
                case TypeId.GameObject:
                    mask |= 0x0020;
                    break;
                case TypeId.DynamicObject:
                    mask |= 0x0040;
                    break;
                case TypeId.Corpse:
                    mask |= 0x0080;
                    break;
                case TypeId.AreaTrigger:
                    mask |= 0x0100;
                    break;
                case TypeId.SceneObject:
                    mask |= 0x0200;
                    break;
            }
            return mask;
        }

        public static void BuildMovementUpdate1140(WorldPacket data, WorldObject obj, CreateObjectBits flags, Player target)
        {
            List<Milliseconds> PauseTimes = null;
            GameObject go = obj.ToGameObject();
            if (go != null)
                PauseTimes = go.GetPauseTimes();

            data.WriteBit(flags.NoBirthAnim);
            data.WriteBit(flags.EnablePortals);
            data.WriteBit(flags.PlayHoverAnim);
            data.WriteBit(flags.MovementUpdate);
            data.WriteBit(flags.MovementTransport);
            data.WriteBit(flags.Stationary);
            data.WriteBit(flags.CombatVictim);
            data.WriteBit(flags.ServerTime);
            data.WriteBit(flags.Vehicle);
            data.WriteBit(flags.AnimKit);
            data.WriteBit(flags.Rotation);
            data.WriteBit(flags.AreaTrigger);
            data.WriteBit(flags.GameObject);
            data.WriteBit(flags.SmoothPhasing);
            data.WriteBit(flags.ThisIsYou);
            data.WriteBit(flags.SceneObject);
            data.WriteBit(flags.ActivePlayer);
            data.WriteBit(flags.Conversation);
            data.FlushBits();

            if (flags.MovementUpdate)
            {
                Unit unit = obj.ToUnit();
                bool HasFallDirection = unit.HasUnitMovementFlag(MovementFlag.Falling);
                bool HasFall = HasFallDirection || unit.m_movementInfo.jump.fallTime != 0;
                bool HasSpline = unit.IsSplineEnabled();

                data.WritePackedGuid(obj.GetGUID());                             // MoverGUID

                data.WriteUInt32(unit.m_movementInfo.Time);                      // MoveTime
                data.WriteFloat(unit.GetPositionX());
                data.WriteFloat(unit.GetPositionY());
                data.WriteFloat(unit.GetPositionZ());
                data.WriteFloat(unit.GetOrientation());

                data.WriteFloat(unit.m_movementInfo.Pitch);                      // Pitch
                data.WriteFloat(unit.m_movementInfo.stepUpStartElevation);       // StepUpStartElevation

                data.WriteUInt32(0);                                             // RemoveForcesIDs.size()
                data.WriteUInt32(0);                                             // MoveIndex

                data.WriteBits((uint)unit.GetUnitMovementFlags(), 30);           // MovementFlags
                data.WriteBits((uint)unit.GetUnitMovementFlags2(), 18);          // MovementFlags2

                data.WriteBit(!unit.m_movementInfo.transport.guid.IsEmpty());    // HasTransport
                data.WriteBit(HasFall);                                          // HasFall
                data.WriteBit(HasSpline);                                        // HasSpline
                data.WriteBit(false);                                            // HeightChangeFailed
                data.WriteBit(false);                                            // RemoteTimeValid
                data.FlushBits();

                if (!unit.m_movementInfo.transport.guid.IsEmpty())
                    unit.m_movementInfo.transport.Write(data);

                if (HasFall)
                {
                    data.WriteUInt32(unit.m_movementInfo.jump.fallTime);         // Time
                    data.WriteFloat(unit.m_movementInfo.jump.zspeed);            // JumpVelocity
                    data.WriteBit(HasFallDirection);
                    data.FlushBits();

                    if (HasFallDirection)
                    {
                        data.WriteFloat(unit.m_movementInfo.jump.sinAngle);      // Direction
                        data.WriteFloat(unit.m_movementInfo.jump.cosAngle);
                        data.WriteFloat(unit.m_movementInfo.jump.xyspeed);       // Speed
                    }
                }

                data.WriteFloat(unit.GetSpeed(UnitMoveType.Walk));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.Run));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.RunBack));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.Swim));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.SwimBack));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.Flight));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.FlightBack));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.TurnRate));
                data.WriteFloat(unit.GetSpeed(UnitMoveType.PitchRate));

                // 1.14.0 REQUIRES the MovementForces ModMagnitude float
                MovementForces movementForces = unit.GetMovementForces();
                if (movementForces != null)
                {
                    data.WriteInt32(movementForces.GetForces().Count);
                    data.WriteFloat(movementForces.GetModMagnitude());           // MovementForcesModMagnitude
                }
                else
                {
                    data.WriteUInt32(0);
                    data.WriteFloat(1.0f);                                       // MovementForcesModMagnitude
                }

                data.WriteBit(HasSpline);
                data.FlushBits();

                if (movementForces != null)
                {
                    foreach (MovementForce force in movementForces.GetForces())
                        force.Write(data, unit);
                }

                if (HasSpline)
                    unit.MoveSpline.Write(data);
            }

            data.WriteInt32(PauseTimes != null ? PauseTimes.Count : 0);

            if (flags.Stationary)
            {
                data.WriteFloat(obj.GetStationaryX());
                data.WriteFloat(obj.GetStationaryY());
                data.WriteFloat(obj.GetStationaryZ());
                data.WriteFloat(obj.GetStationaryO());
            }

            if (flags.CombatVictim)
                data.WritePackedGuid(obj.ToUnit().GetVictim().GetGUID());

            if (flags.ServerTime)
                data.WriteUInt32(LoopTime.RelativeTime);

            if (flags.Vehicle)
            {
                Unit unit = obj.ToUnit();
                data.WriteInt32(unit.GetVehicleKit().GetVehicleInfo().Id);
                data.WriteFloat(unit.GetOrientation());
            }

            if (flags.AnimKit)
            {
                data.WriteUInt16(obj.GetAIAnimKitId());
                data.WriteUInt16(obj.GetMovementAnimKitId());
                data.WriteUInt16(obj.GetMeleeAnimKitId());
            }

            if (flags.Rotation)
                data.WriteInt64(obj.ToGameObject().GetPackedLocalRotation());

            if (PauseTimes != null && !PauseTimes.Empty())
            {
                foreach (var stopFrame in PauseTimes)
                    data.WriteInt32(stopFrame);
            }

            if (flags.MovementTransport)
            {
                obj.m_movementInfo.transport.Write(data);
            }

            if (flags.AreaTrigger)
            {
                AreaTrigger areaTrigger = obj.ToAreaTrigger();
                AreaTriggerCreateProperties createProperties = areaTrigger.GetCreateProperties();
                AreaTriggerShapeInfo shape = areaTrigger.GetShape();

                data.WriteUInt32(areaTrigger.GetTimeSinceCreated());
                data.WriteVector3(areaTrigger.GetRollPitchYaw());

                bool hasAbsoluteOrientation = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.HasAbsoluteOrientation);
                bool hasDynamicShape = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.HasDynamicShape);
                bool hasAttached = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.HasAttached);
                bool hasFaceMovementDir = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.HasFaceMovementDir);
                bool hasFollowsTerrain = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.HasFollowsTerrain);
                bool hasUnk1 = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.Unk1);
                bool hasTargetRollPitchYaw = createProperties != null && createProperties.Flags.HasFlag(AreaTriggerCreatePropertiesFlag.HasTargetRollPitchYaw);
                bool hasScaleCurveID = createProperties != null && createProperties.ScaleCurveId != 0;
                bool hasMorphCurveID = createProperties != null && createProperties.MorphCurveId != 0;
                bool hasFacingCurveID = createProperties != null && createProperties.FacingCurveId != 0;
                bool hasMoveCurveID = createProperties != null && createProperties.MoveCurveId != 0;
                bool hasAreaTriggerSphere = shape.IsSphere();
                bool hasAreaTriggerBox = shape.IsBox();
                bool hasAreaTriggerPolygon = createProperties != null && shape.IsPolygon();
                bool hasAreaTriggerCylinder = shape.IsCylinder();
                bool hasAreaTriggerSpline = areaTrigger.HasSplines();
                bool hasOrbit = areaTrigger.HasOrbit();
                bool hasMovementScript = false;

                data.WriteBit(hasAbsoluteOrientation);
                data.WriteBit(hasDynamicShape);
                data.WriteBit(hasAttached);
                data.WriteBit(hasFaceMovementDir);
                data.WriteBit(hasFollowsTerrain);
                data.WriteBit(hasUnk1);
                data.WriteBit(hasTargetRollPitchYaw);
                data.WriteBit(hasScaleCurveID);
                data.WriteBit(hasMorphCurveID);
                data.WriteBit(hasFacingCurveID);
                data.WriteBit(hasMoveCurveID);
                data.WriteBit(hasAreaTriggerSphere);
                data.WriteBit(hasAreaTriggerBox);
                data.WriteBit(hasAreaTriggerPolygon);
                data.WriteBit(hasAreaTriggerCylinder);
                // Removed 10.x specific AreaTrigger shape types
                data.WriteBit(hasAreaTriggerSpline);
                data.WriteBit(hasOrbit);
                data.WriteBit(hasMovementScript);
                data.FlushBits();

                if (hasAreaTriggerSpline)
                {
                    data.WriteInt32(areaTrigger.GetTimeToTarget());
                    data.WriteUInt32(areaTrigger.GetElapsedTimeForMovement());
                    areaTrigger.GetSpline().Write(data);
                }

                if (hasTargetRollPitchYaw)
                    data.WriteVector3(areaTrigger.GetTargetRollPitchYaw());

                if (hasScaleCurveID)
                    data.WriteInt32(createProperties.ScaleCurveId);

                if (hasMorphCurveID)
                    data.WriteInt32(createProperties.MorphCurveId);

                if (hasFacingCurveID)
                    data.WriteInt32(createProperties.FacingCurveId);

                if (hasMoveCurveID)
                    data.WriteInt32(createProperties.MoveCurveId);

                if (hasAreaTriggerSphere)
                {
                    data.WriteFloat(shape.SphereDatas.Radius);
                    data.WriteFloat(shape.SphereDatas.RadiusTarget);
                }

                if (hasAreaTriggerBox)
                {
                    unsafe
                    {
                        data.WriteFloat(shape.BoxDatas.Extents[0]);
                        data.WriteFloat(shape.BoxDatas.Extents[1]);
                        data.WriteFloat(shape.BoxDatas.Extents[2]);

                        data.WriteFloat(shape.BoxDatas.ExtentsTarget[0]);
                        data.WriteFloat(shape.BoxDatas.ExtentsTarget[1]);
                        data.WriteFloat(shape.BoxDatas.ExtentsTarget[2]);
                    }
                }

                if (hasAreaTriggerPolygon)
                {
                    data.WriteInt32(shape.PolygonVertices.Count);
                    data.WriteInt32(shape.PolygonVerticesTarget.Count);
                    data.WriteFloat(shape.PolygonDatas.Height);
                    data.WriteFloat(shape.PolygonDatas.HeightTarget);

                    foreach (var vertice in shape.PolygonVertices)
                        data.WriteVector2(vertice);

                    foreach (var vertice in shape.PolygonVerticesTarget)
                        data.WriteVector2(vertice);
                }

                if (hasAreaTriggerCylinder)
                {
                    data.WriteFloat(shape.CylinderDatas.Radius);
                    data.WriteFloat(shape.CylinderDatas.RadiusTarget);
                    data.WriteFloat(shape.CylinderDatas.Height);
                    data.WriteFloat(shape.CylinderDatas.HeightTarget);
                    data.WriteFloat(shape.CylinderDatas.LocationZOffset);
                    data.WriteFloat(shape.CylinderDatas.LocationZOffsetTarget);
                }

                if (hasOrbit)
                    areaTrigger.GetOrbit().Write(data);
            }

            if (flags.GameObject)
            {
                bool bit8 = false;
                uint Int1 = 0;
                GameObject gameObject = obj.ToGameObject();
                data.WriteInt32(gameObject.GetWorldEffectID());

                data.WriteBit(bit8);
                data.FlushBits();
                if (bit8)
                    data.WriteUInt32(Int1);
            }

            if (flags.SmoothPhasing)
            {
                SmoothPhasingInfo smoothPhasingInfo = obj.GetSmoothPhasing().GetInfoForSeer(target.GetGUID());
                data.WriteBit(smoothPhasingInfo.ReplaceActive);
                data.WriteBit(smoothPhasingInfo.StopAnimKits);
                data.WriteBit(smoothPhasingInfo.ReplaceObject.HasValue);
                data.FlushBits();
                if (smoothPhasingInfo.ReplaceObject.HasValue)
                    data.WritePackedGuid(smoothPhasingInfo.ReplaceObject.Value);
            }

            if (flags.SceneObject)
            {
                data.WriteBit(false); // HasLocalScriptData
                data.WriteBit(false); // HasPetBattleFullUpdate
                data.FlushBits();
            }

            if (flags.ActivePlayer)
            {
                bool hasSceneInstanceIDs = false;
                bool hasRuneState = false;
                bool hasActionButtons = false; // TODO ActionButtons

                data.WriteBit(hasSceneInstanceIDs);
                data.WriteBit(hasRuneState);
                data.WriteBit(hasActionButtons);
                data.FlushBits();

                if (hasSceneInstanceIDs)
                {
                    int sceneInstanceIDs = 0;
                    data.WriteInt32(sceneInstanceIDs);
                    for (int i = 0; i < sceneInstanceIDs; ++i)
                        data.WriteInt32(0); // SceneInstanceIDs
                }

                if (hasRuneState)
                {
                    data.WriteUInt8(0); // RechargingRuneMask
                    data.WriteUInt8(0); // UsableRuneMask
                    data.WriteUInt32(0); // RuneCount
                }

                if (hasActionButtons)
                {
                    for (int i = 0; i < 132; i++)
                        data.WriteInt32(0); // ActionButton
                }
            }

            if (flags.Conversation)
            {
                Conversation self = obj.ToConversation();
                if (data.WriteBit(self.GetTextureKitId() != 0))
                    data.WriteInt32(self.GetTextureKitId());
                data.FlushBits();
            }
        }

        public static void BuildCreateUpdateBlockForPlayer(UpdateData data, WorldObject obj, Player target, CreateObjectBits flags, UpdateType updateType)
        {
            TypeId typeId = obj.GetTypeId();
            if (target == obj)
                typeId = TypeId.ActivePlayer;

            uint fieldCount = GetFieldCount(typeId);
            uint dynamicFieldCount = GetDynamicFieldCount(typeId);

            WorldPacket buffer = new();
            buffer.WriteUInt8((byte)updateType);
            buffer.WritePackedGuid(obj.GetGUID());
            buffer.WriteUInt8(GetClientTypeId1140(typeId));
            buffer.WriteInt32(GetObjectTypeMask(typeId));

            // Write movement update using our 1.14.0 adapted logic
            BuildMovementUpdate1140(buffer, obj, flags, target);

            // Build the flat update values array
            var updateArray = new UpdateFieldsArray1140(fieldCount);
            UpdateFieldsMapper1140.MapObjectFields(updateArray, obj, target);
            updateArray.WriteToPacket(buffer);

            // Write empty dynamic fields update
            var dynamicFields = new DynamicUpdateFieldsArray(dynamicFieldCount, updateType);
            dynamicFields.WriteToPacket(buffer);

            data.AddUpdateBlock(buffer);
        }

        public static void BuildValuesUpdateBlockForPlayer(UpdateData data, WorldObject obj, Player target)
        {
            TypeId typeId = obj.GetTypeId();
            if (target == obj)
                typeId = TypeId.ActivePlayer;

            uint fieldCount = GetFieldCount(typeId);
            uint dynamicFieldCount = GetDynamicFieldCount(typeId);

            WorldPacket buffer = new();
            buffer.WriteUInt8((byte)UpdateType.Values);
            buffer.WritePackedGuid(obj.GetGUID());

            // For partial updates we also use the flat array; the mask determines what gets sent
            var updateArray = new UpdateFieldsArray1140(fieldCount);
            UpdateFieldsMapper1140.MapObjectFields(updateArray, obj, target);
            updateArray.WriteToPacket(buffer);

            // Write empty dynamic fields update
            var dynamicFields = new DynamicUpdateFieldsArray(dynamicFieldCount, UpdateType.Values);
            dynamicFields.WriteToPacket(buffer);

            data.AddUpdateBlock(buffer);
        }
        public static byte GetClientTypeId1140(TypeId typeId)
        {
            switch (typeId)
            {
                case TypeId.Object: return 0;
                case TypeId.Item: return 1;
                case TypeId.Container: return 2;
                case TypeId.Unit: return 3;
                case TypeId.Player: return 4;
                case TypeId.ActivePlayer: return 5;
                case TypeId.GameObject: return 6;
                case TypeId.DynamicObject: return 7;
                case TypeId.Corpse: return 8;
                case TypeId.AreaTrigger: return 9;
                case TypeId.SceneObject: return 10;
                case TypeId.Conversation: return 11;
                default: return 0;
            }
        }
    }
}
