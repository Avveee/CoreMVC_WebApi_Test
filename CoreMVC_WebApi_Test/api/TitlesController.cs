using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreMVC_WebApi_Test.Data;
using CoreMVC_WebApi_Test.Models;
using CoreMVC_WebApi_Test.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using CoreMVC_WebApi_Test.DTO_s;

namespace CoreMVC_WebApi_Test.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PubsContext _context;

        public TitlesController(
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

        // GET: api/Titles
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TitleDTO>>> GetTitles()
        {
          if (_context.Titles == null)
          {
              return NotFound();
          }
            return await (from title in _context.Titles
                          select new TitleDTO
                          {
                              TitleId= title.TitleId,
                              Title1= title.Title1,
                              Type= title.Type,
                              Price= title.Price,
                              Pubdate= title.Pubdate,
                              PubId=title.PubId
                          }).ToListAsync();
        }

        // GET: api/Titles/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TitleDTO>> GetTitle(string id)
        {
          if (_context.Titles == null)
          {
              return NotFound();
          }
            var title = await _context.Titles.FindAsync(id);

            if (title == null)
            {
                return NotFound();
            }

            return new TitleDTO 
            { 
                TitleId = title.TitleId,
                Title1= title.Title1,
                Type= title.Type,
                Price= title.Price,
                Pubdate= title.Pubdate,
                PubId=title.PubId
            };
        }

        // PUT: api/Titles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTitle(string id, TitleDTO title)
        {
            if (id != title.TitleId)
            {
                return BadRequest();
            }

            _context.Entry(new Title 
            { 
                TitleId = title.TitleId,
                Title1 = title.Title1,
                Type= title.Type,
                Price= title.Price,
                Pubdate= title.Pubdate
            }).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitleExists(id))
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

        // POST: api/Titles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Title>> PostTitle(TitleDTO title)
        {
          if (_context.Titles == null)
          {
              return Problem("Entity set 'PubsContext.Titles'  is null.");
          }
            _context.Titles.Add(new Title 
            {
                TitleId = title.TitleId,
                Title1= title.Title1,
                Type= title.Type,
                Price= title.Price,
                Pubdate= title.Pubdate,
                PubId = title.PubId,
                Royalty = 0,
                YtdSales = 0,
                Notes="",
                Advance = 0
            });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TitleExists(title.TitleId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTitle", new { id = title.TitleId }, title);
        }

        // DELETE: api/Titles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTitle(string id)
        {
            if (_context.Titles == null)
            {
                return NotFound();
            }
            var title = await _context.Titles.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }

            _context.Titles.Remove(title);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        private bool TitleExists(string id)
        {
            return (_context.Titles?.Any(e => e.TitleId == id)).GetValueOrDefault();
        }
    }
}
