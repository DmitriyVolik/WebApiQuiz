using MongoDB.Bson;

namespace WebApiQuiz.Models.DTO;

public class QuizDto
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public IEnumerable<string> Options { get; set; }
}