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
    [Route("/api/[controller]")]
    public class BeerContainerController : BaseApiController
    {
        private readonly MyBeerCellarContext _context;
        private readonly IMapper _mapper;

        public BeerContainerController(MyBeerCellarContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BeerContainer>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<BeerContainer>> GetAsync()
        {
            var containers = await _context.BeerContainers.ToListAsync();

            return containers;
        }

        [HttpGet("/api/[controller]/{id}", Name = "Get")]
        [ProducesResponseTypeAttribute(typeof(BeerContainer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            IActionResult result = null;

            var container = await _context.BeerContainers.FindAsync(id);

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
        public async Task<IActionResult> PostAsync(CreateBeerContainer container)
        {
            IActionResult result = null;

            try
            {
                var containerToCreate = _mapper.Map<BeerContainer>(container);
                await _context.BeerContainers.AddAsync(containerToCreate);
                await _context.SaveChangesAsync();
                result = CreatedAtAction("Get", new {id = containerToCreate.BeerContainerId}, containerToCreate);
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

            var containerToDelete = await _context.BeerContainers.FindAsync(id);

            if (containerToDelete != null)
            {
                _context.BeerContainers.Remove(containerToDelete);
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
        [ProducesResponseType(typeof(BeerContainer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(UpdateBeerContainer container)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                var containerToUpdate = await _context.BeerContainers.FindAsync(container.BeerContainerId);

                if (containerToUpdate != null)
                {
                    containerToUpdate.ContainerType = container.ContainerType;
                    containerToUpdate.DateModified = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    result = Ok(container);
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