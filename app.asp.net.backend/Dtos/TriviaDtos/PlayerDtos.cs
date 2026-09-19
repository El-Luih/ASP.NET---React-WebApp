namespace app.asp.net.backend.TriviaDtos;


public class NameCheckRequest
{
    // The frontend sends the proposed name for a quick availability check.
    public string? PlayerName { get; set; }
}

public class NameCheckResponse
{
    public bool Available { get; set; }

    public string? Message { get; set; }
}
