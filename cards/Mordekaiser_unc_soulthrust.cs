using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_soulthrust() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override bool ShouldGlowGoldInternal => WasCardQuiesceLastCard;

    private bool WasCardQuiesceLastCard {
        get
        {
            var _lastcard = CombatManager.Instance.History.CardPlaysStarted.LastOrDefault(c => c.CardPlay.Card.Owner == Owner);
            return _lastcard != null && _lastcard.CardPlay.Card.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce) && _lastcard.CardPlay.Card != this;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move),
        new CardsVar(1)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (WasCardQuiesceLastCard)
        {
            var _lastcard = CombatManager.Instance.History.CardPlaysStarted.LastOrDefault(c => c.CardPlay.Card.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce) && c.CardPlay.Card.Owner == Owner );
            if (_lastcard != null)
            {
                int num = 0;
                while ( num < (int)DynamicVars.Cards.BaseValue )
                {
                    num++;
                    await _lastcard.CardPlay.Card.AfterCardExhausted(choiceContext, _lastcard.CardPlay.Card, false);
                }
            }
        }
    }

    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    
}