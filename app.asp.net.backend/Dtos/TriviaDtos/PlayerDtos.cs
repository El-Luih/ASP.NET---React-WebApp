namespace app.asp.net.backend.TriviaDtos;


public class NameCheckRequest
{

    public string? PlayerName { get; set; }
}

public class NameCheckResponse
{
    public bool Available { get; set; }

    public string? Message { get; set; }
}
