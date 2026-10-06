using System.Net;
using System.Net.Http.Json;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SpireBot.SpireBotCode.API;

public class SpireBotApi
{
    private HttpClient _client = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:8000/")
    };
    
    public record ContextResponse(string Context);
    
    private async Task<HttpResponseMessage> MakeRequestAsync(PlayerChoiceContext choiceContext, Player player)
    {
        GameState gameState = ConstructGameState(player); 
        HttpResponseMessage response = await _client.PostAsJsonAsync($"/context", gameState);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task RunAsync(PlayerChoiceContext choiceContext, Player player)
    {
        try
        {
            HttpResponseMessage response = await MakeRequestAsync(choiceContext, player);
            ContextResponse? responseObject = await response.Content.ReadFromJsonAsync<ContextResponse>();
            MainFile.Logger.Info(responseObject?.Context ?? "");
        }
        catch (HttpRequestException e)
        {
            MainFile.Logger.Error(e.Message);
        }

    }

    private CardRecord ConstructCardRecord(CardModel card)
    {
        Dictionary<string, DynamicVarRecord> dynamicVars = new Dictionary<string, DynamicVarRecord>();
        foreach (KeyValuePair<string, DynamicVar> kvp in card.DynamicVars)
        {
            string key = kvp.Key;
            DynamicVar dynamicVar = kvp.Value;
            
            DynamicVarRecord dynamicVarRecord = new DynamicVarRecord(dynamicVar.Name, dynamicVar.BaseValue);
            dynamicVars.Add(key, dynamicVarRecord);
        }
        
        return new CardRecord(
            card.Id.ToString(),
            card.Type.ToString(),
            new EnergyCostRecord(card.EnergyCost.CostsX, card.EnergyCost.Canonical),
            card.TargetType.ToString(),
            card.Keywords.Select(k => k.ToString()),
            card.Tags.Select(k => k.ToString()),
            dynamicVars,
            card.Enchantment?.ToSerializable(),
            card.Affliction?.GetType().Name,
            card.IsUpgraded,
            card.BaseReplayCount,
            card.ShouldRetainThisTurn,
            card.IsSlyThisTurn,
            card.GainsBlock,
            card.OrbEvokeType.ToString(),
            card.ExhaustOnNextPlay,
            card.CurrentStarCost
        );
    }

    private GameState ConstructGameState(Player player)
    {
        
        List<CardRecord> drawPile = [];
        List<CardRecord> discardPile = [];
        List<CardRecord> exhaustPile = [];
        List<CardRecord> handPile = [];

        foreach (CardPile item in player.Piles ?? Array.Empty<CardPile>())
        {
            switch (item.Type)
            {
                case PileType.Draw:
                    foreach (CardModel card in item.Cards)
                    {
                        drawPile.Add(ConstructCardRecord(card));
                    }
                    break;
                case PileType.Discard:
                    foreach (CardModel card in item.Cards)
                    {
                        discardPile.Add(ConstructCardRecord(card));
                    }
                    break;
                case PileType.Exhaust:
                    foreach (CardModel card in item.Cards)
                    {
                        exhaustPile.Add(ConstructCardRecord(card));
                    }
                    break;
                case PileType.Hand:
                    foreach (CardModel card in item.Cards)
                    {
                        handPile.Add(ConstructCardRecord(card));
                    }
                    break;
            }
        } 

        return new GameState(
            player.Creature.CurrentHp,
            player.Creature.MaxHp,
            player.MaxEnergy,
            player.Gold,
            player.MaxPotionCount,
            player.BaseOrbSlotCount,
            drawPile, 
            discardPile, 
            exhaustPile,
            handPile
        );
    }
}