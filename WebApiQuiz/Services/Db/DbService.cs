using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiQuiz.Models;
using WebApiQuiz.Models.Options;

namespace WebApiQuiz.Services.Db;

public class DbService
{
    private readonly MongoClient _client;

    private readonly IMongoDatabase _database;

    public DbService(IOptions<DbOptions> config)
    {
        _client = new MongoClient(config.Value.ConnectionString);
        _database = _client.GetDatabase(config.Value.Database);

        if (!_database.ListCollections().Any())
        {
            List<Quiz> quizzesSeed = new List<Quiz>
            {
                new()
                {
                    Title = "The Java Logo",
                    Text = "What is depicted on the Java logo?",
                    Options = new List<string> {"Robot", "Tea leaf", "Cup of coffee", "Bug"},
                    Answers = new List<int> {2}
                },
                new()
                {
                    Title = "The Ultimate Question",
                    Text = "What is the answer to the Ultimate Question of Life, the Universe and Everything?",
                    Options = new List<string> {"Everything goes right", "42", "2+2=4", "11011100"},
                    Answers = new List<int> {3, 4}
                }
            };
            
            var userSeed = new User()
            {
                Email = "user@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Passw0rd%")
            };
            
            _database.GetCollection<Quiz>("Quizzes").InsertMany(quizzesSeed);
            _database.GetCollection<User>("Users").InsertOne(userSeed);
        }
    }

    public List<Quiz> GetAllQuizzes()
    {
        return _database.GetCollection<Quiz>("Quizzes")
            .Find(x => true).ToList();
    }

    public Quiz? GetQuizById(string id)
    {
        return _database.GetCollection<Quiz>("Quizzes")
            .Find(x => x.Id == id)
            .FirstOrDefault();
    }

    public void AddQuiz(Quiz quiz)
    {
        _database.GetCollection<Quiz>("Quizzes")
            .InsertOne(quiz);
    }
    
    public void DeleteQuiz(string id)
    {
        _database.GetCollection<Quiz>("Quizzes")
            .DeleteOne(x => x.Id == id);
    }
    
    public User? GetUserByEmail(string email)
    {
        return _database.GetCollection<User>("Users")
            .Find(x => x.Email == email)
            .FirstOrDefault();
    }
    
    public bool AddUser(User user)
    {
        if (GetUserByEmail(user.Email) is null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _database.GetCollection<User>("Users")
                .InsertOne(user);
            return true;
        }
        return false;
    }

    public bool LoginUser(User userData)
    {
        var user = GetUserByEmail(userData.Email);
        if (user is null) return false;
        userData.Id = user.Id;
        
        return BCrypt.Net.BCrypt.Verify(userData.Password, user.Password);
    }
    
}