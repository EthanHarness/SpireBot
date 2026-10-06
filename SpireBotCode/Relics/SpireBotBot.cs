using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

using SpireBot.SpireBotCode.Relics;
using SpireBot.SpireBotCode.Extensions;

namespace SpireBot.SpireBotCode.Relics;

  
public class SpireBotBot() : SpireBotRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    private SpireBotAPI _api = new  SpireBotAPI();
    
    public override async Task AfterAutoPrePlayPhaseEnteredLate(PlayerChoiceContext choiceContext, Player player)
    {
        await _api.RunAsync(choiceContext, player);
        
        ICombatState combatState;
        if (player != this.Owner)
        {
            combatState = null;
        }
        else
        {
            combatState = player.Creature.CombatState;
            this.Flash();
            bool flag;
            using (CardSelectCmd.PushSelector((ICardSelector) new VakuuCardSelector()))
            {
                int cardsPlayed = 0;
                int startTurn = this.Owner.PlayerCombatState.TurnNumber;
                while (cardsPlayed < 13 && !CombatManager.Instance.IsOverOrEnding && !CombatManager.Instance.IsPlayerReadyToEndTurn(player) && this.Owner.PlayerCombatState.TurnNumber == startTurn)
                {
                    CardModel card = PileType.Hand.GetPile(this.Owner).Cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c.CanPlay()));
                    if (card != null)
                    {
                        Creature target = this.GetTarget(card, combatState);
                        (int, int) valueTuple = await card.SpendResources();
                        await CardCmd.AutoPlay(choiceContext, card, target, skipXCapture: true);
                        ++cardsPlayed;
                        card = (CardModel) null;
                        target = (Creature) null;
                    }
                    else
                        break;
                }
                flag = cardsPlayed >= 13;
                if (cardsPlayed == 0)
                {
                    combatState = (ICombatState) null;
                    return;
                }
            }
            TalkCmd.Play(flag ? new LocString("relics", "WHISPERING_EARRING.warning") : new LocString("relics", "WHISPERING_EARRING.approval"), this.Owner.Creature, VfxColor.Purple);
            combatState = (ICombatState) null;
            
        }
    }
    
    private Creature? GetTarget(CardModel card, ICombatState combatState)
    {
        Rng combatTargets = this.Owner.RunState.Rng.CombatTargets;
        Creature target;
        switch (card.TargetType)
        {
            case TargetType.AnyEnemy:
                target = combatState.HittableEnemies.FirstOrDefault<Creature>();
                break;
            case TargetType.AnyPlayer:
                target = this.Owner.Creature;
                break;
            case TargetType.AnyAlly:
                target = combatTargets.NextItem<Creature>(combatState.Allies.Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer && c != this.Owner.Creature)));
                break;
            default:
                target = (Creature) null;
                break;
        }
        return target;
    }
}