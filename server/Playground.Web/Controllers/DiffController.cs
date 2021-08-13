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
    public class DiffController : Controller
    {
        private AppDbContext db;

        public DiffController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(QueryResult<Diff>), 200)]
        public async Task<IActionResult> QueryDiffs(
            [FromQuery]string page,
            [FromQuery]string pageSize,
            [FromQuery]string search,
            [FromQuery]string sort
        ) => Ok(await db.QueryDiffs(page, pageSize, search, sort));

        [HttpGet("[action]/{id}")]
        public async Task<Diff> GetDiff([FromRoute]int id) => await db.GetDiff(id);

        [HttpPost("[action]")]
        public async Task AddDiff([FromBody]Diff diff) => await db.AddDiff(diff);

        [HttpPost("[action]")]
        public async Task UpdateDiff([FromBody]Diff diff) => await db.UpdateDiff(diff);

        [HttpPost("[action]")]
        public async Task RemoveDiff([FromBody]Diff diff) => await db.RemoveDiff(diff);
    }
}