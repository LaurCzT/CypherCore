SET @OGUID := 3002747;

-- Gameobject templates
UPDATE `gameobject_template` SET `ContentTuningId`=425, `VerifiedBuild`=51886 WHERE `entry`=208156; -- Candy Bucket

UPDATE `gameobject_template_addon` SET `faction`=1735 WHERE `entry`=208156; -- Candy Bucket

-- Quests
DELETE FROM `quest_offer_reward` WHERE `ID`=28987;
INSERT INTO `quest_offer_reward` (`ID`, `Emote1`, `Emote2`, `Emote3`, `Emote4`, `EmoteDelay1`, `EmoteDelay2`, `EmoteDelay3`, `EmoteDelay4`, `RewardText`, `VerifiedBuild`) VALUES
(28987, 0, 0, 0, 0, 0, 0, 0, 0, 'Candy buckets like this are located in inns throughout the realms. Go ahead... take some!', 51886); -- Candy Bucket

DELETE FROM `gameobject_queststarter` WHERE `id`=208156;
INSERT INTO `gameobject_queststarter` (`id`, `quest`, `VerifiedBuild`) VALUES
(208156, 28987, 51886);

UPDATE `gameobject_questender` SET `VerifiedBuild`=51886 WHERE (`id`=208156 AND `quest`=28987);

DELETE FROM `game_event_gameobject_quest` WHERE `id`=208156;

-- Old gameobject spawns
DELETE FROM `gameobject` WHERE `guid` BETWEEN 229373 AND 229441;
DELETE FROM `gameobject` WHERE `guid` BETWEEN 229937 AND 230022;
DELETE FROM `game_event_gameobject` WHERE `guid` BETWEEN 229373 AND 229441;
DELETE FROM `game_event_gameobject` WHERE `guid` BETWEEN 229937 AND 230022;

-- Gameobject spawns
DELETE FROM `gameobject` WHERE `guid` BETWEEN @OGUID+0 AND @OGUID+83;
INSERT INTO `gameobject` (`guid`, `id`, `map`, `zoneId`, `areaId`, `spawnDifficulties`, `PhaseId`, `PhaseGroup`, `position_x`, `position_y`, `position_z`, `orientation`, `rotation0`, `rotation1`, `rotation2`, `rotation3`, `spawntimesecs`, `animprogress`, `state`, `VerifiedBuild`) VALUES
(@OGUID+0, 180405, 0, 28, 193, '0', 0, 0, 1510.329833984375, -1604.4375, 64.65036773681640625, 1.762782454490661621, 0, 0, 0.771624565124511718, 0.636078238487243652, 120, 255, 1, 51886), -- G_Pumpkin_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+1, 180405, 0, 28, 193, '0', 0, 0, 1509.3680419921875, -1645.9097900390625, 67.70035552978515625, 5.340708732604980468, 0, 0, -0.45398998260498046, 0.891006767749786376, 120, 255, 1, 51886), -- G_Pumpkin_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+2, 180405, 0, 28, 193, '0', 0, 0, 1574.217041015625, -1638.189208984375, 64.58261871337890625, 4.747295856475830078, 0, 0, -0.69465827941894531, 0.719339847564697265, 120, 255, 1, 51886), -- G_Pumpkin_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+3, 180406, 0, 28, 193, '0', 0, 0, 1556.0399169921875, -1622.0035400390625, 65.147705078125, 2.460912704467773437, 0, 0, 0.942641258239746093, 0.333807557821273803, 120, 255, 1, 51886), -- G_Pumpkin_02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+4, 180406, 0, 28, 193, '0', 0, 0, 1542.26220703125, -1641.3055419921875, 69.00669097900390625, 3.78736734390258789, 0, 0, -0.94832324981689453, 0.317305892705917358, 120, 255, 1, 51886), -- G_Pumpkin_02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+5, 180406, 0, 28, 193, '0', 0, 0, 1569.5086669921875, -1565.548583984375, 63.61168289184570312, 2.338739633560180664, 0, 0, 0.920504570007324218, 0.3907318115234375, 120, 255, 1, 51886), -- G_Pumpkin_02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+6, 180407, 0, 28, 193, '0', 0, 0, 1474.98095703125, -1603.5521240234375, 67.13401031494140625, 3.24634718894958496, 0, 0, -0.99862861633300781, 0.052353221923112869, 120, 255, 1, 51886), -- G_Pumpkin_03 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+7, 180407, 0, 28, 193, '0', 0, 0, 1461.9566650390625, -1566.0103759765625, 63.73649978637695312, 3.665196180343627929, 0, 0, -0.96592521667480468, 0.258821308612823486, 120, 255, 1, 51886), -- G_Pumpkin_03 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+8, 180410, 0, 28, 193, '0', 0, 0, 1476.5260009765625, -1608.873291015625, 69.42380523681640625, 1.221729278564453125, 0, 0, 0.573575973510742187, 0.819152355194091796, 120, 255, 1, 51886), -- G_HangingSkeleton_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+9, 180410, 0, 28, 193, '0', 0, 0, 1592.8472900390625, -1626.173583984375, 66.09983062744140625, 4.223697185516357421, 0, 0, -0.85716724395751953, 0.515038192272186279, 120, 255, 1, 51886), -- G_HangingSkeleton_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+10, 180411, 0, 28, 193, '0', 0, 0, 1531.9635009765625, -1643.171875, 72.74962615966796875, 5.881760597229003906, 0, 0, -0.19936752319335937, 0.979924798011779785, 120, 255, 1, 51886), -- G_Ghost_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+11, 180411, 0, 28, 193, '0', 0, 0, 1584.10595703125, -1623.85595703125, 67.35535430908203125, 5.881760597229003906, 0, 0, -0.19936752319335937, 0.979924798011779785, 120, 255, 1, 51886), -- G_Ghost_01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+12, 180415, 0, 28, 193, '0', 0, 0, 1477.8038330078125, -1602.654541015625, 67.19211578369140625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+13, 180415, 0, 28, 193, '0', 0, 0, 1473.265625, -1600.9566650390625, 67.07068634033203125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+14, 180415, 0, 28, 193, '0', 0, 0, 1525.732666015625, -1617.0260009765625, 65.1535491943359375, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+15, 180415, 0, 28, 193, '0', 0, 0, 1540.3333740234375, -1579.982666015625, 64.60166168212890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+16, 180415, 0, 28, 193, '0', 0, 0, 1484.810791015625, -1605.34033203125, 67.37509918212890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+17, 180415, 0, 28, 193, '0', 0, 0, 1501.4600830078125, -1614.9166259765625, 65.37836456298828125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+18, 180415, 0, 28, 193, '0', 0, 0, 1530.7413330078125, -1621.7691650390625, 65.63843536376953125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+19, 180415, 0, 28, 193, '0', 0, 0, 1532.251708984375, -1595.6458740234375, 65.07103729248046875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+20, 180415, 0, 28, 193, '0', 0, 0, 1534.4410400390625, -1600.640625, 65.16373443603515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+21, 180415, 0, 28, 193, '0', 0, 0, 1487.5069580078125, -1605.8385009765625, 67.31610107421875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+22, 180415, 0, 28, 193, '0', 0, 0, 1535.7135009765625, -1584.0399169921875, 64.30771636962890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+23, 180415, 0, 28, 193, '0', 0, 0, 1528.967041015625, -1618.8489990234375, 65.3953094482421875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+24, 180415, 0, 28, 193, '0', 0, 0, 1531.470458984375, -1590, 64.49961090087890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+25, 180415, 0, 28, 193, '0', 0, 0, 1531.2239990234375, -1626.4739990234375, 66.31255340576171875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+26, 180415, 0, 28, 193, '0', 0, 0, 1531.1458740234375, -1630.298583984375, 67.0563507080078125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+27, 180415, 0, 28, 193, '0', 0, 0, 1566.77783203125, -1584.16845703125, 63.88544464111328125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+28, 180415, 0, 28, 193, '0', 0, 0, 1531.2083740234375, -1633.9635009765625, 68.03165435791015625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+29, 180415, 0, 28, 193, '0', 0, 0, 1533.873291015625, -1653.859375, 67.91422271728515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+30, 180415, 0, 28, 193, '0', 0, 0, 1535.7117919921875, -1652.65283203125, 67.87796783447265625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+31, 180415, 0, 28, 193, '0', 0, 0, 1538.109375, -1652.513916015625, 67.88715362548828125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+32, 180415, 0, 28, 193, '0', 0, 0, 1520.6475830078125, -1671.3367919921875, 78.3869781494140625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+33, 180415, 0, 28, 193, '0', 0, 0, 1548.0972900390625, -1665.69970703125, 76.01551055908203125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+34, 180415, 0, 28, 193, '0', 0, 0, 1540.1180419921875, -1659.09375, 67.88117218017578125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+35, 180415, 0, 28, 193, '0', 0, 0, 1535.37158203125, -1678.732666015625, 74.97588348388671875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+36, 180415, 0, 28, 193, '0', 0, 0, 1536.0816650390625, -1660.579833984375, 67.8883056640625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+37, 180415, 0, 28, 193, '0', 0, 0, 1539.5191650390625, -1653.407958984375, 67.892578125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+38, 180415, 0, 28, 193, '0', 0, 0, 1533.251708984375, -1657.953125, 67.914337158203125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+39, 180415, 0, 28, 193, '0', 0, 0, 1569.732666015625, -1587.6475830078125, 64.0567169189453125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+40, 180415, 0, 28, 193, '0', 0, 0, 1534.4930419921875, -1659.54345703125, 67.873199462890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+41, 180415, 0, 28, 193, '0', 0, 0, 1541.0086669921875, -1657.71533203125, 67.91436767578125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+42, 180415, 0, 28, 193, '0', 0, 0, 1570.8160400390625, -1592.7430419921875, 64.03205108642578125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+43, 180415, 0, 28, 193, '0', 0, 0, 1527.2447509765625, -1667.30908203125, 76.02330780029296875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+44, 180415, 0, 28, 193, '0', 0, 0, 1538.7760009765625, -1660.2742919921875, 67.89048004150390625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+45, 180415, 0, 28, 193, '0', 0, 0, 1540.8367919921875, -1655.295166015625, 67.9233856201171875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+46, 180415, 0, 28, 193, '0', 0, 0, 1533.0035400390625, -1655.376708984375, 67.9141845703125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+47, 180415, 0, 28, 193, '0', 0, 0, 1570.6927490234375, -1598.2847900390625, 64.24945831298828125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+48, 180415, 0, 28, 193, '0', 0, 0, 1537.41845703125, -1688.642333984375, 57.4022369384765625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+49, 180415, 0, 28, 193, '0', 0, 0, 1539.0885009765625, -1676.4739990234375, 75.08632659912109375, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+50, 180415, 0, 28, 193, '0', 0, 0, 1537.517333984375, -1685.7569580078125, 57.3892822265625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+51, 180415, 0, 28, 193, '0', 0, 0, 1555.2066650390625, -1668.282958984375, 78.36240386962890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+52, 180415, 0, 28, 193, '0', 0, 0, 1543.27783203125, -1678.09033203125, 74.98204803466796875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+53, 180415, 0, 28, 193, '0', 0, 0, 1542.265625, -1685.484375, 57.39056396484375, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+54, 180415, 0, 28, 193, '0', 0, 0, 1577.0347900390625, -1625.703125, 64.4201202392578125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+55, 180415, 0, 28, 193, '0', 0, 0, 1580.7257080078125, -1620.1129150390625, 64.1438446044921875, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+56, 180415, 0, 28, 193, '0', 0, 0, 1542.6771240234375, -1688.30908203125, 57.40257644653320312, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+57, 180415, 0, 28, 193, '0', 0, 0, 1542.2274169921875, -1691.578125, 57.4135284423828125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+58, 180415, 0, 28, 193, '0', 0, 0, 1538.3697509765625, -1691.6649169921875, 57.41134262084960937, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- CandleBlack01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+59, 180425, 0, 28, 193, '0', 0, 0, 1500.5625, -1614.8160400390625, 65.396026611328125, 1.553341388702392578, 0, 0, 0.700908660888671875, 0.713251054286956787, 120, 255, 1, 51886), -- SkullCandle01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+60, 180425, 0, 28, 193, '0', 0, 0, 1583.0677490234375, -1619.4635009765625, 65.07834625244140625, 2.72271275520324707, 0, 0, 0.978147506713867187, 0.207912087440490722, 120, 255, 1, 51886), -- SkullCandle01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+61, 180426, 0, 28, 193, '0', 0, 0, 1527.767333984375, -1618.3385009765625, 79.14371490478515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+62, 180426, 0, 28, 193, '0', 0, 0, 1540.23095703125, -1613.4010009765625, 79.63944244384765625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+63, 180426, 0, 28, 193, '0', 0, 0, 1535.078125, -1585.439208984375, 79.14371490478515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+64, 180426, 0, 28, 193, '0', 0, 0, 1557.8021240234375, -1571.3038330078125, 79.16574859619140625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+65, 180426, 0, 28, 193, '0', 0, 0, 1517.671875, -1599.46875, 79.14371490478515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+66, 180426, 0, 28, 193, '0', 0, 0, 1561.8472900390625, -1615.60595703125, 78.85678863525390625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+67, 180427, 0, 28, 193, '0', 0, 0, 1530.26220703125, -1582.954833984375, 78.72916412353515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+68, 180427, 0, 28, 193, '0', 0, 0, 1527.498291015625, -1609.142333984375, 79.94260406494140625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+69, 180427, 0, 28, 193, '0', 0, 0, 1548.4444580078125, -1603.9566650390625, 79.35282135009765625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+70, 180427, 0, 28, 193, '0', 0, 0, 1560.060791015625, -1604.248291015625, 79.29160308837890625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+71, 180427, 0, 28, 193, '0', 0, 0, 1552.9635009765625, -1620.826416015625, 78.72916412353515625, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Bat02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+72, 180471, 0, 28, 193, '0', 0, 0, 1479.3194580078125, -1604.0885009765625, 75.38874053955078125, 2.775068521499633789, 0, 0, 0.983254432678222656, 0.182238012552261352, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+73, 180471, 0, 28, 193, '0', 0, 0, 1480.1492919921875, -1604.4478759765625, 75.42173004150390625, 2.740161895751953125, 0, 0, 0.979924201965332031, 0.199370384216308593, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+74, 180471, 0, 28, 193, '0', 0, 0, 1482.390625, -1605.361083984375, 75.37177276611328125, 2.740161895751953125, 0, 0, 0.979924201965332031, 0.199370384216308593, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+75, 180471, 0, 28, 193, '0', 0, 0, 1481.5260009765625, -1605.0086669921875, 75.41416168212890625, 2.583080768585205078, 0, 0, 0.961260795593261718, 0.275640487670898437, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+76, 180471, 0, 28, 193, '0', 0, 0, 1580.3629150390625, -1622.1944580078125, 71.625762939453125, 4.258606910705566406, 0, 0, -0.84804725646972656, 0.529920578002929687, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+77, 180471, 0, 28, 193, '0', 0, 0, 1579.0260009765625, -1624.796875, 71.61466217041015625, 4.363324165344238281, 0, 0, -0.81915187835693359, 0.573576688766479492, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+78, 180471, 0, 28, 193, '0', 0, 0, 1579.6492919921875, -1627.0885009765625, 71.55998992919921875, 5.078907966613769531, 0, 0, -0.56640625, 0.824126183986663818, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+79, 180471, 0, 28, 193, '0', 0, 0, 1582.5103759765625, -1621.7413330078125, 71.54453277587890625, 2.967041015625, 0, 0, 0.996193885803222656, 0.087165042757987976, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+80, 180472, 0, 28, 193, '0', 0, 0, 1480.8697509765625, -1604.720458984375, 76.133056640625, 0.471238493919372558, 0, 0, 0.233445167541503906, 0.972369968891143798, 120, 255, 1, 51886), -- HangingSkullLight02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+81, 180472, 0, 28, 193, '0', 0, 0, 1579.657958984375, -1623.453125, 72.98947906494140625, 0.471238493919372558, 0, 0, 0.233445167541503906, 0.972369968891143798, 120, 255, 1, 51886), -- HangingSkullLight02 (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+82, 180523, 0, 28, 193, '0', 0, 0, 1532.7586669921875, -1660.5625, 68.35138702392578125, 0, 0, 0, 0, 1, 120, 255, 1, 51886), -- Apple Bob (Area: Andorhal - Difficulty: 0) CreateObject1
(@OGUID+83, 208156, 0, 28, 193, '0', 0, 0, 1541.935791015625, -1659.4739990234375, 67.92234039306640625, 3.508116960525512695, 0, 0, -0.98325443267822265, 0.182238012552261352, 120, 255, 1, 51886); -- Candy Bucket (Area: Andorhal - Difficulty: 0) CreateObject1

-- Event spawns
DELETE FROM `game_event_gameobject` WHERE `eventEntry`=12 AND `guid` BETWEEN @OGUID+0 AND @OGUID+83;
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
(12, @OGUID+83);