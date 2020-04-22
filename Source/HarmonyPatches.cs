using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using HarmonyLib;
using RimWorld;
using Verse;

namespace SquadUITweaks
{
        [StaticConstructorOnStartup]
    public class HarmonyPatches
    {
        static HarmonyPatches()
            {
                var harmony = new Harmony("drumad.rimworld.mod.squadUItweaks");

                var thistype = typeof(HarmonyPatches);

                harmony.Patch(
                    AccessTools.Method(typeof(PawnAttackGizmoUtility),
                        nameof(PawnAttackGizmoUtility.CanShowEquipmentGizmos)), null,
                    new HarmonyMethod(thistype, nameof(CanShowEquipmentGizmos_postfix)),
                    
                    null);
                harmony.Patch(AccessTools.Method(typeof(PawnAttackGizmoUtility), "ShouldUseSquadAttackGizmo"),
                    new HarmonyMethod(thistype, nameof(ShouldUseSquadAttackGizmo_Postfix)),
                    null,
                    null);
                harmony.Patch(
                    AccessTools.Method(typeof(PawnAttackGizmoUtility),
                        "GetSquadAttackGizmo"), null, new HarmonyMethod(thistype,
                        nameof(GetSquadAttackGizmo_Postfix)));
        }

        //[TweakValue("UIImprovements", 0, 1)]
        //public static float CanShowEquipment_retVal_0isFalse = 1f;
        public static void CanShowEquipmentGizmos_postfix(ref bool __result)
            {
                __result = /*CanShowEquipment_retVal_0isFalse > 0.5f*/ true;
            }

        public static void ShouldUseSquadAttackGizmo_Postfix(ref bool __result)
            {
                if (__result)
                    {
                        return;
                    }

                 __result = (bool)Traverse.Create(typeof(PawnAttackGizmoUtility)).Method("AtLeastOneSelectedColonistHasRangedWeapon").GetValue();
            }


        //TODO just wrapping this might be better?
        public static void GetSquadAttackGizmo_Postfix(
            ref Gizmo __result)
        {
            if (__result is Command_Target command)
                {
                    __result = new Command_Target_Extended(command);
                }
        }
    }
}
    