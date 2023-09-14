using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreMVC_WebApi_Test.Data;
using CoreMVC_WebApi_Test.Models;
using Microsoft.AspNetCore.Identity;
using CoreMVC_WebApi_Test.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using CoreMVC_WebApi_Test.DTO_s;
using CoreMVC_WebApi_Test.DTOs;

namespace CoreMVC_WebApi_Test.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PubsContext _context;

        public PublishersController(
            UserManager<ApplicationUser> userManager,
            PubsContext context,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        [Route("Login")] // /login
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var guid = Guid.NewGuid().ToString();
                // https://datatracker.ietf.org/doc/html/rfc7519#section-4
                var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, guid),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id)
                };

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, role);
                    claims.Add(roleClaim);
                }

                var signingKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  claims: claims,
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(
                  new
                  {
                      access_token = new JwtSecurityTokenHandler().WriteToken(token),
                      userName = model.Username,
                      expiration = token.ValidTo
                  });
            }
            return Unauthorized();
        }

        // GET: api/Publishers
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetPublishers()
        {
          if (_context.Publishers == null)
          {
              return NotFound();
          }
            return await (from publisher in _context.Publishers
                          select new PublisherDTO
                          {
                              PubId = publisher.PubId,
                              PubName = publisher.PubName,
                              City = publisher.City,
                              State = publisher.State,
                              Country = publisher.Country
                          }).ToListAsync();
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PublisherDTO>> GetPublisher(string id)
        {
          if (_context.Publishers == null)
          {
              return NotFound();
          }
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return new PublisherDTO 
            {
                PubId = publisher.PubId,
                PubName = publisher.PubName,
                City = publisher.City,
                State = publisher.State,
                Country = publisher.Country
            };
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPublisher(string id, PublisherDTO publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(new Publisher 
                {
                    PubId = publisher.PubId,
                    PubName = publisher.PubName,
                    City = publisher.City,
                    State = publisher.State,
                    Country = publisher.Country
                }).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Publishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Publisher>> PostPublisher(PublisherDTO publisher)
        {
          if (_context.Publishers == null)
          {
              return Problem("Entity set 'PubsContext.Publishers'  is null.");
          }
            _context.Publishers.Add(new Publisher
            {
                PubId = publisher.PubId,
                PubName = publisher.PubName,
                City = publisher.City,
                State = publisher.State,
                Country = publisher.Country
            });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PublisherExists(publisher.PubId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePublisher(string id)
        {
            if (_context.Publishers == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Titles.RemoveRange(_context.Titles.Where(t => t.PubId == id));
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublisherExists(string id)
        {
            return (_context.Publishers?.Any(e => e.PubId == id)).GetValueOrDefault();
        }
    }
}
