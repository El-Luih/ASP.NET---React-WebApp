using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;
using app.asp.net.backend.TriviaModels;
using app.asp.net.backend.TriviaDtos;
using app.asp.net.backend.TriviaServices;

namespace app.asp.net.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoresController : ControllerBase
{
    private readonly TriviaDBContext _context;

    public ScoresController(TriviaDBContext context)
    {
        _context = context;
    }

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

        if (request.Answers.Select(answer => answer.QuestionId).Distinct().Count() != request.Answers.Count)
        {
            return BadRequest("Each question may only be answered once.");
        }

        if (!await _context.TCategories.AnyAsync(category => category.Id == request.CategoryId))
        {
            return BadRequest($"Category {request.CategoryId} was not found.");
        }

        bool taken = await PlayerNameHelper.IsNameTakenAsync(_context, name);
        if (taken)
        {
            return Conflict($"The name '{name}' is already taken. Please choose another.");
        }

        var questionIds = request.Answers.Select(a => a.QuestionId).ToList();
        var questions = await _context.TQuestions
            .Where(q => q.QCatId == request.CategoryId && questionIds.Contains(q.Id))
            .ToListAsync();

        if (questions.Count != questionIds.Count)
        {
            return BadRequest("Every submitted question must belong to the selected category.");
        }

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

    [HttpGet]
    public async Task<ActionResult<List<LeaderboardEntryDto>>> GetLeaderboard(
        [FromQuery] int categoryId,
        [FromQuery] int top = 10)
    {
        if (categoryId <= 0 || top <= 0)
        {
            return BadRequest("categoryId and top must be positive.");
        }

        var results = await _context.TScores
            .Where(s => s.SCatId == categoryId)
            .OrderByDescending(s => s.SCorrect)
            .ThenBy(s => s.SCreatedAt)
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