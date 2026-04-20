
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using FileAccess = Godot.FileAccess;

namespace Mordekaiser.scripts;

[HarmonyPatch(typeof(NGame), nameof(NGame._Ready))] 
public static class Mordekaiser_bank
{
	public static void Postfix(NGame __instance)
	{
		try
		{
			ModBankLoaderInstaller.TryInstall(__instance);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"[XW_Mordekaiser] Failed to install mod bank loader: {ex}");
		}
	}
}

public static class ModBankLoaderInstaller
{
	private static bool _installed;

	public static void TryInstall(NGame? game)
	{
		if (_installed)
			return;

		if (game == null || !GodotObject.IsInstanceValid(game))
			return;
		
		var baseLoader = game.GetNodeOrNull<Node>("FmodBankLoader");
		if (baseLoader == null)
			return;

		if (game.GetNodeOrNull<Node>("Mordekaiser_FmodBankLoader") != null)
		{
			_installed = true; 
			return;
		}

		var modBanks = new Godot.Collections.Array
		{
			"res://Mordekaiserbank/Mordekaiser.strings.bank", 
			"res://Mordekaiserbank/Mordekaiser.bank",
		};

		if (modBanks.Select(v => v.AsString()).Any(path => !FileAccess.FileExists(path)))
			return;

		var obj = ClassDB.Instantiate("FmodBankLoader");
		if (obj.VariantType  == Variant.Type.Nil)
			return;

		if (obj.AsGodotObject() is not Node MordekaiserLoader)
			return;

		MordekaiserLoader.Name = "Mordekaiser_FmodBankLoader";
		MordekaiserLoader.Set("bank_paths", modBanks);

		game.AddChild(MordekaiserLoader);
		_installed = true;

		Log.Info("莫得凯撒MOD：Bank已加载.");
	}
}
