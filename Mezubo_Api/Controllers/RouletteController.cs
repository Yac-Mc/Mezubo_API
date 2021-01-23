using Mezubo_Api.Business;
using Mezubo_Api.Core.Entities;
using Mezubo_Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mezubo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IMongoRepository<RouletteEntity> _rouletteGenericRepository;
        private readonly IBetsBL _betsBL;

        public RouletteController(IMongoRepository<RouletteEntity> rouletteGenericRepository, IBetsBL betsBL)
        {
            _rouletteGenericRepository = rouletteGenericRepository;
            _betsBL = betsBL;
        }

        [HttpPost("createroulette")]
        public async Task<ActionResult<GenericResponseEntity<string>>> Post()
        {
            RouletteEntity roulette = new RouletteEntity();
            return Ok(await _rouletteGenericRepository.InsertDocument(roulette));
        }

        [HttpPut("openroulette")]
        public async Task<ActionResult<GenericResponseEntity<string>>> Put(string id)
        {
            return Ok(await _betsBL.OpenBet(id));
        }

        [HttpPost("startbet/{userId}")]
        public async Task<ActionResult<GenericResponseEntity<string>>> StartBet(BetEntity rouletteBet)
        {
            return Ok(await _betsBL.StartBet(rouletteBet));
        }

        [HttpPost("closedbet")]
        public async Task<ActionResult<GenericResponseEntity<BetResultEntity>>> ClosedBet(string id)
        {
            return Ok(await _betsBL.ClosedBet(id));
        }

        [HttpGet("getallroulette")]
        public async Task<ActionResult<GenericResponseEntity<IEnumerable<RouletteEntity>>>> Get()
        {
            return Ok(await _rouletteGenericRepository.GetAll());
        }
    }
}
