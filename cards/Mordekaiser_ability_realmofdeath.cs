using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.power;

namespace Mordekaiser.cards;

public class Mordekaiser_ability_realmofdeath() : CardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain,CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<Mordekaiser_deceasedsdomainpower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ("Power",2m),
        new ("GainHp",5m)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var hp = Math.Floor(cardPlay.Target.MaxHp * DynamicVars["GainHp"].BaseValue * 0.01m) ;
        var strPower = cardPlay.Target.GetPower<StrengthPower>();
        var dexPower = cardPlay.Target.GetPower<DexterityPower>();
        await CreatureCmd.SetCurrentHp(cardPlay.Target,cardPlay.Target.CurrentHp - hp);
        await CreatureCmd.Heal(Owner.Creature,hp);
        if (strPower != null)
        {
            await PowerCmd.Apply<StrengthPower>(Owner.Creature,strPower.Amount,Owner.Creature,cardPlay.Card);
            await PowerCmd.Remove(strPower);
        }
        if (dexPower != null)
        {
            await PowerCmd.Apply<DexterityPower>(Owner.Creature,dexPower.Amount,Owner.Creature,cardPlay.Card);
            await PowerCmd.Remove(dexPower);
        }
        await PowerCmd.Apply<Mordekaiser_deceasedsdomainpower>(cardPlay.Target, DynamicVars["Power"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<Mordekaiser_deceasedsdomainpower>(Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1m);
        DynamicVars["GainHp"].UpgradeValueBy(5m);
    }
    
}