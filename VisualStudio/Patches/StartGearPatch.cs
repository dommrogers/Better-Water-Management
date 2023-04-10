using Il2Cpp;
using Il2CppTLD.Gear;
using HarmonyLib;

namespace BetterWaterManagement;

internal class StartGearPatch
{
	[HarmonyPatch(typeof(StartGear), nameof(StartGear.AddAllToInventory))]
	internal class StartGear_AddAllToInventory
	{
		private static void Postfix()
		{
			switch (Settings.options.startingBottles)
			{
				case StartingBottles.Nothing:
					break;
				case StartingBottles.One500mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Water500ml"));
					break;
				case StartingBottles.One750mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_MetalWaterBottle"));
					break;
				case StartingBottles.One1000mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Water1000ml"));
					break;
				case StartingBottles.Two500mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Water500ml"));
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Water500ml"));
					break;
				case StartingBottles.Two750mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_MetalWaterBottle"));
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_MetalWaterBottle"));
					break;
				case StartingBottles.Two1000mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Water1000ml"));
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Water1000ml"));
					break;
				case StartingBottles.One3000mL:
					GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(new AssetReferenceGearItem("GEAR_Waterskin"));
					break;
			}
		}
	}
}
