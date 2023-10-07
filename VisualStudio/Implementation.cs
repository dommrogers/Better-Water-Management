using MelonLoader;
using Il2CppInterop.Runtime.Injection;
using Il2CppTLD.Gear;
using UnityEngine.AddressableAssets;

namespace BetterWaterManagement;

internal sealed class Implementation : MelonMod
{

	public static LiquidType ModWater;
	public static LiquidType PotableWater;

	public override void OnInitializeMelon()
	{
		base.OnInitializeMelon();
		Settings.OnLoad();
		SpawnProbabilities.AddToModComponent();
		ClassInjector.RegisterTypeInIl2Cpp<BetterWaterManagement.ModSaveBehaviour>();
		ClassInjector.RegisterTypeInIl2Cpp<BetterWaterManagement.OverrideCookingState>();
		ClassInjector.RegisterTypeInIl2Cpp<BetterWaterManagement.CoolDown>();
		ClassInjector.RegisterTypeInIl2Cpp<BetterWaterManagement.CookingModifier>();
		ClassInjector.RegisterTypeInIl2Cpp<BetterWaterManagement.CookingPotWaterSaveData>();
		ModWater = Addressables.LoadAssetAsync<LiquidType>("LIQUID_ModWater").WaitForCompletion();
		PotableWater = Addressables.LoadAssetAsync<LiquidType>("LIQUID_WaterPotable").WaitForCompletion();
	}

	internal static void LogError(string message, params object[] parameters) => MelonLogger.Error(message, parameters);
}