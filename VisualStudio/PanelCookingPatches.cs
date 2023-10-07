using Il2Cpp;
using HarmonyLib;

namespace BetterWaterManagement;

//
//These patches make the cook water menu increment by a custom amount instead of the default 0.5 L
//
internal class PanelCookingPatches
{
	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnBoilDown))]
	internal class Panel_CookWater_OnBoilDown
	{
		private static void Prefix(Panel_CookWater __instance, out float __state)
		{
			__state = __instance.m_BoilWaterLiters;
		}
		private static void Postfix(Panel_CookWater __instance, float __state)
		{
			//Implementation.Log("OnBoilDown");
			if (__state != __instance.m_BoilWaterLiters)
			{
				__instance.m_BoilWaterLiters = __state - Settings.GetWaterIncrement();
				__instance.ClampWaterBoilAmount();
			}
		}
	}
	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnBoilUp))]
	internal class Panel_CookWater_OnBoilUp
	{
		private static void Prefix(Panel_CookWater __instance, out float __state)
		{
			__state = __instance.m_BoilWaterLiters;
		}
		private static void Postfix(Panel_CookWater __instance, float __state)
		{
			//Implementation.Log("OnBoilUp");
			if (__state != __instance.m_BoilWaterLiters)
			{
				__instance.m_BoilWaterLiters = __state + Settings.GetWaterIncrement();
				__instance.ClampWaterBoilAmount();
			}
		}
	}
	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnMeltSnowDown))]
	internal class Panel_CookWater_OnMeltSnowDown
	{
		private static void Prefix(Panel_CookWater __instance, out float __state)
		{
			__state = __instance.m_MeltSnowLiters;
		}
		private static void Postfix(Panel_CookWater __instance, float __state)
		{
			//Implementation.Log("OnMeltSnowDown");
			if (__state != __instance.m_MeltSnowLiters)
			{
				__instance.m_MeltSnowLiters = __state - Settings.GetWaterIncrement();
				__instance.ClampMeltSnowAmount();
			}
		}
	}
	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnMeltSnowUp))]
	internal class Panel_CookWater_OnMeltSnowUp
	{
		private static void Prefix(Panel_CookWater __instance, out float __state)
		{
			__state = __instance.m_MeltSnowLiters;
		}
		private static void Postfix(Panel_CookWater __instance, float __state)
		{
			//Implementation.Log("OnMeltSnowUp");
			if (__state != __instance.m_MeltSnowLiters)
			{
				__instance.m_MeltSnowLiters = __state + Settings.GetWaterIncrement();
				__instance.ClampMeltSnowAmount();
			}
		}
	}
}
