using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
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
    }
}