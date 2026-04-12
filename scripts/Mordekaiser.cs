using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;
using Mordekaiser.relics;

namespace Mordekaiser.scripts;

[ModInitializer("XW_Mordekaiser")]
public class Mordekaiser
{
	private static Harmony? _harmony;

	public static void XW_Mordekaiser()
	{
		ScriptManagerBridge.LookupScriptsInAssembly(assembly: typeof(Mordekaiser).Assembly);
		SavedPropertiesTypeCache.InjectTypeIntoCache(typeof(Mordekaiser_relic));
		_harmony = new Harmony("sts2.reme.XW_Mordekaiser");
		_harmony.PatchAll();
	}
	
}

public class MordekaiserKeyWord
{
	public static readonly CardKeyword MordekaiserQuiesce = (CardKeyword)1216;
}




