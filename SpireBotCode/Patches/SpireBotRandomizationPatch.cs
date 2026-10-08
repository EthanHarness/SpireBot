using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Random;

namespace SpireBot.SpireBotCode.Patches;

public static class SpireBotRandomizationState
{
    public static readonly HashSet<ulong> SpireBotPlayers = new();
}

[HarmonyPatch(typeof(StartRunLobby), "BeginRunLocally")]
public static class SpireBotRandomizationPatch
{
    // ReSharper disable once InconsistentNaming
    public static void Prefix(StartRunLobby __instance, string seed, List<ModifierModel> modifiers)
    {
        MainFile.Logger.Info("Randomizing the SpireBot bitch");
        
        Rng rng = new Rng(StringHelper.GetDeterministicHashCode(seed), "spirebot_character_selection");
        for (int index = 0; index < __instance.Players.Count; ++index)
        {
            StartRunLobbyPlayer player = __instance.Players[index];
            if (player.character is Character.SpireBot)
            {
                MainFile.Logger.Info($"Adding SpireBot player {player.id} to map.");
                SpireBotRandomizationState.SpireBotPlayers.Add(player.id);
                
                CharacterModel character = rng.NextItem(ModelDb.AllCharacters) ?? ModelDb.Character<Ironclad>();
                Traverse.Create(__instance).Method("ChangeCharacter", player.id, character, true).GetValue();
            }
        }
    }
}