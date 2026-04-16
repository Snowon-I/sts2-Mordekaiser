using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_com_strike() : CardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];
    
    public override IEnumerable<CardTag> Tags => [CardTag.Strike];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        
        await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            Owner,
            1,
            PileType.Draw.GetPile(Owner), PileType.Hand.GetPile(Owner),
            cards => cards.Where(c => c.Id.Entry == "MORDEKAISER_BASE_STRIKE"),
            true,
            "MORDEKAISER_NO_DRAW_BASEATTACK",
            noCost: true
            );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }

}