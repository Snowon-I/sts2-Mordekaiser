using System.Reflection;
using System.Runtime.InteropServices;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Unlocks;
using Mordekaiser.cards;
using Mordekaiser.Core.Timeline.Epochs;
using Mordekaiser.Mordekaiserpools;
using Mordekaiser.relics;

namespace Mordekaiser.Characters;

public sealed class Mordekaiser : CharacterModel
{
	public override CharacterGender Gender => CharacterGender.Masculine;

	protected override CharacterModel? UnlocksAfterRunAs => null;

	public override Color NameColor => StsColors.darkBlue;

	public override int StartingHp => 60;

	public override int StartingGold => 1;

	public override CardPoolModel CardPool => ModelDb.CardPool<Mordekaisercardpool>();

	public override PotionPoolModel PotionPool => ModelDb.PotionPool<Mordekaiserpotionpool>();

	public override RelicPoolModel RelicPool => ModelDb.RelicPool<Mordekaiserrelicpool>();

	//宝箱手指动画没做，指向，石头剪刀布，即Texture2D ArmPointingTexturePath
	
	public override IEnumerable<CardModel> StartingDeck => [
		ModelDb.Card<Mordekaiser_base_strike>(),
		ModelDb.Card<Mordekaiser_base_strike>(),
		ModelDb.Card<Mordekaiser_base_strike>(),
		ModelDb.Card<Mordekaiser_base_strike>(),
		ModelDb.Card<Mordekaiser_base_strike>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_criticalattack>(),
		ModelDb.Card<Mordekaiser_base_darknessrise>(),
	];

	public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<Mordekaiser_soulcrown>()];

	public override float AttackAnimDelay => 0.15f;

	public override float CastAnimDelay => 0.25f;

	public override Color EnergyLabelOutlineColor => new ("045284FF");
		
	public override Color DialogueColor => new ("005259");

	public override Color MapDrawingColor => new ("28bacb");

	public override Color RemoteTargetingLineColor => new ("478fe1FF");

	public override Color RemoteTargetingLineOutline => new ("127c80FF");

	public override string CharacterSelectSfx => "event:/Mordekaiser/Mordekaiser_select";

	public override List<string> GetArchitectAttackVfx()
	{
		var num = 5;
		var list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		var span = CollectionsMarshal.AsSpan(list);
		var num2 = 0;
		span[num2] = "vfx/vfx_attack_blunt";
		num2++;
		span[num2] = "vfx/vfx_heavy_blunt";
		num2++;
		span[num2] = "vfx/vfx_attack_slash";
		num2++;
		span[num2] = "vfx/vfx_bloody_impact";
		num2++;
		span[num2] = "vfx/vfx_rock_shatter";
		return list;
	}
	
	public override CreatureAnimator GenerateAnimator(MegaSprite controller)
	{
		var animState = new AnimState("idle_loop", true);
		var state1 = new AnimState("cast");
		var state2 = new AnimState("attack");
		var state3 = new AnimState("hurt");
		var state4 = new AnimState("die");
		var state5 = new AnimState("relaxed_loop", true);
		var state6 = new AnimState("Obliterate", true);
		state1.NextState = animState;
		state2.NextState = animState;
		state3.NextState = animState;
		state6.NextState = animState;
		state5.AddBranch("Idle", animState);
		var animator = new CreatureAnimator(animState, controller);
		animator.AddAnyState("Idle", animState);
		animator.AddAnyState("Dead", state4);
		animator.AddAnyState("Hit", state3);
		animator.AddAnyState("Attack", state2);
		animator.AddAnyState("Cast", state1);
		animator.AddAnyState("Relaxed", state5);
		animator.AddAnyState("Obliterate", state6);
		return animator;
	}
	
}

[HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllCharacters), MethodType.Getter)] //nameof可以改为"AllCharacters",如果后续被保护的话
public static class MordekaiserPatchAdd
{
	public static void Postfix(ref IEnumerable<CharacterModel> __result)
	{
		var MordekaiserCharater = __result.Concat([ ModelDb.Character<Mordekaiser>() ]).ToArray();

		__result = MordekaiserCharater;
			
	}
		
}

[HarmonyPatch(typeof(UnlockState), nameof(UnlockState.Characters), MethodType.Getter)] //nameof可以改为"AllCharacters",如果后续被保护的话
public static class MordekaiserLockPatchAdd
{
	public static void Postfix(UnlockState __instance,ref IEnumerable<CharacterModel> __result)
	{
		var MordekaiserCharater = __result.ToList();
		if (!__instance.IsEpochRevealed<Mordekaiser1Epoch>())
			MordekaiserCharater.Remove(ModelDb.Character<Mordekaiser>());
		__result = MordekaiserCharater;
	}
		
}

[HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen.SelectCharacter))]
[HarmonyPatch([typeof(NCharacterSelectButton), typeof(CharacterModel)])]
public static class SelectMordekaiserCharacter
{
	public static bool Ismodekaisermusic { get; set; }

	public static void Postfix(NCharacterSelectButton charSelectButton, CharacterModel characterModel)
	{
		if (string.IsNullOrEmpty(characterModel.Id.Entry))return;

		var isMordekaiserByType = characterModel is Mordekaiser;
		
		if (isMordekaiserByType && !Ismodekaisermusic)
		{
			Ismodekaisermusic = true;
			NAudioManager.Instance?.StopMusic();
		}
		if (!isMordekaiserByType && Ismodekaisermusic)
		{
			Ismodekaisermusic = false;
			NAudioManager.Instance?.PlayMusic("event:/music/menu_update");
		}
	}
	
}

[HarmonyPatch(typeof(NBackButton), "OnEnable")]
public static class BackMordekaiserCharacter
{
	public static void Postfix()
	{
		if (SelectMordekaiserCharacter.Ismodekaisermusic)
		{
			NAudioManager.Instance?.PlayMusic("event:/music/menu_update");
			SelectMordekaiserCharacter.Ismodekaisermusic = false;
		}
	}
	
}

[HarmonyPatch(typeof(NConfirmButton), "OnDisable")]
public static class ConfirmMordekaiserCharacter
{
	public static void Postfix()
	{
		if (SelectMordekaiserCharacter.Ismodekaisermusic)
		{
			SelectMordekaiserCharacter.Ismodekaisermusic = false;
		}
	}
	
}

[HarmonyPatch(typeof(ProgressSaveManager))]
public static class FixMordekaiserCharacterEpochCrash
{
	// 目标：两个会抛异常的方法
	[HarmonyPatch("CheckFifteenElitesDefeatedEpoch")]
	[HarmonyPrefix]
	public static bool Prefix_elites(Player localPlayer)
	{
		return localPlayer.Character is not Mordekaiser;
	}
	
	[HarmonyPatch("CheckFifteenBossesDefeatedEpoch")]
	[HarmonyPrefix]
	public static bool Prefix_boss(Player localPlayer)
	{
		return localPlayer.Character is not Mordekaiser;
	}
	
	[HarmonyPatch("ObtainCharUnlockEpoch")] //打败boss，可做
	[HarmonyPrefix]
	public static bool Prefix_Obtain(Player localPlayer)
	{
		return localPlayer.Character is not Mordekaiser;
	}
	
}

[HarmonyPatch(typeof(TheArchitect), nameof(TheArchitect.DialogueSet), MethodType.Getter)]
public static class MordekaiserArchitectDialogue
{
	
	private static readonly PropertyInfo? _visitIndexProp = typeof(AncientDialogue).GetProperty("VisitIndex");
	
	static void Postfix(TheArchitect __instance , ref AncientDialogueSet __result)
	{
        var mordekaiserId = ModelDb.Character<Mordekaiser>().Id.Entry;

        if (__result.CharacterDialogues.ContainsKey(mordekaiserId))
        {
            return;
        }

        var dialogues = new List<AncientDialogue>
        {
            new ("", "", "") { EndAttackers = ArchitectAttackers.Both },
            new ("", "", "") { EndAttackers = ArchitectAttackers.Both },
            new ("", "", "") { EndAttackers = ArchitectAttackers.Both }
        };
        __result.CharacterDialogues[mordekaiserId] = dialogues.AsReadOnly();

        for (var i = 0; i < dialogues.Count; i++)
        {
            dialogues[i].PopulateLines(__instance.Id.Entry, mordekaiserId, i);
            _visitIndexProp?.SetValue(dialogues[i], (int?)i);
        }
	}

}

[HarmonyPatch(typeof(AtlasResourceLoader), nameof(AtlasResourceLoader.ParsePath))]
public static class AtlasResourceLoader_Mordekaiser_Patch
{
	public static void Postfix(string path, ref (string? AtlasName, string? SpriteName) __result)
	{
		if (__result.SpriteName == null) return;
		
		if (__result.SpriteName.StartsWith("mordekaiser_") && __result.SpriteName.EndsWith("power"))
		{
			 __result = ("mordekaiser_power_atlas", __result.SpriteName);
			 return;
		}
		
		if (__result.SpriteName.StartsWith("card/energy") && __result.SpriteName.EndsWith("mordekaiser"))
		{
			__result = ("energy_mordekaiser", __result.SpriteName);
			return;
		}
		
		if (__result.SpriteName.StartsWith("mordekaiser/mordekaiser"))
		{
			__result = ("mordekaiser_cards", __result.SpriteName);
			return;
		}
		
		if (__result.SpriteName.StartsWith("mordekaiser") && __result.SpriteName.EndsWith("_epoch"))
		{
			__result = ("mordekaiser_epoch", __result.SpriteName);
		}

	}
	
}

[HarmonyPatch]
public static class Mordekaiser_Epochs_Patch
{
	
	[HarmonyPostfix]
	[HarmonyPatch(typeof(NTimelineScreen), nameof(NTimelineScreen.OnSubmenuOpened))]
	static async void Post1(NTimelineScreen __instance)
	{
		try
		{
			var MordekaiserEpoch = EpochModel.Get<Mordekaiser1Epoch>();
			var targetEpochSlotAll = __instance.GetChildrenRecursive<NEpochSlot>();
			foreach (var slot in targetEpochSlotAll)
			{
				var state = SaveManager.Instance.Progress.Epochs.FirstOrDefault(e => e.Id == slot.model.Id)?.State;
				if (state is EpochState.Revealed)
					slot.SetState(EpochSlotState.Complete);
				if (state is EpochState.Obtained)
					slot.SetState(EpochSlotState.Obtained);
				if (state is EpochState.NotObtained)
					slot.SetState(EpochSlotState.NotObtained);
			}
			
			var progress = SaveManager.Instance.Progress;
			var serializableEpoch = progress.Epochs.FirstOrDefault(e => e.Id == MordekaiserEpoch.Id);

			if (serializableEpoch == null || serializableEpoch.State == EpochState.Revealed) return;
			if (!progress.IsEpochObtained(MordekaiserEpoch.Id))
				return;
			
			var slot1 = new EpochSlotData(MordekaiserEpoch.Id, EpochSlotState.Obtained);
			await __instance.AddEpochSlots([slot1],true);
			
			SaveManager.Instance.UnlockSlot(MordekaiserEpoch.Id);
		}
		catch (Exception e)
		{
			GD.PrintErr(e);
		}
	}
	
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProgressSaveManager), nameof(ProgressSaveManager.UpdateWithRunData))]
	static void Post2(ProgressSaveManager __instance, SerializableRun serializableRun, bool victory)
	{
		SerializablePlayer? serializablePlayer;
		var progress = __instance.Progress;
		var MordekaiserEpoch = EpochModel.Get<Mordekaiser1Epoch>();
		if (progress.IsEpochObtained(MordekaiserEpoch.Id)) return;
		if (serializableRun.Players.Count == 1)
		{
			serializablePlayer = serializableRun.Players.First();
		}
		else
		{
			var localPlayerId = PlatformUtil.GetLocalPlayerId(serializableRun.PlatformType); 
			serializablePlayer = serializableRun.Players.FirstOrDefault(p => p.NetId == localPlayerId);
		}
		SaveManager.Instance.Progress.ObtainEpoch(MordekaiserEpoch.Id);
		serializablePlayer!.DiscoveredEpochs.Add(MordekaiserEpoch.Id);
		__instance.SaveProgress();
	}
	
}
