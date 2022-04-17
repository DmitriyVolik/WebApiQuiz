using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiQuiz.Models;

namespace WebApiQuiz.Services.Db;

public class DbService
{
    private readonly MongoClient _client;

    private readonly IMongoDatabase _database;

    public DbService(IOptions<DbConfig> config)
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
        Console.WriteLine(quiz.Id);
        _database.GetCollection<Quiz>("Quizzes")
            .InsertOne(quiz);
    }
    
}