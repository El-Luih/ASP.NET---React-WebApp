using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;
using app.asp.net.backend.TriviaDtos;

namespace app.asp.net.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly TriviaDBContext _context;

    public QuestionsController(TriviaDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuestionDto>>> GetQuestions(
        [FromQuery] int categoryId,
        [FromQuery] int count = 7)
    {
        // Validate the request before touching the database.
        if (categoryId <= 0 || count <= 0)
        {
            return BadRequest("categoryId and count must be positive.");
        }

        var questionsInCategory = await _context.TQuestions
            .Where(q => q.QCatId == categoryId)
            .ToListAsync();

        if (questionsInCategory.Count == 0)
        {
            return NotFound($"No questions found for category {categoryId}.");
        }

        // Shuffling on the server prevents every player from seeing the same order.
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
            })
            .ToList();

        return Ok(selected);
    }
}