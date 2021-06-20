using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ISIT420Final.Controllers
{
    public class SongsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllSongTitlesForYear(int year)
        {
            ISIT420FinalEntitiesConnection myCollection = new ISIT420FinalEntitiesConnection();

            var songInfo = from song in myCollection.Songs
                           join album in myCollection.Albums on song.AlbumID equals album.AlbumID
                           where song.Explicit.Equals("True")
                                 || song.Explicit.Equals("False")
                           select new { album.ReleaseDate, song.SongTitle };

            List<SongDetails> songDetailList = new List<SongDetails>();

            foreach (var song in songInfo.ToList())
            {
                // ReleaseDate from query needs to be parsed to extract ReleaseDate year value
                // to compare against the year passed in from API call
                int newSongYear = DateTime.Parse(song.ReleaseDate.ToString()).Year;

                if (newSongYear == year)
                {
                    var newSongDetail = new SongDetails
                    {
                        SongYear = newSongYear,
                        SongTitle = song.SongTitle
                    };

                    songDetailList.Add(newSongDetail);
                }
            }

            var songListForYear = songDetailList.Where(s => s.SongYear == year).Select(s => s.SongTitle).OrderBy(songTitle => songTitle);

            return Ok(songListForYear);
        }
    }

    class SongDetails
    {
        public int SongYear { get; set; }
        public string SongTitle { get; set; }
    }
}
