using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using Mordekaiser.Core.CardLibrary;
using Mordekaiser.Mordekaiserpools;

namespace Mordekaiser.scripts;

[HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary._Ready))] 
public class Mordekaiser_card_library
{
    
    static void Postfix(
        NCardLibrary __instance,
        ref Dictionary<NCardPoolFilter, Func<CardModel, bool>> ____poolFilters,
        ref Dictionary<CharacterModel, NCardPoolFilter> ____cardPoolFilters
        )
    {
        
        var poolFilters = __instance.GetNode("Sidebar/MarginContainer/TopVBox/PoolFilters");
        
        if (poolFilters == null || poolFilters.HasNode("MordekaiserPool")) return;
            
        var scene = GD.Load<PackedScene>("res://scenes/cardlibrary/MordekaiserPool.tscn");
        var mordekaiserPool = scene.Instantiate<MordekaiserPoolFilter>();
        mordekaiserPool.Name = "MordekaiserPool";
        mordekaiserPool.FocusNeighborTop = __instance.GetNode("Sidebar/MarginContainer/TopVBox/SearchBar/TextArea").GetPath();
        
        var insertIndex = poolFilters.GetNodeOrNull("DefectPool")?.GetIndex() + 1 ?? 1;
        poolFilters.AddChild(mordekaiserPool);
        poolFilters.MoveChild(mordekaiserPool, insertIndex);
        
        mordekaiserPool.Toggled += _ => { __instance.Call("UpdateCardPoolFilter", mordekaiserPool); };
        
        ____poolFilters.Add(mordekaiserPool, c => c.Pool is Mordekaisercardpool);
        ____cardPoolFilters.Add(ModelDb.Character<Characters.Mordekaiser>(), mordekaiserPool);
        
        mordekaiserPool.FocusEntered += () => { __instance.Set("_lastHoveredControl", mordekaiserPool); };

    }

}