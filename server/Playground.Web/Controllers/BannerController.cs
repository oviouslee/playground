using Microsoft.AspNetCore.Mvc;

using Playground.Core.Banner;

namespace Playground.Web.Controllers
{
    [Route("api/[controller]")]
    public class BannerController : Controller
    {
        private BannerConfig config;

        public BannerController(BannerConfig config) => this.config = config;

        [HttpGet("[action]")]
        public BannerConfig GetConfig() => config;
    }
}