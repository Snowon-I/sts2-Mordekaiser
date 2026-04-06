using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Modding;

namespace Mordekaiser.scripts;

[ModInitializer("XW_Mordekaiser")]
public class Mordekaiser
{
	private static Harmony? _harmony;

	public static void XW_Mordekaiser()
	{
		ScriptManagerBridge.LookupScriptsInAssembly(assembly: typeof(Mordekaiser).Assembly);
		_harmony = new Harmony("sts2.reme.XW_Mordekaiser");
		_harmony.PatchAll();
	}
	
}

public class MordekaiserKeyWord
{
	public static readonly CardKeyword MordekaiserQuiesce = (CardKeyword)1216;
}




