namespace app.asp.net.backend.TriviaDtos;


public class SubmitScoreRequest
{
    public string PlayerName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public List<AnswerDto> Answers { get; set; } = new();
}

public class AnswerDto
{
    public int QuestionId { get; set; }
    public char SelectedOption { get; set; } // 'A', 'B', 'C', or 'D'
}


public class ScoreResultDto
{
    public int Correct { get; set; }
    public int Total { get; set; }
    public string PlayerName { get; set; } = string.Empty;
}


public class LeaderboardEntryDto
{
    public string PlayerName { get; set; } = string.Empty;
    public int Correct { get; set; }
    public int Total { get; set; }
    public DateTime CreatedAt { get; set; }
}