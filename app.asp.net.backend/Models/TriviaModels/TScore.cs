using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.asp.net.backend.TriviaModels;

public class TScore
{
    public int Id { get; set; }
    public int SCatId { get; set; }
    public string SPlayerName { get; set; } = string.Empty;

    [ForeignKey(nameof(SCatId))]
    public TCategory? SCategory { get; set; }
    public int SCorrect { get; set; }
    public int STotal { get; set; }
    public DateTime SCreatedAt { get; set; } = DateTime.UtcNow;
}