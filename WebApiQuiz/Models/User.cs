using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiQuiz.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set;}
    
    [Required]
    [MinLength(8)]
    [PasswordPropertyText]
    public string Password { get; set;}
}