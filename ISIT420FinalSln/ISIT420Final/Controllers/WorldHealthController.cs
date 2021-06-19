using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class WorldHealthController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAllYears()
        {
            ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

            var allYears = (from worldHealth in myCollection.WorldHealths
                            select new { worldHealth.Year })
                                    .Distinct()
                                        .OrderByDescending(year => year);

            return Ok(allYears.ToList());
        }

        
    }
}