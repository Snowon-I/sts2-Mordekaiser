using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public class Mordekaiser_com_underworldstrike() : CardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(3m),
        new ExtraDamageVar(6m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((model, _) => model.Owner.Deck.Cards.Count(c => c.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce)))
    ];
    
    public override IEnumerable<CardTag> Tags => [CardTag.Strike];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
}