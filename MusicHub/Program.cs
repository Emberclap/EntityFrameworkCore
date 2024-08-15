using Microsoft.EntityFrameworkCore;
using MusicHub.Data;
using MusicHub.Data.Models;
using System.Text;

namespace MusicHub
{
    public class Program
    {
        static void Main()
        {
            var context = new MusicHubDbContext();
            Console.WriteLine(ExportAlbumsInfo(context, 9));
        }
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(p => p.Id == producerId)
                .Select(p => new
                {
                    p.Name,
                    p.ReleaseDate,
                    p.Producer,
                    songs = p.Songs
                        .Select(s => new
                        {
                           s.Name,
                           s.Price,
                           s.Writer
                        })
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.Producer}");
                sb.AppendLine($"-Songs:");

                int counter = 0;

                foreach (var song in album.songs)
                {
                    sb.AppendLine($"---#{counter+1}");
                    sb.AppendLine($"---SongName:{song.Name}");
                    sb.AppendLine($"---Price:{song.Price:f2}");
                    sb.AppendLine($"---Writer:{song.Writer}");
                }

            }
            return sb.ToString().TrimEnd();
        }
    }
}
