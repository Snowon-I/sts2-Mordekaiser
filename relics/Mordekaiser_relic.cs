using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using Mordekaiser.cards;
using Mordekaiser.power;

namespace Mordekaiser.relics;

public sealed class Mordekaiser_relic : RelicModel
{
	private int _monstersouls;
	
	private int _mordekaiserleavel = 1;
	
	public override string PackedIconPath => $"res://images/relics/akabeko.png";

	protected override string PackedIconOutlinePath => $"res://images/relics/akabeko.png";

	protected override string BigIconPath => $"res://images/relics/akabeko.png";
	
	protected override IEnumerable<IHoverTip> ExtraHoverTips => 
	[
		HoverTipFactory.FromCard<Mordekaiser_ability_deathsgrasp>(),
		HoverTipFactory.FromCard<Mordekaiser_ability_obliterate>(),
		HoverTipFactory.FromCard<Mordekaiser_ability_indestructible_block>(),
	];

	public override RelicRarity Rarity => RelicRarity.Starter;

	public override bool ShowCounter => true;

	public override int DisplayAmount => MonsterSouls;
	
	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[ 
		new("mordekaisergetsoul", 6), 
		new("mordekaisernowleavel", Mordekaiserleavel)
	];

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
		while (MonsterSouls >= DynamicVars["mordekaisergetsoul"].BaseValue)
		{
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
			if ((decimal)Mordekaiserleavel >= 1)
			{
				CardModel Mordekaiserobliterate = Owner.Creature.CombatState.CreateCard<Mordekaiser_ability_obliterate>(Owner);
				if (Mordekaiserleavel >= 5)
				{
					CardCmd.Upgrade(Mordekaiserobliterate);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiserobliterate, PileType.Hand, addedByPlayer: true);
			}
			if ((decimal)Mordekaiserleavel >= 2)
			{
				CardModel Mordekaiser_indestructible = Owner.Creature.CombatState.CreateCard<Mordekaiser_ability_indestructible_block>(Owner);
				if (Mordekaiserleavel >= 6)
				{
					CardCmd.Upgrade(Mordekaiser_indestructible);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_indestructible, PileType.Hand, addedByPlayer: true);
			}
			if ((decimal)Mordekaiserleavel >= 3)
			{
				CardModel Mordekaiserdeathsgrasp = Owner.Creature.CombatState!.CreateCard<Mordekaiser_ability_deathsgrasp>(Owner);
				if (Mordekaiserleavel >= 7)
				{
					CardCmd.Upgrade(Mordekaiserdeathsgrasp);
				}
				await CardPileCmd.AddGeneratedCardToCombat(Mordekaiserdeathsgrasp, PileType.Hand, addedByPlayer: true);
			}
		}
	}

	public override async Task BeforeCombatStart()
	{
		if ((decimal)Mordekaiserleavel >= 2)
			await PowerCmd.Apply<Mordekaiser_potentialblock>(Owner.Creature,5m, Owner.Creature,null);
	}
	
}
