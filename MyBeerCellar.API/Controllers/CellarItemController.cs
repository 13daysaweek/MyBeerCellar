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
    public class CellarItemController : BaseApiController
    {
        private readonly MyBeerCellarContext _context;
        private readonly IMapper _mapper;

        public CellarItemController(MyBeerCellarContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        public async Task<IActionResult> Post(CreateCellarItem createCellarItemRequest)
        {
            IActionResult result = null;

            try
            {
                var cellarItem = _mapper.Map<CellarItem>(createCellarItemRequest);
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

        [HttpDelete("/api/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result = null;

            try
            {
                var item = await _context.CellarItems.FindAsync(id);

                if (item != null)
                {
                    _context.CellarItems.Remove(item);
                    await _context.SaveChangesAsync();
                    result = NoContent();
                }
                else
                {
                    result = NotFound();
                }
            }
            catch (Exception e)
            {
                result = BadRequest();
            }

            return result;
        }

        [HttpPut]
        [ProducesResponseType(typeof(CellarItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(UpdateCellarItem updateCellarItemRequest)
        {
            IActionResult result = null;

            try
            {
                var existingItem = await _context.CellarItems.FindAsync(updateCellarItemRequest.CellarItemId);

                if (existingItem != null)
                {
                    existingItem.DateModified = DateTime.UtcNow;
                    existingItem.BeerContainerId = updateCellarItemRequest.BeerContainerId;
                    existingItem.BeerStyleId = updateCellarItemRequest.BeerStyleId;
                    existingItem.ItemName = updateCellarItemRequest.ItemName;
                    existingItem.Quantity = updateCellarItemRequest.Quantity;
                    existingItem.YearProduced = updateCellarItemRequest.YearProduced;
                    await _context.SaveChangesAsync();

                    var updatedItem = await _context.CellarItems
                        .Include(_ => _.Style)
                        .Include(_ => _.Container)
                        .FirstAsync(_ => _.CellarItemId == updateCellarItemRequest.CellarItemId);

                    result = Ok(updatedItem);
                }
                else
                {
                    result = NotFound();
                }

            }
            catch (Exception e)
            {
                result = BadRequest();
            }

            return result;
        }
    }
}