using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ProducesResponseType(typeof(IEnumerable<BeerStyle>), StatusCodes.Status200OK)]
    public class BeerStyleController : ControllerBase
    {
        private readonly MyBeerCellarContext _context;

        public BeerStyleController(MyBeerCellarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<BeerStyle>> Get()
        {
            var styles = await _context.BeerStyles.ToListAsync();

            return styles;
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        [ProducesResponseType(typeof(BeerStyle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            IActionResult result = null;
            var style = await _context.BeerStyles.FindAsync(id);

            if (style != null)
            {
                result = new OkObjectResult(style);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}