using Il2CppInterop.Runtime.Attributes;
using UnityEngine;

namespace BetterWaterManagement;

public abstract class ModSaveBehaviour : MonoBehaviour
{
	[HideFromIl2Cpp]
	public abstract void Deserialize(string data);

	[HideFromIl2Cpp]
	public abstract string Serialize();

	public ModSaveBehaviour(IntPtr intPtr) : base(intPtr) { }
}