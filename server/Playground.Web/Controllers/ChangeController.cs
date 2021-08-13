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
    public class ChangeController : Controller
    {
        private AppDbContext db;

        public ChangeController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(QueryResult<Change>), 200)]
        public async Task<IActionResult> QueryChanges(
            [FromQuery]string page,
            [FromQuery]string pageSize,
            [FromQuery]string search,
            [FromQuery]string sort
        ) => Ok(await db.QueryChanges(page, pageSize, search, sort));

        [HttpGet("[action]/{id}")]
        public async Task<Change> GetChange([FromRoute]int id) => await db.GetChange(id);

        [HttpPost("[action]")]
        public async Task AddChange([FromBody]Change change) => await db.AddChange(change);

        [HttpPost("[action]")]
        public async Task UpdateChange([FromBody]Change change) => await db.UpdateChange(change);

        [HttpPost("[action]")]
        public async Task RemoveChange([FromBody]Change change) => await db.RemoveChange(change);
    }
}