using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;
using app.asp.net.backend.TriviaDtos;

namespace app.asp.net.backend.Controllers;

[ApiController]
[Route("api/[controller]")] // -> api/questions
public class QuestionsController : ControllerBase
{
    private readonly TriviaDBContext _context;

    public QuestionsController(TriviaDBContext context)
    {
        _context = context;
    }

    // GET api/questions?categoryId=2&count=5
    [HttpGet]
    public async Task<ActionResult<List<QuestionDto>>> GetQuestions(
        [FromQuery] int categoryId,
        [FromQuery] int count = 5)
    {
        var questionsInCategory = await _context.TQuestions
            .Where(q => q.QCatId == categoryId)
            .ToListAsync();

        if (questionsInCategory.Count == 0)
        {
            return NotFound($"No questions found for category {categoryId}.");
        }

        // Simple in-memory shuffle - fine at this scale (45 questions per category).
        var random = new Random();
        var selected = questionsInCategory
            .OrderBy(_ => random.Next())
            .Take(count)
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.QText,
                OptionA = q.QOptionA,
                OptionB = q.QOptionB,
                OptionC = q.QOptionC,
                OptionD = q.QOptionD
                // QCorrectOption is intentionally left out of QuestionDto.
            })
            .ToList();

        return Ok(selected);
    }
}