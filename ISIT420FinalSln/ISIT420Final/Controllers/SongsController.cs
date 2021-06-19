using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class SongsController : ApiController
    {
        ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

        [HttpGet]
        public IHttpActionResult GetAllSongTitles()
        {
            var allSongTitles = (from song in myCollection.Songs
                                 where song.Explicit.ToLower().Equals("true")
                                 || song.Explicit.ToLower().Equals("false")
                                 select new { song.SongTitle }).Distinct().OrderByDescending(song => song);

            return Ok(allSongTitles.ToList());
        }
    }
}