using System.Net;
using System.Net.Http.Json;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SpireBot.SpireBotCode.Extensions;

public class SpireBotAPI
{
    private HttpClient _client = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:8000/")
    };
    
    public record ContextResponse(string Context);

    public record EnergyCostRecord(
        bool CostsX,
        int Canonical
    );
    
    //TODO: Add rest of game state and probably refactor this
    public record CardRecord(
        string Id,
        string Type,
        EnergyCostRecord EnergyCost,
        string TargetType,
        IEnumerable<String> Keywords,
        IEnumerable<String> Tags,
        //DynamicVarSet DynamicVars,
        //EnchantmentModel? Enchantment,
        //AfflictionModel? Affliction,
        bool IsUpgraded,
        int BaseReplayCount,
        bool ShouldRetainThisTurn,
        bool IsSlyThisTurn,
        bool GainsBlock,
        string OrbEvokeType,
        bool ExhaustOnNextPlay,
        int CurrentStarCost
    );
    

    public record GameState(
        int CurrentHp,
        int MaxHp,
        int MaxEnergy,
        int Gold,
        int PotionsSlotCount,
        int OrbSlotCount,
        
        IEnumerable<CardRecord> DrawPile,
        IEnumerable<CardRecord> DiscardPile,
        IEnumerable<CardRecord> ExhaustPile
    );
    
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
            MainFile.Logger.Info(responseObject?.Context);
        }
        catch (HttpRequestException e)
        {
            MainFile.Logger.Error(e.Message);
        }

    }

    private CardRecord ConstructCardRecord(CardModel card)
    {
        return new CardRecord(
            card.Id.ToString(),
            card.Type.ToString(),
            new EnergyCostRecord(card.EnergyCost.CostsX, card.EnergyCost.Canonical),
            card.TargetType.ToString(),
            card.Keywords.Select(k => k.ToString()),
            card.Tags.Select(k => k.ToString()),
            //card.DynamicVars,
            //card.Enchantment,
            //card.Affliction,
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
        
        List<CardRecord> DrawPile = new();
        List<CardRecord> DiscardPile = new();
        List<CardRecord> ExhaustPile = new();

        foreach (CardPile item in player.Piles ?? Array.Empty<CardPile>())
        {
            switch (item.Type)
            {
                case PileType.Draw:
                    foreach (CardModel card in item.Cards)
                    {
                        DrawPile.Add(ConstructCardRecord(card));
                    }
                    break;
                case PileType.Discard:
                    foreach (CardModel card in item.Cards)
                    {
                        DiscardPile.Add(ConstructCardRecord(card));
                    }
                    break;
                case PileType.Exhaust:
                    foreach (CardModel card in item.Cards)
                    {
                        ExhaustPile.Add(ConstructCardRecord(card));
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
            DrawPile, 
            DiscardPile, 
            ExhaustPile
        );
    }
}