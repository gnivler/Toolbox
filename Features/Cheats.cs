using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Toolbox.Features
{
    public static class Cheats
    {
        internal static bool Cheat;
        private static UpgradePanel upgradePanel;

        // cheat
        [HarmonyPatch(typeof(CraftingPanel), "OnRecipeSlotPress")]
        [HarmonyPrefix]
        public static void CraftingPanelOnRecipeSlotPressPrefix(RecipeSlot recipeSlot)
        {
            if (Cheat)
            {
                foreach (var stack in recipeSlot.recipe.ingredients)
                {
                    if (stack.isEdiable)
                    {
                        Mod.Log($"Adding edible {stack.def.name}");
                        PantryManager.instance.AddItem(stack);
                    }
                    else if (stack.def.category == ItemDef.Category.Medical)
                    {
                        MedicineCabinetManager.instance.AddItem(stack);
                    }
                    else
                    {
                        Mod.Log($"Adding item {stack.def.name}");
                        ShelterInventoryManager.instance.inventory.AddItems(stack);
                    }
                }

                var removalList = new List<List<ItemStack>>();
                foreach (var item in ShelterInventoryManager.instance.inventory.inventory.Values)
                {
                    if (item.Any(i => i.isEdiable || i.def.type == ItemDef.ItemType.Medicine))
                    {
                        removalList.Add(item);
                    }
                }

                foreach (var itemList in removalList)
                {
                    foreach (var item in itemList)
                    {
                        Mod.Log($"Moving {item.def.name}");
                        PantryManager.instance.AddItem(item);
                        ShelterInventoryManager.instance.inventory.RemoveItems(item, item.amount);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UpgradePanel), "OnOpen")]
        [HarmonyPostfix]
        public static void UpgradePanelAwake(UpgradePanel __instance)
        {
            upgradePanel ??= __instance;
        }

        [HarmonyPatch(typeof(UpgradePanel), "CraftSelectedUpgrade")]
        [HarmonyPrefix]
        public static void UpgradePanelOnUpgradeClicked()
        {
            if (Cheat)
            {
                var recipe = upgradePanel.objUpgrade.BuildRecipeForUpgrade(upgradePanel.m_selectedGrid.path_type, upgradePanel.m_selectedLevel);
                foreach (var stack in recipe.ingredients)
                {
                    ShelterInventoryManager.instance.inventory.AddItems(stack);
                }
            }
        }

        [HarmonyPatch(typeof(QuestManager), "IsFactionQuestComplete")]
        [HarmonyPostfix]
        public static void QuestManagerIsFactionQuestComplete(ref bool __result)
        {
            if (Input.GetKey(KeyCode.F3))
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(AIFaction), "GetPredictedQuestScore")]
        [HarmonyPrefix]
        public static bool AIFactionGetPredictedQuestScore(ref bool __runOriginal, AIFaction __instance, ref int __result)
        {
            if (Input.GetKey(KeyCode.F3))
            {
                __result = __instance.m_questScore_Stage3;
                __runOriginal = false;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(RadioPanel), "IsQuestComplete")]
        [HarmonyPostfix]
        public static void RadioPanelIsQuestComplete(RadioPanel __instance, ref bool __result)
        {
            if (Input.GetKey(KeyCode.F3))
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(RadioPanel), "OnRequestAlliance")]
        [HarmonyPrefix]
        public static void RadioPanelOnRequestAlliance(RadioPanel __instance)
        {
            if (Input.GetKey(KeyCode.F3))
            {
                __instance.m_chosenFaction.m_playerRep = 500;
                __instance.m_chosenFaction.m_currentFactionQuestStage = AIFaction.FactionQuestStage.Completed;
                __instance.m_currentDialogueStage = RadioPanel.DialogueStage.RequestAlliancePass;
            }
        }

        [HarmonyPatch(typeof(BaseCharacter), "SetUpSpawn")]
        [HarmonyPostfix]
        public static void BaseCharacterSetUpSpawnPostfix(BaseCharacter __instance)
        {
            if (Cheat
                && __instance.isShelterMember)
            {
                Mod.Log("Max " + __instance.name);
                MaxEverythingOut(__instance.baseRH.memberNavigation.member);
            }
        }

        [HarmonyPatch(typeof(Member), "SaveLoad")]
        [HarmonyPostfix]
        public static void MemberSaveLoad(Member __instance)
        {
            if (Cheat
                && __instance.isShelterMember)
            {
                Mod.Log("Max " + __instance.name);
                MaxEverythingOut(__instance);
            }
        }

        private static void MaxEverythingOut(Member member)
        {
            member.baseStats.strength.level = 20;
            member.baseStats.strength.cap = 20;
            member.baseStats.dexterity.level = 20;
            member.baseStats.dexterity.cap = 20;
            member.baseStats.charisma.level = 20;
            member.baseStats.charisma.cap = 20;
            member.baseStats.intelligence.level = 20;
            member.baseStats.intelligence.cap = 20;
            member.baseStats.perception.level = 20;
            member.baseStats.perception.cap = 20;
            member.baseStats.fortitude.level = 20;
            member.baseStats.fortitude.cap = 20;
            member.SetupProfession(new Profession(member.baseRH.baseCharacter)
            {
                m_strengthSkills = new Dictionary<ProfessionsManager.ProfessionSkillType, SkillUpgradeLevels>
                {
                    { ProfessionsManager.ProfessionSkillType.CrushWindpipe, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PoisonPunch, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.BackpackWeightTraining, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PumpUp, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.InherentStrength, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Demoralise, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.BluntForceSpecialisation, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ImposingPhysique, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Headbutt, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.SetBone, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Kick, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ShoulderBarge, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.UtilitySpecialist, new SkillUpgradeLevels(2, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ExplodingHeartAttack, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ThunderousUppercut, new SkillUpgradeLevels(3, 0, 0, 0, 0) }
                },
                m_dexteritySkills = new Dictionary<ProfessionsManager.ProfessionSkillType, SkillUpgradeLevels>
                {
                    { ProfessionsManager.ProfessionSkillType.SprayGunshot, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.RangedWeaponTraining, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.FastReflexes, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.BladeSpecialisation, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.FlickSandAttack, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.SleightOfHandAttack, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.AimedGunshot, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.KnickArtery, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.RetreatAttack, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Backstab, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Disarm, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.CQCTraining, new SkillUpgradeLevels(1, 0, 0, 0, 0) }
                },
                m_intelligenceSkills = new Dictionary<ProfessionsManager.ProfessionSkillType, SkillUpgradeLevels>
                {
                    { ProfessionsManager.ProfessionSkillType.EmergencyTourniquet, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Focussed, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.MedicalTraining, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Tactician, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PuttingOnABraveFace, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ThickSkinned, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.DistractionTactics, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.MentalFortifications, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.EmergencyHealing, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.CPR, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.KnowledgeOfAnatomy, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.CombatAnalysis, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ResourcefulHealing, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.CalculatedOneTwo, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Surgeon, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Experiment, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ImproviseExplosive, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                },
                m_charismaSkills = new Dictionary<ProfessionsManager.ProfessionSkillType, SkillUpgradeLevels>
                {
                    { ProfessionsManager.ProfessionSkillType.SoothingWords, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Inspiring, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.BedsideManner, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Motivator, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Welcoming, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ProductionManager, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ConvincingVoice, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ConfuseOpponent, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.MarchingSongs, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PlaceboEffect, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.MissionOfMercy, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.SilverTongue, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Rallying, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                },
                m_perceptionSkills = new Dictionary<ProfessionsManager.ProfessionSkillType, SkillUpgradeLevels>
                {
                    { ProfessionsManager.ProfessionSkillType.AssessOpponent, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.AlwaysPrepared, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Unshakeable, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.StudyMovements, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.QuickStudy, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ExpeditedHealing, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PoisonResillience, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Taunt, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Autopsy, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.EideticMemory, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.RelishesAChallenge, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Hunter, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Therapist, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ReturnToSender, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.AutomaticRepairing, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Demoralise, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.LocateWeakpoint, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                },
                m_fortitudeSkills = new Dictionary<ProfessionsManager.ProfessionSkillType, SkillUpgradeLevels>
                {
                    { ProfessionsManager.ProfessionSkillType.ExtractPoison, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PainResistanceTraining, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.ShakeItOff, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.IronStomach, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.StrongImmuneSystem, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.FastHealer, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.TirelessEngineering, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.HomeTurfAdvantage, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.UnarmedSpecialisation, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.BloodTransfusion, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.HardenedSkin, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Valiant, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.SharedHealing, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.WorkingLongHours, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.WarmBlooded, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.RageAttack, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.FinalCounterDown, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.DeterminedToWin, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.Hardy, new SkillUpgradeLevels(1, 0, 0, 0, 0) },
                    { ProfessionsManager.ProfessionSkillType.PatchYourselfUp, new SkillUpgradeLevels(3, 0, 0, 0, 0) },
                }
            });

            member.UpdateMaxHealth(member.baseStats.Fortitude.Level);
            member.Heal(1000);
        }
    }
}
