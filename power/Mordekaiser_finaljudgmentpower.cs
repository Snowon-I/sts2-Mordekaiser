using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.power;

public class Mordekaiser_finaljudgmentpower : PowerModel
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool IsInstanced => true;
    
    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (!Owner.IsDead && Amount == 1)
        {
            foreach (var Enemy in CombatState.HittableEnemies)
            {
                await CreatureCmd.Kill(Enemy);
            }
        }
        await PowerCmd.ModifyAmount(this, -1, Owner, null);
    }
    
}