using Microsoft.AspNetCore.Mvc;
using app.asp.net.backend.TriviaData;
using app.asp.net.backend.TriviaDtos;
using app.asp.net.backend.TriviaServices;

namespace app.asp.net.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly TriviaDBContext _context;

    public PlayersController(TriviaDBContext context)
    {
        _context = context;
    }

    [HttpPost("verify-name")]
    public async Task<ActionResult<NameCheckResponse>> VerifyName([FromBody] NameCheckRequest request)
    {
        // This is a friendly pre-check; ScoresController verifies again before saving.
        var name = request.PlayerName?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            return Ok(new NameCheckResponse
            {
                Available = false,
                Message = "Please enter a name to continue."
            });
        }

        bool taken = await PlayerNameHelper.IsNameTakenAsync(_context, name);

        if (taken)
        {
            return Ok(new NameCheckResponse
            {
                Available = false,
                Message = $"'{name}' is already taken. Please choose another name."
            });
        }

        return Ok(new NameCheckResponse { Available = true });
    }
}