SET @CGUID := 396408;
SET @OGUID := 247114;

-- Gameobject templates
UPDATE `gameobject_template` SET `ContentTuningId`=425, `VerifiedBuild`=46366 WHERE `entry`=208168; -- Candy Bucket

UPDATE `gameobject_template_addon` SET `faction`=1735 WHERE `entry`=208168; -- Candy Bucket

-- Quests
UPDATE `quest_offer_reward` SET `VerifiedBuild`=46366 WHERE `ID`=29005;

DELETE FROM `gameobject_queststarter` WHERE `id`=208168;
INSERT INTO `gameobject_queststarter` (`id`, `quest`, `VerifiedBuild`) VALUES
(208168, 29005, 46366);

UPDATE `gameobject_questender` SET `VerifiedBuild`=46366 WHERE (`id`=208168 AND `quest`=29005);

DELETE FROM `game_event_gameobject_quest` WHERE `id`=208168;

-- Creature spawns
DELETE FROM `creature` WHERE `guid`=@CGUID+0;
INSERT INTO `creature` (`guid`, `id`, `map`, `zoneId`, `areaId`, `spawnDifficulties`, `PhaseId`, `PhaseGroup`, `modelid`, `equipment_id`, `position_x`, `position_y`, `position_z`, `orientation`, `spawntimesecs`, `wander_distance`, `currentwaypoint`, `curhealth`, `curmana`, `MovementType`, `npcflag`, `unit_flags`, `dynamicflags`, `VerifiedBuild`) VALUES
(@CGUID+0, 22816, 1, 4709, 4853, '0', 0, 0, 0, 0, -3212.687255859375, -1716.953369140625, 92.58986663818359375, 5.374788761138916015, 120, 10, 0, 188, 0, 1, 0, 0, 0, 46366); -- Black Cat (Area: Desolation Hold - Difficulty: 0) (possible waypoints or random movement)

-- Old gameobject spawns
DELETE FROM `gameobject` WHERE `guid` BETWEEN 228058 AND 228101;
DELETE FROM `gameobject` WHERE `guid` BETWEEN 228469 AND 228561;
DELETE FROM `game_event_gameobject` WHERE `guid` BETWEEN 228058 AND 228101;
DELETE FROM `game_event_gameobject` WHERE `guid` BETWEEN 228469 AND 228561;

-- Gameobject spawns
DELETE FROM `gameobject` WHERE `guid` BETWEEN @OGUID+0 AND @OGUID+97;
INSERT INTO `gameobject` (`guid`, `id`, `map`, `zoneId`, `areaId`, `spawnDifficulties`, `PhaseId`, `PhaseGroup`, `position_x`, `position_y`, `position_z`, `orientation`, `rotation0`, `rotation1`, `rotation2`, `rotation3`, `spawntimesecs`, `animprogress`, `state`, `VerifiedBuild`) VALUES
(@OGUID+0, 180405, 1, 4709, 4853, '0', 0, 0, -3285.270751953125, -1696.87158203125, 122.621063232421875, 4.572763919830322265, 0, 0, -0.75470924377441406, 0.656059443950653076, 120, 255, 1, 46366), -- G_Pumpkin_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+1, 180405, 1, 4709, 4853, '0', 0, 0, -3225.34033203125, -1673.079833984375, 99.55564117431640625, 5.759587764739990234, 0, 0, -0.25881862640380859, 0.965925931930541992, 120, 255, 1, 46366), -- G_Pumpkin_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+2, 180405, 1, 4709, 4853, '0', 0, 0, -3205.515625, -1730.4097900390625, 93.55985260009765625, 4.241150379180908203, 0, 0, -0.85264015197753906, 0.522498607635498046, 120, 255, 1, 46366), -- G_Pumpkin_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+3, 180405, 1, 4709, 4853, '0', 0, 0, -3277.350830078125, -1826.4757080078125, 108.2657394409179687, 0.087265998125076293, 0, 0, 0.043619155883789062, 0.999048233032226562, 120, 255, 1, 46366), -- G_Pumpkin_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+4, 180405, 1, 4709, 4853, '0', 0, 0, -3235.380126953125, -1822.875, 111.28521728515625, 0.977383077144622802, 0, 0, 0.469470977783203125, 0.882947921752929687, 120, 255, 1, 46366), -- G_Pumpkin_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+5, 180405, 1, 4709, 4853, '0', 0, 0, -3165.58154296875, -1760.1597900390625, 108.2303466796875, 0.977383077144622802, 0, 0, 0.469470977783203125, 0.882947921752929687, 120, 255, 1, 46366), -- G_Pumpkin_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+6, 180406, 1, 4709, 4853, '0', 0, 0, -3300.39404296875, -1807.3125, 106.8153762817382812, 6.003933906555175781, 0, 0, -0.13917255401611328, 0.990268170833587646, 120, 255, 1, 46366), -- G_Pumpkin_02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+7, 180406, 1, 4709, 4853, '0', 0, 0, -3286.958251953125, -1824.9375, 111.0849761962890625, 6.09120035171508789, 0, 0, -0.09584522247314453, 0.995396256446838378, 120, 255, 1, 46366), -- G_Pumpkin_02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+8, 180406, 1, 4709, 4853, '0', 0, 0, -3253.84033203125, -1827.5867919921875, 109.5627212524414062, 0.663223206996917724, 0, 0, 0.325567245483398437, 0.945518851280212402, 120, 255, 1, 46366), -- G_Pumpkin_02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+9, 180406, 1, 4709, 4853, '0', 0, 0, -3172.810791015625, -1740.1041259765625, 105.3613967895507812, 2.251473426818847656, 0, 0, 0.902585029602050781, 0.430511653423309326, 120, 255, 1, 46366), -- G_Pumpkin_02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+10, 180407, 1, 4709, 4853, '0', 0, 0, -3299.045166015625, -1687.435791015625, 123.2102432250976562, 1.169368624687194824, 0, 0, 0.551936149597167968, 0.833886384963989257, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+11, 180407, 1, 4709, 4853, '0', 0, 0, -3312.62841796875, -1762.4600830078125, 110.1776275634765625, 2.024578809738159179, 0, 0, 0.848047256469726562, 0.529920578002929687, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+12, 180407, 1, 4709, 4853, '0', 0, 0, -3234.895751953125, -1763.54345703125, 92.8831024169921875, 3.717553615570068359, 0, 0, -0.95881938934326171, 0.284016460180282592, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+13, 180407, 1, 4709, 4853, '0', 0, 0, -3293.59375, -1816.6197509765625, 107.994781494140625, 5.969027042388916015, 0, 0, -0.1564340591430664, 0.987688362598419189, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+14, 180407, 1, 4709, 4853, '0', 0, 0, -3265.182373046875, -1828.611083984375, 110.5107803344726562, 6.248279094696044921, 0, 0, -0.01745223999023437, 0.999847710132598876, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+15, 180407, 1, 4709, 4853, '0', 0, 0, -3242.694580078125, -1825.3228759765625, 112.7970428466796875, 6.248279094696044921, 0, 0, -0.01745223999023437, 0.999847710132598876, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+16, 180407, 1, 4709, 4853, '0', 0, 0, -3179.6025390625, -1771.0625, 108.2389907836914062, 6.248279094696044921, 0, 0, -0.01745223999023437, 0.999847710132598876, 120, 255, 1, 46366), -- G_Pumpkin_03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+17, 180408, 1, 4709, 4853, '0', 0, 0, -3225.350830078125, -1673.125, 101.1752700805664062, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- G_WitchHat_01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+18, 180414, 1, 4709, 4853, '0', 0, 0, -3225.7431640625, -1675.34033203125, 99.38809967041015625, 4.049167633056640625, 0, 0, -0.89879322052001953, 0.438372820615768432, 120, 255, 1, 46366), -- Cauldron (Area: Desolation Hold - Difficulty: 0)
(@OGUID+19, 180415, 1, 4709, 4853, '0', 0, 0, -3306.885498046875, -1685.6927490234375, 126.5540771484375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+20, 180415, 1, 4709, 4853, '0', 0, 0, -3305.55029296875, -1687.53125, 124.7850723266601562, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+21, 180415, 1, 4709, 4853, '0', 0, 0, -3304.8681640625, -1686.078125, 126.5790481567382812, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+22, 180415, 1, 4709, 4853, '0', 0, 0, -3301.927001953125, -1688.6041259765625, 123.9581985473632812, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+23, 180415, 1, 4709, 4853, '0', 0, 0, -3251.529541015625, -1718.2135009765625, 94.33170318603515625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+24, 180415, 1, 4709, 4853, '0', 0, 0, -3251.404541015625, -1720.295166015625, 93.37094879150390625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+25, 180415, 1, 4709, 4853, '0', 0, 0, -3250.600830078125, -1719.109375, 93.2472991943359375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+26, 180415, 1, 4709, 4853, '0', 0, 0, -3253.302001953125, -1719.654541015625, 93.736968994140625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+27, 180415, 1, 4709, 4853, '0', 0, 0, -3250.861083984375, -1717.310791015625, 93.6382293701171875, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+28, 180415, 1, 4709, 4853, '0', 0, 0, -3222.850830078125, -1675.4635009765625, 100.8471527099609375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+29, 180415, 1, 4709, 4853, '0', 0, 0, -3230.9462890625, -1667.204833984375, 101.4241867065429687, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+30, 180415, 1, 4709, 4853, '0', 0, 0, -3224.37841796875, -1676.55908203125, 100.4570999145507812, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+31, 180415, 1, 4709, 4853, '0', 0, 0, -3222.501708984375, -1677.26220703125, 101.340606689453125, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- CandleBlack01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+32, 180425, 1, 4709, 4853, '0', 0, 0, -3231.09033203125, -1668.1441650390625, 101.4788665771484375, 4.118979454040527343, 0, 0, -0.88294696807861328, 0.469472706317901611, 120, 255, 1, 46366), -- SkullCandle01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+33, 180426, 1, 4709, 4853, '0', 0, 0, -3291.296875, -1712.8507080078125, 133.992767333984375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+34, 180426, 1, 4709, 4853, '0', 0, 0, -3312.411376953125, -1735.7708740234375, 122.8666305541992187, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+35, 180426, 1, 4709, 4853, '0', 0, 0, -3264.741455078125, -1681.3507080078125, 131.1856231689453125, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+36, 180426, 1, 4709, 4853, '0', 0, 0, -3258.257080078125, -1756.63720703125, 92.6760406494140625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+37, 180426, 1, 4709, 4853, '0', 0, 0, -3237.61279296875, -1745.001708984375, 92.21099090576171875, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+38, 180426, 1, 4709, 4853, '0', 0, 0, -3223.882080078125, -1721.6041259765625, 92.36252593994140625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+39, 180426, 1, 4709, 4853, '0', 0, 0, -3228.546875, -1698.295166015625, 95.91880035400390625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+40, 180426, 1, 4709, 4853, '0', 0, 0, -3241.640625, -1741.5347900390625, 91.913604736328125, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+41, 180426, 1, 4709, 4853, '0', 0, 0, -3241.385498046875, -1811.6197509765625, 111.3460235595703125, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+42, 180426, 1, 4709, 4853, '0', 0, 0, -3265.614501953125, -1813.876708984375, 114.1769866943359375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+43, 180426, 1, 4709, 4853, '0', 0, 0, -3257.111083984375, -1815.2742919921875, 111.8281021118164062, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+44, 180426, 1, 4709, 4853, '0', 0, 0, -3178.078125, -1761.3211669921875, 107.1629409790039062, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+45, 180426, 1, 4709, 4853, '0', 0, 0, -3167.72216796875, -1753.920166015625, 107.0633316040039062, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+46, 180426, 1, 4709, 4853, '0', 0, 0, -3159.532958984375, -1742.923583984375, 101.4036636352539062, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+47, 180427, 1, 4709, 4853, '0', 0, 0, -3296.62158203125, -1728.8489990234375, 135.3777618408203125, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+48, 180427, 1, 4709, 4853, '0', 0, 0, -3266.744873046875, -1698.1978759765625, 136.9410247802734375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+49, 180427, 1, 4709, 4853, '0', 0, 0, -3218.366455078125, -1715.9149169921875, 92.38958740234375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+50, 180427, 1, 4709, 4853, '0', 0, 0, -3239.59375, -1745.453125, 92.0661468505859375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+51, 180427, 1, 4709, 4853, '0', 0, 0, -3229.7744140625, -1717.592041015625, 121.7616806030273437, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+52, 180427, 1, 4709, 4853, '0', 0, 0, -3220.48779296875, -1710.4461669921875, 111.5846786499023437, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+53, 180427, 1, 4709, 4853, '0', 0, 0, -3241.69091796875, -1744.5364990234375, 91.9067840576171875, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+54, 180427, 1, 4709, 4853, '0', 0, 0, -3260.239501953125, -1755.421875, 113.8280029296875, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+55, 180427, 1, 4709, 4853, '0', 0, 0, -3259.647705078125, -1825.55908203125, 99.44490814208984375, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+56, 180427, 1, 4709, 4853, '0', 0, 0, -3192.26220703125, -1790.701416015625, 92.13677978515625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+57, 180427, 1, 4709, 4853, '0', 0, 0, -3164.078125, -1768.7882080078125, 95.17014312744140625, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Bat02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+58, 180471, 1, 4709, 4853, '0', 0, 0, -3288.647705078125, -1679.5399169921875, 141.6427001953125, 0.715584874153137207, 0, 0, 0.350207328796386718, 0.936672210693359375, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+59, 180471, 1, 4709, 4853, '0', 0, 0, -3298.092041015625, -1684.8125, 141.9681396484375, 0.366517573595046997, 0, 0, 0.182234764099121093, 0.98325502872467041, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+60, 180471, 1, 4709, 4853, '0', 0, 0, -3293.210205078125, -1682.875, 141.5181884765625, 0.628316879272460937, 0, 0, 0.309016227722167968, 0.95105677843093872, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+61, 180471, 1, 4709, 4853, '0', 0, 0, -3290.1025390625, -1681.1180419921875, 141.4798583984375, 0.715584874153137207, 0, 0, 0.350207328796386718, 0.936672210693359375, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+62, 180471, 1, 4709, 4853, '0', 0, 0, -3287.296875, -1678.0225830078125, 141.8128662109375, 0.733038187026977539, 0, 0, 0.358367919921875, 0.933580458164215087, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+63, 180471, 1, 4709, 4853, '0', 0, 0, -3285.979248046875, -1676.4166259765625, 141.9869384765625, 0.802850961685180664, 0, 0, 0.390730857849121093, 0.920504987239837646, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+64, 180471, 1, 4709, 4853, '0', 0, 0, -3296.373291015625, -1684.1475830078125, 141.8131103515625, 0.401424884796142578, 0, 0, 0.199367523193359375, 0.979924798011779785, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+65, 180471, 1, 4709, 4853, '0', 0, 0, -3294.984375, -1683.5572509765625, 141.6768798828125, 0.436331570148468017, 0, 0, 0.216439247131347656, 0.976296067237854003, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+66, 180471, 1, 4709, 4853, '0', 0, 0, -3258.87158203125, -1724.0191650390625, 97.68093109130859375, 0.558503925800323486, 0, 0, 0.275636672973632812, 0.961261868476867675, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+67, 180471, 1, 4709, 4853, '0', 0, 0, -3259.975830078125, -1724.814208984375, 97.57424163818359375, 1.151916384696960449, 0, 0, 0.544638633728027343, 0.838670849800109863, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+68, 180471, 1, 4709, 4853, '0', 0, 0, -3261.34033203125, -1725.689208984375, 97.50745391845703125, 0.890116631984710693, 0, 0, 0.430510520935058593, 0.902585566043853759, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+69, 180471, 1, 4709, 4853, '0', 0, 0, -3311.97216796875, -1764.0833740234375, 129.41973876953125, 2.076939344406127929, 0, 0, 0.861628532409667968, 0.50753939151763916, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+70, 180471, 1, 4709, 4853, '0', 0, 0, -3255.640625, -1719.546875, 97.47737884521484375, 0.558503925800323486, 0, 0, 0.275636672973632812, 0.961261868476867675, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+71, 180471, 1, 4709, 4853, '0', 0, 0, -3257.057373046875, -1722.4114990234375, 97.66492462158203125, 0.942476630210876464, 0, 0, 0.453989982604980468, 0.891006767749786376, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+72, 180471, 1, 4709, 4853, '0', 0, 0, -3312.9462890625, -1762.203125, 129.5939483642578125, 2.146752834320068359, 0, 0, 0.878816604614257812, 0.477159708738327026, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+73, 180471, 1, 4709, 4853, '0', 0, 0, -3311.053955078125, -1765.8958740234375, 129.2496490478515625, 2.059488296508789062, 0, 0, 0.857167243957519531, 0.515038192272186279, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+74, 180471, 1, 4709, 4853, '0', 0, 0, -3256.388916015625, -1721.048583984375, 97.56262969970703125, 0.942476630210876464, 0, 0, 0.453989982604980468, 0.891006767749786376, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+75, 180471, 1, 4709, 4853, '0', 0, 0, -3223.928955078125, -1675.3992919921875, 119.5629730224609375, 5.672322273254394531, 0, 0, -0.3007049560546875, 0.953717231750488281, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+76, 180471, 1, 4709, 4853, '0', 0, 0, -3227.65625, -1672.9478759765625, 119.1805191040039062, 5.6897735595703125, 0, 0, -0.29237174987792968, 0.956304728984832763, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+77, 180471, 1, 4709, 4853, '0', 0, 0, -3232.092041015625, -1667.640625, 119.5213394165039062, 5.427974700927734375, 0, 0, -0.41469287872314453, 0.909961462020874023, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+78, 180471, 1, 4709, 4853, '0', 0, 0, -3308.90966796875, -1776.4879150390625, 129.5751495361328125, 1.710421562194824218, 0, 0, 0.754709243774414062, 0.656059443950653076, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+79, 180471, 1, 4709, 4853, '0', 0, 0, -3231.048583984375, -1669.1163330078125, 119.362060546875, 5.480334281921386718, 0, 0, -0.39073085784912109, 0.920504987239837646, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+80, 180471, 1, 4709, 4853, '0', 0, 0, -3222.2119140625, -1676.46875, 119.72918701171875, 5.619962215423583984, 0, 0, -0.32556724548339843, 0.945518851280212402, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+81, 180471, 1, 4709, 4853, '0', 0, 0, -3230.109375, -1670.5364990234375, 119.2201080322265625, 5.6897735595703125, 0, 0, -0.29237174987792968, 0.956304728984832763, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+82, 180471, 1, 4709, 4853, '0', 0, 0, -3309.265625, -1771.2569580078125, 129.1250152587890625, 1.972219824790954589, 0, 0, 0.83388519287109375, 0.55193793773651123, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+83, 180471, 1, 4709, 4853, '0', 0, 0, -3233.21533203125, -1665.8072509765625, 119.68890380859375, 5.375615119934082031, 0, 0, -0.4383707046508789, 0.898794233798980712, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+84, 180471, 1, 4709, 4853, '0', 0, 0, -3309.994873046875, -1767.7569580078125, 129.08673095703125, 2.059488296508789062, 0, 0, 0.857167243957519531, 0.515038192272186279, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+85, 180471, 1, 4709, 4853, '0', 0, 0, -3309.01904296875, -1774.654541015625, 129.4199066162109375, 1.745326757431030273, 0, 0, 0.766043663024902343, 0.642788589000701904, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+86, 180471, 1, 4709, 4853, '0', 0, 0, -3225.895751953125, -1674.1632080078125, 119.371490478515625, 5.6897735595703125, 0, 0, -0.29237174987792968, 0.956304728984832763, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+87, 180471, 1, 4709, 4853, '0', 0, 0, -3309.163330078125, -1773.1510009765625, 129.2836761474609375, 1.780233979225158691, 0, 0, 0.7771453857421875, 0.629321098327636718, 120, 255, 1, 46366), -- HangingSkullLight01 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+88, 180472, 1, 4709, 4853, '0', 0, 0, -3291.5, -1681.921875, 142.7299346923828125, 5.323255538940429687, 0, 0, -0.46174812316894531, 0.887011110782623291, 120, 255, 1, 46366), -- HangingSkullLight02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+89, 180472, 1, 4709, 4853, '0', 0, 0, -3257.76904296875, -1723.3194580078125, 98.9659423828125, 5.567600727081298828, 0, 0, -0.35020732879638671, 0.936672210693359375, 120, 255, 1, 46366), -- HangingSkullLight02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+90, 180472, 1, 4709, 4853, '0', 0, 0, -3228.9375, -1671.8941650390625, 120.3505172729492187, 3.961898565292358398, 0, 0, -0.91705989837646484, 0.398749500513076782, 120, 255, 1, 46366), -- HangingSkullLight02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+91, 180472, 1, 4709, 4853, '0', 0, 0, -3309.56591796875, -1769.376708984375, 130.1752471923828125, 0.366517573595046997, 0, 0, 0.182234764099121093, 0.98325502872467041, 120, 255, 1, 46366), -- HangingSkullLight02 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+92, 180523, 1, 4709, 4853, '0', 0, 0, -3217.59375, -1664.3055419921875, 100.3092422485351562, 0, 0, 0, 0, 1, 120, 255, 1, 46366), -- Apple Bob (Area: Desolation Hold - Difficulty: 0)
(@OGUID+93, 185436, 1, 4709, 4853, '0', 0, 0, -3225.505126953125, -1675.3367919921875, 99.89443206787109375, 2.635444164276123046, 0, 0, 0.96814727783203125, 0.250381410121917724, 120, 255, 1, 46366), -- Sitting Skeleton 03 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+94, 185438, 1, 4709, 4853, '0', 0, 0, -3226.173583984375, -1675.3316650390625, 99.93881988525390625, 0.27925160527229309, 0, 0, 0.139172554016113281, 0.990268170833587646, 120, 255, 1, 46366), -- Sitting Skeleton 04 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+95, 207736, 1, 4709, 4853, '0', 0, 0, -3294.788330078125, -1676.82470703125, 122.625, 5.35816192626953125, 0, 0, -0.446197509765625, 0.894934535026550292, 120, 255, 1, 46366), -- G_HangingSkeleton_01 (Scale 2) (Area: Desolation Hold - Difficulty: 0)
(@OGUID+96, 208059, 1, 4709, 4853, '0', 0, 0, -3266.369873046875, -1715.407958984375, 108.0419235229492187, 1.186823248863220214, 0, 0, 0.559192657470703125, 0.829037725925445556, 120, 255, 1, 46366), -- G_Pumpkin_02 Scale 5.0 (Area: Desolation Hold - Difficulty: 0)
(@OGUID+97, 208168, 1, 4709, 4853, '0', 0, 0, -3221.083251953125, -1660.970458984375, 99.90734100341796875, 5.602506637573242187, 0, 0, -0.33380699157714843, 0.942641437053680419, 120, 255, 1, 46366); -- Candy Bucket (Area: Desolation Hold - Difficulty: 0)

-- Event spawns
DELETE FROM `game_event_creature` WHERE `eventEntry`=12 AND `guid`=@CGUID+0;
INSERT INTO `game_event_creature` (`eventEntry`, `guid`) VALUES
(12, @CGUID+0);

DELETE FROM `game_event_gameobject` WHERE `eventEntry`=12 AND `guid` BETWEEN @OGUID+0 AND @OGUID+97;
INSERT INTO `game_event_gameobject` (`eventEntry`, `guid`) VALUES
(12, @OGUID+0),
(12, @OGUID+1),
(12, @OGUID+2),
(12, @OGUID+3),
(12, @OGUID+4),
(12, @OGUID+5),
(12, @OGUID+6),
(12, @OGUID+7),
(12, @OGUID+8),
(12, @OGUID+9),
(12, @OGUID+10),
(12, @OGUID+11),
(12, @OGUID+12),
(12, @OGUID+13),
(12, @OGUID+14),
(12, @OGUID+15),
(12, @OGUID+16),
(12, @OGUID+17),
(12, @OGUID+18),
(12, @OGUID+19),
(12, @OGUID+20),
(12, @OGUID+21),
(12, @OGUID+22),
(12, @OGUID+23),
(12, @OGUID+24),
(12, @OGUID+25),
(12, @OGUID+26),
(12, @OGUID+27),
(12, @OGUID+28),
(12, @OGUID+29),
(12, @OGUID+30),
(12, @OGUID+31),
(12, @OGUID+32),
(12, @OGUID+33),
(12, @OGUID+34),
(12, @OGUID+35),
(12, @OGUID+36),
(12, @OGUID+37),
(12, @OGUID+38),
(12, @OGUID+39),
(12, @OGUID+40),
(12, @OGUID+41),
(12, @OGUID+42),
(12, @OGUID+43),
(12, @OGUID+44),
(12, @OGUID+45),
(12, @OGUID+46),
(12, @OGUID+47),
(12, @OGUID+48),
(12, @OGUID+49),
(12, @OGUID+50),
(12, @OGUID+51),
(12, @OGUID+52),
(12, @OGUID+53),
(12, @OGUID+54),
(12, @OGUID+55),
(12, @OGUID+56),
(12, @OGUID+57),
(12, @OGUID+58),
(12, @OGUID+59),
(12, @OGUID+60),
(12, @OGUID+61),
(12, @OGUID+62),
(12, @OGUID+63),
(12, @OGUID+64),
(12, @OGUID+65),
(12, @OGUID+66),
(12, @OGUID+67),
(12, @OGUID+68),
(12, @OGUID+69),
(12, @OGUID+70),
(12, @OGUID+71),
(12, @OGUID+72),
(12, @OGUID+73),
(12, @OGUID+74),
(12, @OGUID+75),
(12, @OGUID+76),
(12, @OGUID+77),
(12, @OGUID+78),
(12, @OGUID+79),
(12, @OGUID+80),
(12, @OGUID+81),
(12, @OGUID+82),
(12, @OGUID+83),
(12, @OGUID+84),
(12, @OGUID+85),
(12, @OGUID+86),
(12, @OGUID+87),
(12, @OGUID+88),
(12, @OGUID+89),
(12, @OGUID+90),
(12, @OGUID+91),
(12, @OGUID+92),
(12, @OGUID+93),
(12, @OGUID+94),
(12, @OGUID+95),
(12, @OGUID+96),
(12, @OGUID+97);