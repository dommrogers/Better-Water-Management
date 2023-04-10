using MelonLoader;
using Il2CppInterop.Runtime.Injection;
namespace BetterWaterManagement;

internal sealed class Implementation : MelonMod
{

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
	}

	internal static void LogError(string message, params object[] parameters) => MelonLogger.Error(message, parameters);
}