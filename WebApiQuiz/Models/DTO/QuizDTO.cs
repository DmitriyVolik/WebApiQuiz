namespace WebApiQuiz.Models.DTO;

public class QuizDTO
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public IEnumerable<string> Options { get; set; }
}