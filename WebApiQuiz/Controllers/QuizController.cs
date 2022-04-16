using Microsoft.AspNetCore.Mvc;
using WebApiQuiz.Models;
using WebApiQuiz.Models.DTO;

namespace WebApiQuiz.Controllers;

[ApiController]
[Route("[controller]")]
public class QuizController : ControllerBase
{
    private static readonly List<Quiz> Quizzes = new List<Quiz>
    {
        new()
        {
            Id = 1,
            Title = "The Java Logo",
            Text = "What is depicted on the Java logo?",
            Options = new List<string> {"Robot", "Tea leaf", "Cup of coffee", "Bug"},
            Answers = new List<int> {2}
        },
        new()
        {
            Id = 2,
            Title = "The Ultimate Question",
            Text = "What is the answer to the Ultimate Question of Life, the Universe and Everything?",
            Options = new List<string> {"Everything goes right","42","2+2=4","11011100"},
            Answers = new List<int> {3,4}
        }
    };

    private readonly ILogger<QuizController> _logger;

    public QuizController(ILogger<QuizController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("{id}")]
    public IActionResult GetQuizById(int id)
    {
        var quiz = Quizzes.FirstOrDefault(x => x.Id == id);

        if (quiz is null)
        {
            return NotFound();
        }
        
        return Ok(QuizToDto(quiz));
    }
    
    [HttpGet]
    public IEnumerable<QuizDTO> GetQuizzes()
    {
        return Quizzes.Select(QuizToDto);
    }

    [HttpPost("{id}/Solve")]
    public IActionResult Solve(int id, int[] answer)
    {
        var quiz = Quizzes.FirstOrDefault(x=>x.Id == id);

        if (quiz == null)
        {
            return NotFound();
        }
        
        var correct = true;
        
        for (var i = 0; i < quiz.Answers.Count(); i++)
        {
            if (quiz.Answers[i] != answer[i])
            {
                correct = false;
                break;
            }
        }

        object response;
        if (correct)
        {
            response = new
            {
                success = true,
                feedback = "Congratulations, you're right!",
            };
        }
        else
        {
            response = new
            {
                success = false,
                feedback = "Wrong answer! Please, try again.",
            };
        }
        return Ok(response);
    }
    
    private static QuizDTO QuizToDto(Quiz quiz)
    {
        return new QuizDTO
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Text = quiz.Text,
            Options = quiz.Options,
        };
    }
}