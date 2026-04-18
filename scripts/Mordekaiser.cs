using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;
using Mordekaiser.Core.Timeline.Epochs;
using Mordekaiser.Core.Timeline.Stories;
using Mordekaiser.relics;

namespace Mordekaiser.scripts;

[ModInitializer("XW_Mordekaiser")]
public class Mordekaiser
{
	private static Harmony? _harmony;

	public static void XW_Mordekaiser()
	{
		ScriptManagerBridge.LookupScriptsInAssembly(assembly: typeof(Mordekaiser).Assembly);
		
		SavedPropertiesTypeCache.InjectTypeIntoCache(typeof(Mordekaiser_soulcrown));
		AtlasManager.LoadAtlas("mordekaiser_power_atlas");
		AtlasManager.LoadAtlas("energy_mordekaiser");
		AtlasManager.LoadAtlas("mordekaiser_cards");
		AtlasManager.LoadAtlas("mordekaiser_epoch");
		RegisterEpochManually<Mordekaiser1Epoch>();
		RegisterStoryManually();
		InjectMordekaiserEpochId("MORDEKAISER1_EPOCH");
		GD.Print("莫德凯撒mod当前版本0.1.1");
		
		_harmony = new Harmony("sts2.snowI.XW_Mordekaiser");
		_harmony.PatchAll();
	}
	
	private static void RegisterEpochManually<T>() where T : EpochModel, new()
	{
		var instance = new T();
		var id = instance.Id;
		
		var typeDict = (Dictionary<string, Type>)typeof(EpochModel)
			.GetField("_epochTypeDictionary", BindingFlags.Static | BindingFlags.NonPublic)!
			.GetValue(null)!;

		var idDict = (Dictionary<Type, string>)typeof(EpochModel)
			.GetField("_typeToIdDictionary", BindingFlags.Static | BindingFlags.NonPublic)!
			.GetValue(null)!;
		
		typeDict[id] = typeof(T);
		idDict[typeof(T)] = id;
	}
	
	private static void RegisterStoryManually()
	{
		var storyModelType = typeof(StoryModel);
		var dictField = storyModelType.GetField("_storyTypeDictionary", BindingFlags.NonPublic | BindingFlags.Static);;
		if (dictField == null)
		{
		}
		else
		{
			var dictionary = (Dictionary<string, Type>)dictField.GetValue(null)!;
			const string key = "MORDEKAISER"; 
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, typeof(MordekaiserStory));
			}
		}

	}
	
	public static void InjectMordekaiserEpochId(string MordekaiserEpochId)
	{
		var epochModelType = typeof(EpochModel);
		var allIdsField = epochModelType.GetField("_allEpochIds", BindingFlags.NonPublic | BindingFlags.Static);
        
		if (allIdsField == null) return;

		var initializedList = EpochModel.AllEpochIds;
		var newList = initializedList.ToList();
		if (!newList.Contains(MordekaiserEpochId))
		{
			newList.Add(MordekaiserEpochId);
		}
		allIdsField.SetValue(null, newList);
	}
	
}

public class MordekaiserKeyWord
{
	public static readonly CardKeyword MordekaiserQuiesce = (CardKeyword)1216;
}




