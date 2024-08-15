using MusicHub.Data;
using MusicHub.Initializer;
using System.Text;

namespace MusicHub
{
    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(p => p.ProducerId == producerId)
                .Select(p => new
                {
                    p.Name,
                    p.ReleaseDate,
                    p.Producer,
                    price = p.Price,
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

            foreach (var album in albums.OrderByDescending(p => p.price))
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.Producer.Name}");
                sb.AppendLine($"-Songs:");

                int counter = 0;

                foreach (var song in album.songs.OrderByDescending(s => s.Name).ThenBy(s => s.Writer))
                {
                    sb.AppendLine($"---#{counter + 1}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer.Name}");
                    counter++;
                }
                sb.AppendLine($"-AlbumPrice: {album.price:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {

            var span = new TimeSpan(0, 0, duration);
            var songs = context.Songs
               .Where(s => s.Duration > span)
               .Select(s => new
               {
                   s.Name,
                   performers = s.SongPerformers
                    .Select(p => $"{p.Performer.FirstName} {p.Performer.LastName}")
                    .ToList(),
                   writer = s.Writer.Name,
                   albumProducer = s.Album.Producer.Name,
                   duration = s.Duration.ToString("c"),
               })
               .OrderBy(s => s.Name)
               .ThenBy(p => p.writer)
               .ToList();

            var sb = new StringBuilder();
            int counter = 0;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{counter + 1}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.writer}");
                if (song.performers.Any())
                {
                    foreach (var performer in song.performers.OrderBy(p => p))
                    {
                        sb.AppendLine($"---Performer: {performer}");
                    }
                }
                sb.AppendLine($"---AlbumProducer: {song.albumProducer}");
                sb.AppendLine($"---Duration: {song.duration}");
                counter++;
            }
            return sb.ToString().TrimEnd();
        }
    }
}
