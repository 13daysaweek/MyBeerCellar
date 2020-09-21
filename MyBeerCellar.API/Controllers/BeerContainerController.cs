using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Azure.Core;
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

        [HttpPost]
        [ProducesResponseType(typeof(BeerContainer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(BeerContainer container)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.BeerContainer.AddAsync(container);
                    await _context.SaveChangesAsync();
                    result = CreatedAtAction(nameof(GetByIdAsync), new {id = container.BeerContainerId}, container);
                }
                catch (Exception e)
                {
                    result = BadRequest();
                }
            }
            else
            {
                result = BadRequest();
            }

            return result;
        }

        [HttpDelete]
        [Route("/api/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            IActionResult result = null;

            var containerToDelete = await _context.BeerContainer.FindAsync(id);

            if (containerToDelete != null)
            {
                _context.BeerContainer.Remove(containerToDelete);
                await _context.SaveChangesAsync();
                result = Accepted();
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpPut]
        [ProducesResponseType(typeof(BeerContainer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(BeerContainer container)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                var containerToUpdate = await _context.BeerContainer.FindAsync(container.BeerContainerId);

                if (containerToUpdate != null)
                {
                    containerToUpdate.ContainerType = container.ContainerType;
                    containerToUpdate.DateModified = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    result = NotFound();
                }
            }
            else
            {
                result = BadRequest();
            }

            return result;
        }
    }
}