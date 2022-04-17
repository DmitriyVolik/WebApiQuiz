using Microsoft.AspNetCore.Mvc;
using WebApiQuiz.Models;
using WebApiQuiz.Models.DTO;
using WebApiQuiz.Services.Db;

namespace WebApiQuiz.Controllers;

[ApiController]
[Route("Quizzes")]
public class QuizController : ControllerBase
{
    private readonly ILogger<QuizController> _logger;
    
    private readonly DbService _dbService;
    
    public QuizController(ILogger<QuizController> logger, DbService dbService)
    {
        _logger = logger;
        _dbService = dbService;
    }
    
    [HttpGet("{id}")]
    public IActionResult GetQuizById(string id)
    {
        Quiz? quiz;
        try
        {
            quiz = _dbService.GetQuizById(id);
        }
        catch (Exception e)
        {
            return BadRequest("Id is incorrect");
        }
        
        if (quiz is null)
        {
            return NotFound();
        }
        
        return Ok(QuizToDto(quiz));
    }
    
    [HttpGet]
    public IEnumerable<QuizDto> GetQuizzes()
    {
        return _dbService.GetAllQuizzes().Select(QuizToDto);
    }
    
    [HttpPost]
    public IActionResult CreateQuiz(Quiz newQuiz)
    {
        _dbService.AddQuiz(newQuiz);
        
        return Ok(QuizToDto(newQuiz));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteQuiz(string id)
    {
        try
        {
            if (!_dbService.DeleteQuiz(id))
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            return BadRequest("Id is incorrect");
        }
        
        return Ok();
    }

    [HttpPost("{id}/Solve")]
    public IActionResult Solve(string id, int[] answer)
    {
        var quiz = _dbService.GetQuizById(id);
    
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
    
    private static QuizDto QuizToDto(Quiz? quiz)
    {
        return new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Text = quiz.Text,
            Options = quiz.Options,
        };
    }
}