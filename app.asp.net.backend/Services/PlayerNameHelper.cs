//Micro-services that checks if the player name given is already taken.

using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;

namespace app.asp.net.backend.TriviaServices;

public static class PlayerNameHelper
{
    public static async Task<bool> IsNameTakenAsync(TriviaDBContext context, string name)
    {
        var normalized = name.Trim().ToLower();
        return await context.TScores
            .AnyAsync(s => s.SPlayerName.ToLower() == normalized);
    }
}
