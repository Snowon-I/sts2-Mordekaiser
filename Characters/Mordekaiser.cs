using System.Reflection;
using System.Runtime.InteropServices;
using Godot;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Saves.Managers;
using Mordekaiser.cards;
using Mordekaiser.Mordekaiserpools;
using Mordekaiser.relics;

namespace Mordekaiser.Characters;

public sealed class Mordekaiser : CharacterModel
{
	public override CharacterGender Gender => CharacterGender.Masculine;

	protected override CharacterModel? UnlocksAfterRunAs => null;

	public override Color NameColor => StsColors.darkBlue;

	public override int StartingHp => 60;

	public override int StartingGold => 0;

	public override CardPoolModel CardPool => ModelDb.CardPool<Mordekaisercardpool>();

	public override PotionPoolModel PotionPool => ModelDb.PotionPool<Mordekaiserpotionpool>();

	public override RelicPoolModel RelicPool => ModelDb.RelicPool<Mordekaiserrelicpool>();
	
	//商人动画没做，即string MerchantAnimPath
	//休息动画没做，即string RestSiteAnimPath
	//商人动画没做，即string MerchantAnimPath
	//宝箱手指动画没做，指向，石头剪刀布，即Texture2D ArmPointingTexturePath
	
	public override IEnumerable<CardModel> StartingDeck => [
		ModelDb.Card<Mordekaiser_base_attack>(),
		ModelDb.Card<Mordekaiser_base_attack>(),
		ModelDb.Card<Mordekaiser_base_attack>(),
		ModelDb.Card<Mordekaiser_base_attack>(),
		ModelDb.Card<Mordekaiser_base_attack>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_defend>(),
		ModelDb.Card<Mordekaiser_base_criticalattack>(),
		ModelDb.Card<Mordekaiser_base_darknessrise>(),
	];

	public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<Mordekaiser_relic>()];

	public override float AttackAnimDelay => 0.15f;

	public override float CastAnimDelay => 0.25f;

	public override Color EnergyLabelOutlineColor => new Color("801212FF");

	public override Color DialogueColor => new Color("590700");

	public override Color MapDrawingColor => new Color("CB282B");

	public override Color RemoteTargetingLineColor => new Color("E15847FF");

	public override Color RemoteTargetingLineOutline => new Color("801212FF");

	public override string CharacterSelectSfx => $"event:/sfx/characters/ironclad/ironclad_select";

	public override List<string> GetArchitectAttackVfx()
	{
		int num = 5;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<string> span = CollectionsMarshal.AsSpan(list);
		int num2 = 0;
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
		AnimState animState = new AnimState("idle_loop", true);
		AnimState state1 = new AnimState("cast");
		AnimState state2 = new AnimState("attack");
		AnimState state3 = new AnimState("hurt");
		AnimState state4 = new AnimState("die");
		AnimState state5 = new AnimState("relaxed_loop", true);
		AnimState state6 = new AnimState("Obliterate", true);
		state1.NextState = animState;
		state2.NextState = animState;
		state3.NextState = animState;
		state6.NextState = animState;
		state5.AddBranch("Idle", animState);
		CreatureAnimator animator = new CreatureAnimator(animState, controller);
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


[UsedImplicitly] //消除未被使用警告
[HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllCharacters), MethodType.Getter)] //nameof可以改为"AllCharacters",如果后续被保护的话
public static class MordekaiserPatchAdd
{
	public static void Postfix(ref IEnumerable<CharacterModel> __result)
	{
		var MordekaiserCharater = __result.Concat([ ModelDb.Character<Mordekaiser>() ]).ToArray();

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

		bool isMordekaiserByType = characterModel is Mordekaiser;
		
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

[HarmonyPatch(typeof(NCard), "Reload")]
public static class MordekaiserEnergyAdd
{
	private const string CustomEnergyIconPath = "res://images/atlases/mordekaiser_energy.png";
	
	public static void Postfix(NCard __instance)
	{
		if (!__instance.IsNodeReady() || __instance.Model?.Pool == null ) return;
		
		CardPoolModel targetPool = __instance.Model.Pool;
		
		if (targetPool is not Mordekaisercardpool ) return;

		// 1. 反射获取NCard的私有字段 _energyIcon
		var energyIconField = typeof(NCard).GetField(
			"_energyIcon", 
			BindingFlags.NonPublic | BindingFlags.Instance
		);
		
		var originalEnergyIcon = energyIconField?.GetValue(__instance) as TextureRect;
		
		if (originalEnergyIcon == null ) return;

		originalEnergyIcon.Texture = ResourceLoader.Load<Texture2D>(CustomEnergyIconPath);

	}

}

[HarmonyPatch(typeof(ProgressSaveManager))]
public static class FixMyCustomCharacterEpochCrash
{
	// 目标：两个会抛异常的方法
	[HarmonyPatch("CheckFifteenElitesDefeatedEpoch")]
	[HarmonyPatch("CheckFifteenBossesDefeatedEpoch")]
	[HarmonyPrefix]
	public static bool Prefix(Player localPlayer)
	{
		return localPlayer.Character is not Mordekaiser;
	}
}
