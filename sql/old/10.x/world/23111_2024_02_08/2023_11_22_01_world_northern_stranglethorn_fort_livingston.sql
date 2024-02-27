SET @OGUID := 3005489;

-- Gameobject templates
UPDATE `gameobject_template` SET `ContentTuningId`=425, `VerifiedBuild`=51886 WHERE `entry`=208125; -- Candy Bucket

UPDATE `gameobject_template_addon` SET `faction`=1732 WHERE `entry`=208125; -- Candy Bucket

-- Quests
DELETE FROM `quest_offer_reward` WHERE `ID`=28964;
INSERT INTO `quest_offer_reward` (`ID`, `Emote1`, `Emote2`, `Emote3`, `Emote4`, `EmoteDelay1`, `EmoteDelay2`, `EmoteDelay3`, `EmoteDelay4`, `RewardText`, `VerifiedBuild`) VALUES
(28964, 0, 0, 0, 0, 0, 0, 0, 0, 'Candy buckets like this are located in inns throughout the realms. Go ahead... take some!', 51886); -- Candy Bucket

DELETE FROM `gameobject_queststarter` WHERE `id`=208125;
INSERT INTO `gameobject_queststarter` (`id`, `quest`, `VerifiedBuild`) VALUES
(208125, 28964, 51886);

UPDATE `gameobject_questender` SET `VerifiedBuild`=51886 WHERE (`id`=208125 AND `quest`=28964);

DELETE FROM `game_event_gameobject_quest` WHERE `id`=208125;

-- Gameobject spawns
DELETE FROM `gameobject` WHERE `guid` BETWEEN @OGUID+0 AND @OGUID+32;
INSERT INTO `gameobject` (`guid`, `id`, `map`, `zoneId`, `areaId`, `spawnDifficulties`, `PhaseId`, `PhaseGroup`, `position_x`, `position_y`, `position_z`, `orientation`, `rotation0`, `rotation1`, `rotation2`, `rotation3`, `spawntimesecs`, `animprogress`, `state`, `VerifiedBuild`) VALUES
(@OGUID+0, 180405, 0, 33, 5320, '0', 0, 0, -12827.6494140625, -401.064239501953125, 12.95209598541259765, 3.560472726821899414, 0, 0, -0.97814750671386718, 0.207912087440490722, 120, 255, 1, 51886), -- G_Pumpkin_01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+1, 180405, 0, 33, 5320, '0', 0, 0, -12805.5244140625, -436.21527099609375, 13.0759744644165039, 1.448621988296508789, 0, 0, 0.662619590759277343, 0.748956084251403808, 120, 255, 1, 51886), -- G_Pumpkin_01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+2, 180406, 0, 33, 5320, '0', 0, 0, -12836.8642578125, -401.423614501953125, 12.95469093322753906, 3.176533222198486328, 0, 0, -0.999847412109375, 0.017469281330704689, 120, 255, 1, 51886), -- G_Pumpkin_02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+3, 180406, 0, 33, 5320, '0', 0, 0, -12860.87890625, -417.982635498046875, 12.96472644805908203, 5.113816738128662109, 0, 0, -0.55193614959716796, 0.833886384963989257, 120, 255, 1, 51886), -- G_Pumpkin_02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+4, 180407, 0, 33, 5320, '0', 0, 0, -12809.0068359375, -445.875, 12.96668052673339843, 1.902408957481384277, 0, 0, 0.814115524291992187, 0.580702960491180419, 120, 255, 1, 51886), -- G_Pumpkin_03 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+5, 180407, 0, 33, 5320, '0', 0, 0, -12861.9306640625, -427.234375, 13.21140289306640625, 4.572763919830322265, 0, 0, -0.75470924377441406, 0.656059443950653076, 120, 255, 1, 51886), -- G_Pumpkin_03 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+6, 180408, 0, 33, 5320, '0', 0, 0, -12818.767578125, -422.90277099609375, 18.00031089782714843, 5.044002056121826171, 0, 0, -0.58070278167724609, 0.814115643501281738, 120, 255, 1, 51886), -- G_WitchHat_01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+7, 180415, 0, 33, 5320, '0', 0, 0, -12841.625, -411.02777099609375, 13.66728687286376953, 5.235987663269042968, 0, 0, -0.5, 0.866025388240814208, 120, 255, 1, 51886), -- CandleBlack01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+8, 180415, 0, 33, 5320, '0', 0, 0, -12841.705078125, -411.232635498046875, 13.61534976959228515, 0.802850961685180664, 0, 0, 0.390730857849121093, 0.920504987239837646, 120, 255, 1, 51886), -- CandleBlack01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+9, 180415, 0, 33, 5320, '0', 0, 0, -12811.4892578125, -428.375, 15.11994075775146484, 5.393068790435791015, 0, 0, -0.43051052093505859, 0.902585566043853759, 120, 255, 1, 51886), -- CandleBlack01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+10, 180415, 0, 33, 5320, '0', 0, 0, -12811.5595703125, -428.661468505859375, 15.00187492370605468, 0.802850961685180664, 0, 0, 0.390730857849121093, 0.920504987239837646, 120, 255, 1, 51886), -- CandleBlack01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+11, 180415, 0, 33, 5320, '0', 0, 0, -12811.2470703125, -428.560760498046875, 15.18839359283447265, 0.418878614902496337, 0, 0, 0.207911491394042968, 0.978147625923156738, 120, 255, 1, 51886), -- CandleBlack01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+12, 180425, 0, 33, 5320, '0', 0, 0, -12821.5732421875, -425.86285400390625, 13.91074752807617187, 3.211419343948364257, 0, 0, -0.9993906021118164, 0.034906134009361267, 120, 255, 1, 51886), -- SkullCandle01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+13, 180425, 0, 33, 5320, '0', 0, 0, -12847.0419921875, -418.032989501953125, 13.86970996856689453, 5.462882041931152343, 0, 0, -0.39874839782714843, 0.917060375213623046, 120, 255, 1, 51886), -- SkullCandle01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+14, 180426, 0, 33, 5320, '0', 0, 0, -12837.603515625, -426.21875, 18.19655227661132812, 1.32644820213317871, 0, 0, 0.615660667419433593, 0.788011372089385986, 120, 255, 1, 51886), -- Bat01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+15, 180426, 0, 33, 5320, '0', 0, 0, -12840.53125, -429.220489501953125, 21.11986732482910156, 4.97418975830078125, 0, 0, -0.60876083374023437, 0.793353796005249023, 120, 255, 1, 51886), -- Bat01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+16, 180426, 0, 33, 5320, '0', 0, 0, -12834.5693359375, -424.203125, 19.83699989318847656, 2.44346022605895996, 0, 0, 0.939692497253417968, 0.34202045202255249, 120, 255, 1, 51886), -- Bat01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+17, 180426, 0, 33, 5320, '0', 0, 0, -12836.919921875, -424.848968505859375, 21.17567062377929687, 0.104719325900077819, 0, 0, 0.052335739135742187, 0.998629570007324218, 120, 255, 1, 51886), -- Bat01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+18, 180427, 0, 33, 5320, '0', 0, 0, -12839.919921875, -425.482635498046875, 19.43795394897460937, 3.769911527633666992, 0, 0, -0.95105648040771484, 0.309017121791839599, 120, 255, 1, 51886), -- Bat02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+19, 180427, 0, 33, 5320, '0', 0, 0, -12833.607421875, -429.9288330078125, 17.53478813171386718, 5.270895957946777343, 0, 0, -0.48480892181396484, 0.87462007999420166, 120, 255, 1, 51886), -- Bat02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+20, 180427, 0, 33, 5320, '0', 0, 0, -12835.6005859375, -427.7569580078125, 26.12606239318847656, 4.48549652099609375, 0, 0, -0.7826080322265625, 0.622514784336090087, 120, 255, 1, 51886), -- Bat02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+21, 180471, 0, 33, 5320, '0', 0, 0, -12823.9033203125, -420.399322509765625, 16.11289787292480468, 4.468043327331542968, 0, 0, -0.7880105972290039, 0.615661680698394775, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+22, 180471, 0, 33, 5320, '0', 0, 0, -12820.955078125, -428.20660400390625, 16.10209083557128906, 5.881760597229003906, 0, 0, -0.19936752319335937, 0.979924798011779785, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+23, 180471, 0, 33, 5320, '0', 0, 0, -12830.8134765625, -445.515625, 15.13029289245605468, 2.251473426818847656, 0, 0, 0.902585029602050781, 0.430511653423309326, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+24, 180471, 0, 33, 5320, '0', 0, 0, -12831.0068359375, -449.727447509765625, 15.24355411529541015, 0.401424884796142578, 0, 0, 0.199367523193359375, 0.979924798011779785, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+25, 180471, 0, 33, 5320, '0', 0, 0, -12813.22265625, -425.282989501953125, 16.12880516052246093, 0.785396754741668701, 0, 0, 0.38268280029296875, 0.923879802227020263, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+26, 180471, 0, 33, 5320, '0', 0, 0, -12816.0634765625, -417.4757080078125, 16.08460426330566406, 2.879789113998413085, 0, 0, 0.991444587707519531, 0.130528271198272705, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+27, 180471, 0, 33, 5320, '0', 0, 0, -12836.4130859375, -445.298614501953125, 15.17396068572998046, 4.031712055206298828, 0, 0, -0.90258502960205078, 0.430511653423309326, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+28, 180471, 0, 33, 5320, '0', 0, 0, -12836.5869140625, -449.477447509765625, 15.21264553070068359, 5.602506637573242187, 0, 0, -0.33380699157714843, 0.942641437053680419, 120, 255, 1, 51886), -- HangingSkullLight01 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+29, 180472, 0, 33, 5320, '0', 0, 0, -12822.146484375, -424.140625, 16.56383705139160156, 3.665196180343627929, 0, 0, -0.96592521667480468, 0.258821308612823486, 120, 255, 1, 51886), -- HangingSkullLight02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+30, 180472, 0, 33, 5320, '0', 0, 0, -12837.576171875, -447.34027099609375, 17.73743438720703125, 2.199114561080932617, 0, 0, 0.8910064697265625, 0.453990638256072998, 120, 255, 1, 51886), -- HangingSkullLight02 (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+31, 180523, 0, 33, 5320, '0', 0, 0, -12853.28515625, -435.54339599609375, 13.22400093078613281, 2.059488296508789062, 0, 0, 0.857167243957519531, 0.515038192272186279, 120, 255, 1, 51886), -- Apple Bob (Area: Fort Livingston - Difficulty: 0) CreateObject1
(@OGUID+32, 208125, 0, 33, 5320, '0', 0, 0, -12847.4755859375, -435.87152099609375, 12.96668052673339843, 2.478367090225219726, 0, 0, 0.94551849365234375, 0.325568377971649169, 120, 255, 1, 51886); -- Candy Bucket (Area: Fort Livingston - Difficulty: 0) CreateObject1

-- Event spawns
DELETE FROM `game_event_gameobject` WHERE `eventEntry`=12 AND `guid` BETWEEN @OGUID+0 AND @OGUID+32;
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
(12, @OGUID+32);