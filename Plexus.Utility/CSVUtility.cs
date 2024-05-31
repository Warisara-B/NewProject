using ServiceStack.Text;
using Microsoft.AspNetCore.Http;

namespace Plexus.Utility
{
    public static class CSVUtility
    {
        public static IEnumerable<IEnumerable<string>> ReadFile(IFormFile? file, bool isContainHeader)
        {
            if (file is null || !file.ContentType.Contains("text/csv"))
            {
                return Enumerable.Empty<IEnumerable<string>>();
            }
            
            var response = new List<IEnumerable<string>>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string? row;

                if (isContainHeader)
                {
                    row = reader.ReadLine();
                }

                while (!string.IsNullOrEmpty((row = reader.ReadLine())))
                {
                    response.Add(CsvReader.ParseLines(row));
                }
            }

            return response;
        }
    }
}