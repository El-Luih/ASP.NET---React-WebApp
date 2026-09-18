using System.Text.Json;
using System.Text.Json.Serialization;
using app.asp.net.backend.TriviaModels;

namespace app.asp.net.backend.TriviaData;

// These classes only exist to match the shape of seed-data.json for deserialization. Their property names stay tied to the JSON file's keys (via JsonPropertyName).
public class SeedFile
{
    [JsonPropertyName("categories")]
    public List<SeedCategory> Categories { get; set; } = new();
}

public class SeedCategory
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("questions")]
    public List<SeedQuestion> Questions { get; set; } = new();
}

public class SeedQuestion
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("optionA")]
    public string OptionA { get; set; } = string.Empty;

    [JsonPropertyName("optionB")]
    public string OptionB { get; set; } = string.Empty;

    [JsonPropertyName("optionC")]
    public string OptionC { get; set; } = string.Empty;

    [JsonPropertyName("optionD")]
    public string OptionD { get; set; } = string.Empty;

    [JsonPropertyName("correctOption")]
    public string CorrectOption { get; set; } = string.Empty;
}

public static class DbInitializer
{
    public static void Seed(TriviaDBContext context, string contentRootPath)
    {
        if (context.TCategories.Any())
        {
            return;
        }

        var jsonPath = Path.Combine(contentRootPath, "trivia-seed-data.json");
        var json = File.ReadAllText(jsonPath);
        var seedFile = JsonSerializer.Deserialize<SeedFile>(json)
            ?? throw new InvalidOperationException("trivia-seed-data.json could not be parsed.");

        foreach (var seedCategory in seedFile.Categories)
        {
            var category = new TCategory { CatName = seedCategory.Name };
            context.TCategories.Add(category);

            foreach (var sq in seedCategory.Questions)
            {
                context.TQuestions.Add(new TQuestion
                {
                    QCategory = category, // EF Core resolves QCatId from this
                    QText = sq.Text,
                    QOptionA = sq.OptionA,
                    QOptionB = sq.OptionB,
                    QOptionC = sq.OptionC,
                    QOptionD = sq.OptionD,
                    QCorrectOption = sq.CorrectOption[0] // "B" -> 'B'
                });
            }
        }

        context.SaveChanges();
    }
}
