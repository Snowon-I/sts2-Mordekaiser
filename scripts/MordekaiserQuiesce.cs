using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.scripts;

public class MordekaiserQuiesceEvent(
    CardModel card,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history)
    : CombatHistoryEntry(card.Owner.Creature, roundNumber, currentSide, history)
{
    public CardModel Card { get; } = card;

    public override string Description => $"{Actor.Player!.Character.Id.Entry}因{Card.Id.Entry}触发了寂灭效果";
    
}

public static class MordekaiserQuiesceTriggerHistoryExtensions
{
    
    private static readonly MethodInfo? _addMethod = typeof(CombatHistory).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
    
    // 扩展History
    public static void MordekaiserQuiesceTrigger(this CombatHistory history,CombatState combatState, CardModel card)
    {
        var QuiesceEvent = new MordekaiserQuiesceEvent(card,combatState.RoundNumber,combatState.CurrentSide,history);
        if (_addMethod != null)
            _addMethod.Invoke(history, [QuiesceEvent]);
    }
    
    public static IEnumerable<MordekaiserQuiesceEvent> MordekaiserQuiesceEvents(this CombatHistory history)
    {
        return history.Entries.OfType<MordekaiserQuiesceEvent>();
    }

}
