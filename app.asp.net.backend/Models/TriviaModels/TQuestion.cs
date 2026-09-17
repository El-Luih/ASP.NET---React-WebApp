using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.asp.net.backend.TriviaModels;

public class TQuestion
{
    public int Id { get; set; }
    public int QCatId { get; set; }

    [ForeignKey(nameof(QCatId))]
    public TCategory? QCategory { get; set; }
    public string QText { get; set; } = string.Empty;

    public string QOptionA { get; set; } = string.Empty;
    public string QOptionB { get; set; } = string.Empty;
    public string QOptionC { get; set; } = string.Empty;
    public string QOptionD { get; set; } = string.Empty;
    public char QCorrectOption { get; set; } //A, B, C, or D
}