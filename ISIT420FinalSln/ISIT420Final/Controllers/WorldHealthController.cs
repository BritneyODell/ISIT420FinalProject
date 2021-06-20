using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class WorldHealthController : ApiController
    {
        private string countryName = "United States";

        [HttpGet]
        public IHttpActionResult GetAllYears()
        {

            ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

            var songInfo = from song in myCollection.Songs
                           join album in myCollection.Albums on song.AlbumID equals album.AlbumID
                           where song.Explicit.Equals("True")
                                 || song.Explicit.Equals("False")
                           select new { album.ReleaseDate };

            // List of release dates for song albums
            var releaseDateList = songInfo.Select(s => s.ReleaseDate).Distinct().OrderByDescending(date => date).ToList();

            List<int> songYearList = new List<int>();

            // ReleaseDate from query needs to be parsed to extract ReleaseDate year value
            // to compare against the U.S. world health years
            foreach (var date in releaseDateList)
            {
                int year = DateTime.Parse(date.ToString()).Year;
                songYearList.Add(year);
            }

            // U.S. world health Year
            var usValue = from worldHealth in myCollection.WorldHealths
                          where worldHealth.CountryName == countryName
                          select new { worldHealth.Year};

            var usYearList = usValue.Select(s => s.Year).Distinct().OrderByDescending(date => date).ToList();

            List<int> whYearList = new List<int>();

            foreach (var year in usYearList)
            {
                int whyear = Convert.ToInt32(year);
                whYearList.Add(whyear);
            }

            var commonYears = songYearList.Intersect(whYearList).ToList();

            return Ok(commonYears);
        }
    }
}