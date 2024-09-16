using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Library.Services.Books
{
    public class SearchBook
    {
        private readonly HttpClient _httpClient;

        public SearchBook(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://hapi-books.p.rapidapi.com/search/{query}"),
                Headers =
                {
                    { "x-rapidapi-key", "683e536ef5mshe8358cfc575d40ep1198a9jsnfc23a843a67c" }, // Replace with your API key
                    { "x-rapidapi-host", "hapi-books.p.rapidapi.com" },
                },
            };

            try
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var books = JsonConvert.DeserializeObject<List<Book>>(body);

                    // Ensure that each book has a non-null Authors list
                    if (books != null)
                    {
                        foreach (var book in books)
                        {
                            book.Authors ??= new List<string>(); // Initialize if null
                        }
                    }

                    return books ?? new List<Book>(); // Return an empty list if null
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"Error fetching books: {ex.Message}");
                return new List<Book>(); // Return an empty list on error
            }
        }
    }

    public class Book
    {
        [JsonProperty("book_id")]
        public int BookId { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public string Url { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        public double Rating { get; set; }

        public int CreatedEditions { get; set; }

        public int Year { get; set; }
    }
}
