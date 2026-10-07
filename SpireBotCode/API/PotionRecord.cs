namespace SpireBot.SpireBotCode.API;

public record PotionRecord(
    string Id,
    string PotionUsage,
    string PotionRarity,
    string TargetType,
    bool HasBeenRemovedFromState,
    Dictionary<string, DynamicVarRecord> DynamicVars
);