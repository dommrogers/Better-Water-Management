using Il2Cpp;
using Il2CppTLD.Cooking;
using HarmonyLib;
using ModComponent.Utils;
using UnityEngine;

namespace BetterWaterManagement;

internal class MeltAndCookButton
{
	internal static string text;
	private static GameObject button;

	public static void Execute()
	{
		Panel_CookWater panel_CookWater = InterfaceManager.GetPanel<Panel_CookWater>();
		GearItem cookedItem = panel_CookWater.GetSelectedFood();
		CookingPotItem cookingPotItem = panel_CookWater.m_CookingPotInteractedWith;
		CookSettings cookSettings = panel_CookWater.m_CookSettings;

		GearItem result = cookedItem.Drop(1, false, true);

		CookingModifier cookingModifier = CookingUtils.GetOrCreateComponent<CookingModifier>(result);
		cookingModifier.additionalMinutes = result.m_Cookable.m_PotableWaterRequiredLiters * cookSettings.m_MinutesToMeltSnowPerLiter;
		cookingModifier.Apply();

		GameAudioManager.Play3DSound(result.m_Cookable.PutInPotAudio, cookingPotItem.gameObject);
		cookingPotItem.StartCooking(result);
		panel_CookWater.ExitCookingInterface();
	}

	internal static void Initialize(Panel_CookWater panel_CookWater)
	{
		text = Localization.Get("GAMEPLAY_ButtonMelt") + " & " + Localization.Get("GAMEPLAY_ButtonCook");

		button = UnityEngine.Object.Instantiate<GameObject>(panel_CookWater.m_ActionButtonObject, panel_CookWater.m_ActionButtonObject.transform.parent, true);
		button.transform.Translate(0, 0.09f, 0);
		Utils.GetComponentInChildren<UILabel>(button).text = text;
		Il2CppSystem.Collections.Generic.List<EventDelegate> placeHolderList = new Il2CppSystem.Collections.Generic.List<EventDelegate>();
		placeHolderList.Add(new EventDelegate(new System.Action(Execute)));
		Utils.GetComponentInChildren<UIButton>(button).onClick = placeHolderList;

		NGUITools.SetActive(button, false);
	}

	internal static void SetActive(bool active)
	{
		NGUITools.SetActive(button, active);
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.RefreshFoodList))]
internal class Panel_CookWater_RefreshFoodList
{
	internal static void Postfix(Panel_CookWater __instance)
	{
		Il2CppSystem.Collections.Generic.List<GearItem> foodList = __instance.m_FoodList;
		if (foodList == null)
		{
			return;
		}

		foreach (GearItem eachGearItem in foodList)
		{
			CookingModifier cookingModifier = CookingUtils.GetComponentSafe<CookingModifier>(eachGearItem);
			cookingModifier?.Revert();
			//if(cookingModifier) Implementation.Log("{0} reverted from Melt and Cook", eachGearItem.name);
		}
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.Initialize))]
internal class Panel_CookWater_Initialize
{
	internal static void Postfix(Panel_CookWater __instance)
	{
		MeltAndCookButton.Initialize(__instance);
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.UpdateButtonLegend))]
internal class Panel_CookWater_UpdateButtonLegend
{
	internal static void Prefix(Panel_CookWater __instance)
	{
		GearItem cookedItem = __instance.GetSelectedFood();
		bool requiresWater = (cookedItem?.m_Cookable?.m_PotableWaterRequiredLiters ?? 0) > 0;

		if (Utils.IsMouseActive())
		{
			MeltAndCookButton.SetActive(requiresWater);
		}
		else
		{
			__instance.m_ButtonLegendContainer.BeginUpdate();
			__instance.m_ButtonLegendContainer.UpdateButton("Inventory_Drop", MeltAndCookButton.text, requiresWater, 2, false);
		}
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.UpdateGamepadControls))]
internal class Panel_CookWater_UpdateGamepadControls
{
	internal static bool Prefix(Panel_CookWater __instance)
	{
		if (!InputManager.GetInventoryDropPressed(GameManager.Instance()))
		{
			return true;
		}

		GearItem cookedItem = __instance.GetSelectedFood();
		bool requiresWater = (cookedItem?.m_Cookable?.m_PotableWaterRequiredLiters ?? 0) > 0;
		if (!requiresWater)
		{
			return true;
		}

		MeltAndCookButton.Execute();
		return false;
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.UpdateGearItem))]
internal class Panel_CookWater_UpdateGearItem
{
	internal static void Postfix(Panel_CookWater __instance)
	{
		CookSettings cookSettings = __instance.m_CookSettings;
		GearItem cookedItem = __instance.GetSelectedFood();
		if (cookedItem == null || cookedItem.m_Cookable == null)
		{
			return;
		}

		CookingPotItem cookingPotItem = __instance.m_CookingPotInteractedWith;
		if (cookingPotItem == null)
		{
			return;
		}

		if (cookedItem.m_Cookable.m_PotableWaterRequiredLiters <= 0)
		{
			return;
		}

		float litersRequired = cookedItem.m_Cookable.m_PotableWaterRequiredLiters;
		float additionalMinutes = litersRequired * cookSettings.m_MinutesToMeltSnowPerLiter * cookingPotItem.GetTotalCookMultiplier();

		__instance.m_Label_CookedItemCookTime.text = GetCookingTime(cookedItem.m_Cookable.m_CookTimeMinutes * cookingPotItem.GetTotalCookMultiplier()) + " (+" + GetCookingTime(additionalMinutes) + " " + Localization.Get("GAMEPLAY_ButtonMelt") + ")";
	}

	private static string GetCookingTime(float minutes)
	{
		if (minutes < 60)
		{
			return Utils.GetExpandedDurationString(Mathf.RoundToInt(minutes));
		}

		return Utils.GetDurationString(Mathf.RoundToInt(minutes));
	}
}
