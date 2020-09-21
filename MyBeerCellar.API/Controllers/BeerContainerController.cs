using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Controllers
{
    public class BeerContainerController : BaseApiController
    {
        private readonly MyBeerCellarContext _context;

        public BeerContainerController(MyBeerCellarContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BeerContainer>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<BeerContainer>> GetAsync()
        {
            var containers = await _context.BeerContainer.ToListAsync();

            return containers;
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        [ProducesResponseTypeAttribute(typeof(BeerContainer), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            IActionResult result = null;

            var container = await _context.BeerContainer.FindAsync(id);

            if (container != null)
            {
                result = Ok(container);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}