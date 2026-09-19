namespace app.asp.net.backend.TriviaModels;

public class TCategory
{
    // Categories are the top-level grouping used by the quiz and leaderboard.
    public int Id { get; set; }
    public string CatName { get; set; } = string.Empty;
}

