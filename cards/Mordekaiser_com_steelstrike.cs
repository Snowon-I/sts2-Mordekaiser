using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public class Mordekaiser_com_steelstrike() : CardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move)];
    
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
            cards => cards.Where(c => c.Tags.Contains(CardTag.Strike)),
            true,
            "MORDEKAISER_NO_DRAW_TAGSTRIKE"
        );
    }

    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}