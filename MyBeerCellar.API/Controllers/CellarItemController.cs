using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Controllers
{
    public class CellarItemController : BaseApiController
    {
        private readonly MyBeerCellarContext _context;

        public CellarItemController(MyBeerCellarContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CellarItem>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CellarItem>> GetAsync()
        {
            var items = await _context.CellarItems
                .Include(_ => _.Style)
                .Include(_ => _.Container)
                .ToListAsync();

            return items;
        }

        [HttpGet("/api/[controller]/{id}")]
        [ProducesResponseType(typeof(CellarItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            IActionResult result = null;

            var item = await _context.CellarItems
                .Include(_ => _.Container)
                .Include(_ => _.Style)
                .FirstOrDefaultAsync(_ => _.CellarItemId == id);

            if (item != null)
            {
                result = Ok(item);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CellarItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CellarItem cellarItem)
        {
            IActionResult result = null;

            try
            {
                cellarItem.Container = null;
                cellarItem.Style = null;
                await _context.CellarItems.AddAsync(cellarItem);
                await _context.SaveChangesAsync();
                result = CreatedAtAction(nameof(GetById), new {id = cellarItem.CellarItemId}, cellarItem);
            }
            catch (Exception e)
            {
                result = BadRequest();
            }

            return result;
        }
    }
}