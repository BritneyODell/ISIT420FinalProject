using System;
using System.Linq;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class PositiveEffectController : ApiController
    {
        private string countryName = "United States";

        [HttpGet]
        public IHttpActionResult GetPositiveEffect(int year, string song)
        {
            try
            {
                ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

                var songInfo = from album in myCollection.Albums
                               from songs in myCollection.Songs
                               where songs.SongTitle == song
                               && songs.AlbumID == album.AlbumID
                               select new { album.ReleaseDate, songs.Danceability };

                var albumYear = DateTime.Parse(songInfo.FirstOrDefault().ReleaseDate.ToString()).Year;

                // Positive Effect Of Country Health value
                var peValue = from worldHealth in myCollection.WorldHealths
                              where worldHealth.CountryName == countryName
                              && worldHealth.Year == year
                              && albumYear == worldHealth.Year
                              select new { worldHealth.Year, worldHealth.PositiveEffect };

                // PositiveEffect for U.S is above .7
                // no need to consider this value for conditions
                if (albumYear == peValue.FirstOrDefault().Year)
                {
                    if (songInfo.FirstOrDefault().Danceability >= .5)
                    {
                        return Ok(new { Status = "high", Year = year, PeValue = peValue.FirstOrDefault().PositiveEffect, SongTitle = song });
                    }
                }

                // low song danceability
                return Ok(new { Status = "slim", Year = year, PeValue = peValue.FirstOrDefault().PositiveEffect, SongTitle = song });
            }
            catch (Exception)
            {
                // No data returned from queries
                return Ok(new { Status = "no data" });
            }
        }
    }
}