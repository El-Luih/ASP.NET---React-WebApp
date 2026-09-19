using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;
using app.asp.net.backend.TriviaDtos;

namespace app.asp.net.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly TriviaDBContext _context;

    public CategoriesController(TriviaDBContext context)
    {
        _context = context;
    }

    // GET api/categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _context.TCategories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.CatName
            })
            .ToListAsync();

        return Ok(categories);
    }
}