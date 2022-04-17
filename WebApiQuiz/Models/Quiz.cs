using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiQuiz.Models;

public class Quiz
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public List<string> Options { get; set; }
    
    public List<int> Answers { get; set; }
}