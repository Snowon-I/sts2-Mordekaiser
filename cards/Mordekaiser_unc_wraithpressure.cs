using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.power;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_wraithpressure() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
		new CalculatedVar("Power").WithMultiplier((_,c) => c != null ? c.Player.Deck.Cards.Count(card => card.Pile.Type == PileType.Exhaust) : 0)
	];
	
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(CombatState);
		await PowerCmd.Apply<Mordekaiser_wraithpressurepower>(CombatState.HittableEnemies,((CalculatedVar)DynamicVars["Power"]).Calculate(Owner.Creature),Owner.Creature,this);
	}
	
	public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";
	
	protected override void OnUpgrade()
	{
		DynamicVars.CalculationExtra.UpgradeValueBy(1);
	}
	
	
}
