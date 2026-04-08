using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public class Mordekaiser_unc_wraithseek() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(11m, ValueProp.Move)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        var card1 = PileType.Draw.GetPile(Owner).Cards.FirstOrDefault(c => c.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce) && c.Type == CardType.Attack );
        if (card1 != null)
        {
            await CardPileCmd.Add(card1,PileType.Draw); 
            if (card1.Keywords.Contains(CardKeyword.Exhaust))
            {
                var card2 = PileType.Draw.GetPile(Owner).Cards.FirstOrDefault(c => c.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce) && c.Type == CardType.Attack );
                if (card2 != null)
                {
                    await CardPileCmd.Add(card2,PileType.Draw);
                    if (!card2.Keywords.Contains(CardKeyword.Exhaust))
                    {
                        card2.AddKeyword(CardKeyword.Exhaust);
                    }
                }
            }
        }
    }
    
    public override string PortraitPath => "res://images/card_portraits/Mordekaiser_unc_wraithseek.png";
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}