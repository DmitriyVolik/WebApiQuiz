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
            var collection = _database.GetCollection<Quiz>("Quizzes");
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
            collection.InsertMany(quizzesSeed);
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
    
    public bool DeleteQuiz(string id)
    {
        return _database.GetCollection<Quiz>("Quizzes")
            .DeleteOne(x => x.Id == id).DeletedCount > 0;
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
            _database.GetCollection<User>("Users")
                .InsertOne(user);
            return true;
        }
        return false;
    }

    public bool LoginUser(User userData)
    {
        var user = GetUserByEmail(userData.Email);
        
        if (user is not null && user.Password == userData.Password)
        {
            return true;
        }

        return false;
    }
    
}