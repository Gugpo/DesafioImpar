using Microsoft.AspNetCore.Mvc;

namespace Impar.BackEnd.Evaluation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesDbContext _dbContext;
        public MessagesController(MessagesDbContext ctx)
        {
            _dbContext = ctx;
        }

        [HttpGet]
        [Route("status")]
        public IActionResult Status()
        {
            return Ok("Mostre o status do envio aqui");
        }


        [HttpPost]
        [Route("send")]
        public IActionResult SendMessages()
        {
            try
            {
                // Obtem os usuários totais
                List<User> totalUsers = new();
                totalUsers = _dbContext.Users.ToList();

                int seed = totalUsers.Count() < 4000 ? totalUsers.Count() : 4000;

                for (int i = 0; i < totalUsers.Count; i+= seed)
                {
                    List<User> users = totalUsers.Skip(i).Take(seed).ToList();
                    // Manda as mensagens
                    foreach (User user in users)
                    {
                        // Não precisa fazer o envio de fato, este Thread.Sleep
                        // apenas simula o tempo gasto no envio
                        Thread.Sleep(500);

                        // Salva no banco a mensagem enviada
                        _dbContext.Messages.Add(new Message
                        {
                            MessageContent = $"Esta é uma mensagem enviada para {user.Name} ({user.Email})",
                            SentAt= DateTime.UtcNow,
                            Subject = $"User: {user.Name}",
                            UserId = user.Id
                        });
                    }
                    _dbContext.SaveChanges();
                }
                // Retorna Ok (200)
                return Ok();
            }
            catch 
            {

            }
            
        }
    }
}
