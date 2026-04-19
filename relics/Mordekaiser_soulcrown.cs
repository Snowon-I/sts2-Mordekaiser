using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using Mordekaiser.cards;
using Mordekaiser.power;

namespace Mordekaiser.relics;

public sealed class Mordekaiser_soulcrown : RelicModel
{
	private int _monstersouls;
	
	private int _mordekaiserleavel = 1;
	
	private const int _mordekaiserneedsouls = 6;

	public override string PackedIconPath => $"res://images/Mordekaiser_relic/{Id.Entry.ToLowerInvariant()}.png";

	protected override string PackedIconOutlinePath => $"res://images/Mordekaiser_relic/{Id.Entry.ToLowerInvariant()}_outline.png";

	protected override string BigIconPath => $"res://images/Mordekaiser_relic/{Id.Entry.ToLowerInvariant()}_big.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => [new("mordekaisernowleavel",Mordekaiserleavel)];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => 
	[
		HoverTipFactory.FromCard<Mordekaiser_ability_obliterate>(Mordekaiserleavel >= 5),
		HoverTipFactory.FromCard<Mordekaiser_ability_indestructible_block>(Mordekaiserleavel >= 6),
		HoverTipFactory.FromCard<Mordekaiser_ability_deathsgrasp>(Mordekaiserleavel >= 7),
		HoverTipFactory.FromCard<Mordekaiser_ability_realmofdeath>(Mordekaiserleavel >= 8)
	];

	public override RelicRarity Rarity => RelicRarity.Starter;

	public override bool ShowCounter => true;

	public override int DisplayAmount => MonsterSouls;

	[SavedProperty]
	public int MonsterSouls
	{
		get => _monstersouls;
		
		set
		{
			AssertMutable();
			_monstersouls = value;
			InvokeDisplayAmountChanged();
		}
	}
	
	[SavedProperty]
	public int Mordekaiserleavel
	{
		get => _mordekaiserleavel;
		
		set
		{
			AssertMutable();
			_mordekaiserleavel = value;
			DynamicVars["mordekaisernowleavel"].BaseValue = _mordekaiserleavel;
		}
	}
	
	public override Task AfterCombatVictory(CombatRoom room)
	{
		MonsterSouls += room.RoomType switch
		{
			RoomType.Monster => 1,
			RoomType.Elite => 2,
			RoomType.Boss => 4,
			_ => 0
		};
		Flash();
		while (MonsterSouls >= _mordekaiserneedsouls)
		{
			CreatureCmd.GainMaxHp(Owner.Creature, 3m);
			MonsterSouls -= 6;
			Mordekaiserleavel ++;
			Flash();
		}
		return Task.CompletedTask;
	}
	
	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
	{
		if (player == Owner && Owner.Creature.CombatState?.RoundNumber <= 1)
		{
			if (Mordekaiserleavel >= 1)
			{
				CardModel Mordekaiserobliterate = Owner.Creature.CombatState.CreateCard<Mordekaiser_ability_obliterate>(Owner);
				if (Mordekaiserleavel >= 5)
				{
					CardCmd.Upgrade(Mordekaiserobliterate);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiserobliterate, PileType.Hand, addedByPlayer: true);
			}
			if (Mordekaiserleavel >= 2)
			{
				CardModel Mordekaiser_indestructible = Owner.Creature.CombatState.CreateCard<Mordekaiser_ability_indestructible_block>(Owner);
				if (Mordekaiserleavel >= 6)
				{
					CardCmd.Upgrade(Mordekaiser_indestructible);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_indestructible, PileType.Hand, addedByPlayer: true);
			}
			if (Mordekaiserleavel >= 3)
			{
				CardModel Mordekaiserdeathsgrasp = Owner.Creature.CombatState!.CreateCard<Mordekaiser_ability_deathsgrasp>(Owner);
				if (Mordekaiserleavel >= 7)
				{
					CardCmd.Upgrade(Mordekaiserdeathsgrasp);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiserdeathsgrasp, PileType.Hand, addedByPlayer: true);
			}
			if (Mordekaiserleavel >= 4)
			{
				CardModel Mordekaiser_realmofdeath = Owner.Creature.CombatState!.CreateCard<Mordekaiser_ability_realmofdeath>(Owner);
				if (Mordekaiserleavel >= 8)
				{
					CardCmd.Upgrade(Mordekaiser_realmofdeath);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_realmofdeath, PileType.Hand, addedByPlayer: true);
			}
		}
	}

	public override async Task BeforeCombatStart()
	{
		if ((decimal)Mordekaiserleavel >= 2)
			await PowerCmd.Apply<Mordekaiser_potentialblockpower>(Owner.Creature,5m, Owner.Creature,null);
	}
	
}

public sealed class Mordekaiser_soulcrown_orobas : RelicModel
{
	private int _monstersouls;
	
	private int _mordekaiserleavel = 5;

	private const int _mordekaiserneedsouls = 6;
	
	protected override IEnumerable<DynamicVar> CanonicalVars => [new("mordekaisernowleavel",Mordekaiserleavel)];

	public override string PackedIconPath => $"res://images/Mordekaiser_relic/{ModelDb.Relic<Mordekaiser_soulcrown>().Id.Entry.ToLowerInvariant()}.png";

	protected override string PackedIconOutlinePath => $"res://images/Mordekaiser_relic/{ModelDb.Relic<Mordekaiser_soulcrown>().Id.Entry.ToLowerInvariant()}_outline.png";

	protected override string BigIconPath => $"res://images/Mordekaiser_relic/{ModelDb.Relic<Mordekaiser_soulcrown>().Id.Entry.ToLowerInvariant()}_big.png";
	
	protected override IEnumerable<IHoverTip> ExtraHoverTips => 
	[
		HoverTipFactory.FromCard<Mordekaiser_ability_obliterate>(Mordekaiserleavel >= 5),
		HoverTipFactory.FromCard<Mordekaiser_ability_indestructible_block>(Mordekaiserleavel >= 6),
		HoverTipFactory.FromCard<Mordekaiser_ability_deathsgrasp>(Mordekaiserleavel >= 7),
		HoverTipFactory.FromCard<Mordekaiser_ability_realmofdeath>(Mordekaiserleavel >= 8)
	];

	public override RelicRarity Rarity => RelicRarity.Ancient;

	public override bool ShowCounter => true;

	public override int DisplayAmount => MonsterSouls;

	[SavedProperty]
	public int MonsterSouls
	{
		get => _monstersouls;
		
		set
		{
			AssertMutable();
			_monstersouls = value;
			InvokeDisplayAmountChanged();
		}
	}
	
	[SavedProperty]
	public int Mordekaiserleavel
	{
		get => _mordekaiserleavel;
		
		set
		{
			AssertMutable();
			_mordekaiserleavel = value;
			DynamicVars["mordekaisernowleavel"].BaseValue = _mordekaiserleavel;
		}
	}
	
	public override Task AfterCombatVictory(CombatRoom room)
	{
		MonsterSouls += room.RoomType switch
		{
			RoomType.Monster => 1,
			RoomType.Elite => 2,
			RoomType.Boss => 4,
			_ => 0
		};
		Flash();
		while (MonsterSouls >= _mordekaiserneedsouls)
		{
			CreatureCmd.GainMaxHp(Owner.Creature, 5m);
			MonsterSouls -= _mordekaiserneedsouls;
			Mordekaiserleavel ++;
			Flash();
		}
		return Task.CompletedTask;
	}
	
	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
	{
		if (player == Owner && Owner.Creature.CombatState?.RoundNumber <= 1)
		{
			if (Mordekaiserleavel >= 1)
			{
				CardModel Mordekaiserobliterate = Owner.Creature.CombatState.CreateCard<Mordekaiser_ability_obliterate>(Owner);
				if (Mordekaiserleavel >= 5)
				{
					CardCmd.Upgrade(Mordekaiserobliterate);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiserobliterate, PileType.Hand, addedByPlayer: true);
			}
			if (Mordekaiserleavel >= 2)
			{
				CardModel Mordekaiser_indestructible = Owner.Creature.CombatState.CreateCard<Mordekaiser_ability_indestructible_block>(Owner);
				if (Mordekaiserleavel >= 6)
				{
					CardCmd.Upgrade(Mordekaiser_indestructible);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_indestructible, PileType.Hand, addedByPlayer: true);
			}
			if (Mordekaiserleavel >= 3)
			{
				CardModel Mordekaiserdeathsgrasp = Owner.Creature.CombatState!.CreateCard<Mordekaiser_ability_deathsgrasp>(Owner);
				if (Mordekaiserleavel >= 7)
				{
					CardCmd.Upgrade(Mordekaiserdeathsgrasp);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiserdeathsgrasp, PileType.Hand, addedByPlayer: true);
			}
			if (Mordekaiserleavel >= 4)
			{
				CardModel Mordekaiser_realmofdeath = Owner.Creature.CombatState!.CreateCard<Mordekaiser_ability_realmofdeath>(Owner);
				if (Mordekaiserleavel >= 8)
				{
					CardCmd.Upgrade(Mordekaiser_realmofdeath);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_realmofdeath, PileType.Hand, addedByPlayer: true);
			}
		}
	}

	public override async Task BeforeCombatStart()
	{
		if ((decimal)Mordekaiserleavel >= 2)
			await PowerCmd.Apply<Mordekaiser_potentialblockpower>(Owner.Creature,5m, Owner.Creature,null);
	}
	
}

[HarmonyPatch(typeof(TouchOfOrobas), nameof(TouchOfOrobas.GetUpgradedStarterRelic))]
public static class MordekaiserOrobasPatch
{
	static bool Prefix(RelicModel starterRelic, ref RelicModel __result)
	{
		if (starterRelic.Id == ModelDb.Relic<Mordekaiser_soulcrown>().Id)
		{
			__result = ModelDb.Relic<Mordekaiser_soulcrown_orobas>();
			return false;
		}
		return true;
	}
	
}
