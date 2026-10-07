namespace SpireBot.SpireBotCode.API;

public record GameState(
    int CurrentHp,
    int MaxHp,
    int MaxEnergy,
    int Gold,
    int PotionsSlotCount,
    int OrbSlotCount,
        
    IEnumerable<CardRecord> DrawPile,
    IEnumerable<CardRecord> DiscardPile,
    IEnumerable<CardRecord> ExhaustPile,
    IEnumerable<CardRecord> HandPile,
    
    IEnumerable<RelicRecord> Relics,
    IEnumerable<PotionRecord> Potions
);