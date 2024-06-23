using Microsoft.AspNetCore.Mvc;

namespace hng_task1.Controllers
{
    [ApiController]
    [Route("api/hello")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IIpService _ipService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;


        public UserController(ILogger<UserController> logger, IIpService ipService, IUserService userService, ApplicationDbContext context)
        {
            _logger = logger;
            _ipService = ipService;
            _userService = userService;
            _context = context;
        }


        [HttpPost]
        public IActionResult GreetClient([FromQuery] string visitor_name)
        {
            _logger.LogInformation("Handling Log Information");
            var clientIp = _ipService.GetClientIp(HttpContext);
            _logger.LogInformation("Client Ip: {ClientIp}", clientIp);

            var user = _userService.CreateUser(visitor_name, clientIp);

            // Add the new user to the context
            _context.Users.Add(user);

            // Save changes to the database
            _context.SaveChanges();

            return Ok(user);
        }
    }
}