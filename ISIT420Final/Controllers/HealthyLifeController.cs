using System;
using System.Linq;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class HealthyLifeController : ApiController
    {
        private string countryName = "United States";

        [HttpGet]
        public IHttpActionResult GetHealthyLife(int year, string song)
        {
            try
            {
                ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

                var songInfo = from album in myCollection.Albums
                               from songs in myCollection.Songs
                               where songs.SongTitle == song
                               && songs.AlbumID == album.AlbumID
                               select new { album.ReleaseDate, songs.Energy };

                var albumYear = DateTime.Parse(songInfo.FirstOrDefault().ReleaseDate.ToString()).Year;

                // Healthy Life Expectancy At Birth value
                var hlValue = from worldHealth in myCollection.WorldHealths
                              where worldHealth.CountryName == countryName
                              && worldHealth.Year == year
                              && albumYear == worldHealth.Year
                              select new { worldHealth.Year, worldHealth.HealthyLifeExpectancyAtBirth };

                // HealthyLifeExpectancyAtBirth for U.S is above 68
                // no need to consider this value for conditions
                if (albumYear == hlValue.FirstOrDefault().Year)
                {
                    if (songInfo.FirstOrDefault().Energy >= .5)
                    {
                        return Ok(new { Status = "high", Year = year, HlValue = hlValue.FirstOrDefault().HealthyLifeExpectancyAtBirth, SongTitle = song });
                    }
                }

                // low song energy
                return Ok(new { Status = "slim", Year = year, HlValue = hlValue.FirstOrDefault().HealthyLifeExpectancyAtBirth, SongTitle = song });
            }
            catch (Exception)
            {
                // No data returned from queries
                return Ok(new { Status = "no data" });
            }
        }

    }
}