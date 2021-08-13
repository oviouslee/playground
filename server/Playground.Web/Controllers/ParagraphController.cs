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
    public class ParagraphController : Controller
    {
        private AppDbContext db;

        public ParagraphController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(QueryResult<Paragraph>), 200)]
        public async Task<IActionResult> QueryParagraphs(
            [FromQuery]string page,
            [FromQuery]string pageSize,
            [FromQuery]string search,
            [FromQuery]string sort
        ) => Ok(await db.QueryParagraphs(page, pageSize, search, sort));

        [HttpGet("[action]/{id}")]
        public async Task<Paragraph> GetParagraph([FromRoute]int id) => await db.GetParagraph(id);

        [HttpPost("[action]")]
        public async Task AddParagraph([FromBody]Paragraph paragraph) => await db.AddParagraph(paragraph);

        [HttpPost("[action]")]
        public async Task UpdateParagraph([FromBody]Paragraph paragraph) => await db.UpdateParagraph(paragraph);

        [HttpPost("[action]")]
        public async Task RemoveParagraph([FromBody]Paragraph paragraph) => await db.RemoveParagraph(paragraph);
    }
}