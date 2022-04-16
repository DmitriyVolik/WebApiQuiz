namespace WebApiQuiz.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public List<string> Options { get; set; }
    
    public List<int> Answers { get; set; }
}