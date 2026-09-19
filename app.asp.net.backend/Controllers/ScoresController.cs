using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;
using app.asp.net.backend.TriviaModels;
using app.asp.net.backend.TriviaDtos;
using app.asp.net.backend.TriviaServices;

namespace app.asp.net.backend.Controllers;

[ApiController]
[Route("api/[controller]")] // -> api/scores
public class ScoresController : ControllerBase
{
    private readonly TriviaDBContext _context;

    public ScoresController(TriviaDBContext context)
    {
        _context = context;
    }

    // POST api/scores
    // Body: { "playerName": "...", "categoryId": 2, "answers": [{ "questionId": 5, "selectedOption": "B" }, ...] }
    [HttpPost]
    public async Task<ActionResult<ScoreResultDto>> SubmitScore([FromBody] SubmitScoreRequest request)
    {
        var name = request.PlayerName?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("PlayerName is required.");
        }

        if (request.Answers.Count == 0)
        {
            return BadRequest("At least one answer is required.");
        }

        bool taken = await PlayerNameHelper.IsNameTakenAsync(_context, name);
        if (taken)
        {
            return Conflict($"The name '{name}' is already taken. Please choose another.");
        }

        // Look up the real questions server-side - never trust a client-reported score.
        var questionIds = request.Answers.Select(a => a.QuestionId).ToList();
        var questions = await _context.TQuestions
            .Where(q => questionIds.Contains(q.Id))
            .ToListAsync();

        int correctCount = 0;
        foreach (var answer in request.Answers)
        {
            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question != null &&
                char.ToUpperInvariant(answer.SelectedOption) == char.ToUpperInvariant(question.QCorrectOption))
            {
                correctCount++;
            }
        }

        var score = new TScore
        {
            SPlayerName = name,
            SCatId = request.CategoryId,
            SCorrect = correctCount,
            STotal = request.Answers.Count,
            SCreatedAt = DateTime.UtcNow
        };

        _context.TScores.Add(score);
        await _context.SaveChangesAsync();

        return Ok(new ScoreResultDto
        {
            Correct = correctCount,
            Total = request.Answers.Count,
            PlayerName = name
        });
    }

    // GET api/scores?categoryId=2&top=10
    [HttpGet]
    public async Task<ActionResult<List<LeaderboardEntryDto>>> GetLeaderboard(
        [FromQuery] int categoryId,
        [FromQuery] int top = 10)
    {
        var results = await _context.TScores
            .Where(s => s.SCatId == categoryId)
            .OrderByDescending(s => s.SCorrect)
            .ThenByDescending(s => s.SCreatedAt)
            .Take(top)
            .Select(s => new LeaderboardEntryDto
            {
                PlayerName = s.SPlayerName,
                Correct = s.SCorrect,
                Total = s.STotal,
                CreatedAt = s.SCreatedAt
            })
            .ToListAsync();

        return Ok(results);
    }
}