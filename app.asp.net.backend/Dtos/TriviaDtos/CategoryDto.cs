namespace app.asp.net.backend.TriviaDtos;

public class CategoryDto
{
    // The frontend only needs the category ID and display name.
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
