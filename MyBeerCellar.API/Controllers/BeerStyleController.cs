using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ProducesResponseType(typeof(IEnumerable<BeerStyle>), StatusCodes.Status200OK)]
    public class BeerStyleController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<BeerStyle>> Get()
        {
            return await Task.FromResult(new List<BeerStyle>
            {
                new BeerStyle
                {
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    StyleId = 1,
                    StyleName = "Imperial Stout"
                },
                new BeerStyle
                {
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    StyleId = 2,
                    StyleName = "NE IPA"
                }
            }.AsEnumerable());
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        [ProducesResponseType(typeof(BeerStyle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            IActionResult result = null;

            if (id == 1)
            {
                result = Ok(new BeerStyle
                {
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    StyleId = 1,
                    StyleName = "Imperial Stout"
                });
            }
            else
            {
                result = NotFound();
            }

            return await Task.FromResult(result);
        }
    }
}