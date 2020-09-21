using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BeerStyleController : ControllerBase
    {
        private readonly MyBeerCellarContext _context;
        private readonly IMapper _mapper;

        public BeerStyleController(MyBeerCellarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BeerStyle>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<BeerStyle>> GetAsync()
        {
            var styles = await _context.BeerStyles.ToListAsync();

            return styles;
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        [ProducesResponseType(typeof(BeerStyle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
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

        [HttpPost]
        [ProducesResponseType(typeof(BeerStyle), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateBeerStyle beerStyle)
        {
            IActionResult result = null;

            try
            {
                var itemToAdd = _mapper.Map<BeerStyle>(beerStyle);
                await _context.BeerStyles.AddAsync(itemToAdd);
                await _context.SaveChangesAsync();
                result = CreatedAtAction(nameof(GetByIdAsync), new { id = itemToAdd.StyleId }, itemToAdd);
            }
            catch (Exception e)
            {
                result = BadRequest();
            }
            
            return result;
        }

        [HttpDelete]
        [Route("/api/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            IActionResult result = null;
            var style = await _context.BeerStyles.FindAsync(id);

            if (style != null)
            {
                _context.BeerStyles.Remove(style);
                await _context.SaveChangesAsync();
                result = NoContent();
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpPut]
        [ProducesResponseType(typeof(BeerStyle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(UpdateBeerStyle beerStyle)
        {
            IActionResult result = null;

            var style = await _context.BeerStyles.FindAsync(beerStyle.StyleId);

            if (style != null)
            {
                style.StyleName = beerStyle.StyleName;
                style.DateModified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                result = Ok(style);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}