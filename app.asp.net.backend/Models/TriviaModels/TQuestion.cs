using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.asp.net.backend.TriviaModels;

public class TQuestion
{
    // QCatId links each question to the category that owns it.
    public int Id { get; set; }
    public int QCatId { get; set; }

    [ForeignKey(nameof(QCatId))]
    public TCategory? QCategory { get; set; }
    public string QText { get; set; } = string.Empty;

    public string QOptionA { get; set; } = string.Empty;
    public string QOptionB { get; set; } = string.Empty;
    public string QOptionC { get; set; } = string.Empty;
    public string QOptionD { get; set; } = string.Empty;
    // This value stays in the database and is never included in QuestionDto.
    public char QCorrectOption { get; set; }
}