using System;
using System.Linq;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class PerceptionsOfCorruptionController : ApiController
    {
        ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

        private string countryName = "United States";

        [HttpGet]
        public IHttpActionResult GetPerceptionsOfCorruption(int year, string song)
        {
            try
            {
                var songInfo = from album in myCollection.Albums
                               from songs in myCollection.Songs
                               where songs.SongTitle == song
                               && songs.AlbumID == album.AlbumID
                               select new { album.ReleaseDate, songs.Explicit };

                var albumYear = DateTime.Parse(songInfo.FirstOrDefault().ReleaseDate.ToString()).Year;

                // Perceptions of corruption value
                var pcValue = from worldHealth in myCollection.WorldHealths
                              where worldHealth.CountryName == countryName
                              && worldHealth.Year == year
                              && albumYear == worldHealth.Year
                              select new { worldHealth.Year, worldHealth.PerceptionsOfCorruption };

                // Perceptions of Corruption for U.S is above .6
                // no need to consider this value for conditions
                if (albumYear == pcValue.FirstOrDefault().Year)
                {
                    if (songInfo.FirstOrDefault().Explicit.ToLower().Equals("true"))
                    {
                        return Ok(new { Status = "high", Year = year, PocValue = pcValue.FirstOrDefault().PerceptionsOfCorruption, SongTitle = song });
                    }
                }

                return Ok(new { Status = "slim", Year = year, PocValue = pcValue.FirstOrDefault().PerceptionsOfCorruption, SongTitle = song });
            }
            catch (Exception)
            {
                // No data returned from queries
                return Ok(new { Status = "no data" });
            }
        }
    }
}