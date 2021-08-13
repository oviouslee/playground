using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Playground.Core.ApiQuery;
using Playground.Data;
using Playground.Data.Entities;
using Playground.Data.Extensions;

namespace Playground.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConflictController : Controller
    {
        private AppDbContext db;

        public ConflictController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(QueryResult<Conflict>), 200)]
        public async Task<IActionResult> QueryConflicts(
            [FromQuery]string page,
            [FromQuery]string pageSize,
            [FromQuery]string search,
            [FromQuery]string sort
        ) => Ok(await db.QueryConflicts(page, pageSize, search, sort));

        [HttpGet("[action]/{id}")]
        public async Task<Conflict> GetConflict([FromRoute]int id) => await db.GetConflict(id);

        [HttpPost("[action]")]
        public async Task AddConflict([FromBody]Conflict conflict) => await db.AddConflict(conflict);

        [HttpPost("[action]")]
        public async Task UpdateConflict([FromBody]Conflict conflict) => await db.UpdateConflict(conflict);

        [HttpPost("[action]")]
        public async Task RemoveConflict([FromBody]Conflict conflict) => await db.RemoveConflict(conflict);
    }
}