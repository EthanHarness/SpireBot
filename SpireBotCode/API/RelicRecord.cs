namespace SpireBot.SpireBotCode.API;

public record RelicRecord(
    string Id,
    string Status,
    bool IsTradeable,
    bool IsUsedUp,
    bool ShowCounter,
    int DisplayAmount,
    bool IsWax,
    bool IsMelted,
    bool HasBeenRemovedFromState,
    Dictionary<string, DynamicVarRecord> DynamicVars
);