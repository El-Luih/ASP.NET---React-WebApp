using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;

namespace app.asp.net.backend.TriviaServices;

public static class PlayerNameHelper
{
    public static async Task<bool> IsNameTakenAsync(TriviaDBContext context, string name)
    {
        // Comparing normalized names makes "Luis" and "luis" the same username.
        var normalized = name.Trim().ToLower();
        return await context.TScores
            .AnyAsync(s => s.SPlayerName.ToLower() == normalized);
    }
}
