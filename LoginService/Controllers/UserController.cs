using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginService.DBContext;
using LoginService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserContext _context;

        //Constructor to intialize InMemoryDB
        public UserController(UserContext context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                _context.Users.Add(
                    new User { Email = "Item1", Password = "123456"}
                );
                _context.SaveChanges();
            }
        }

        //Get all Users from DB
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var item = _context.Users.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User user)
        {
            if(user == null || user.Id != id)
            {
                return BadRequest();
            }

            var lst_user = _context.Users.FirstOrDefault(t => t.Id == id);
            if(user == null)
            {
                return NotFound();
            }

            lst_user.Email = user.Email;
            lst_user.Password = user.Email;

            _context.Users.Update(lst_user);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
