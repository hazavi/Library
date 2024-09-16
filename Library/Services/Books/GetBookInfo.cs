using Newtonsoft.Json;

namespace Library.Services.Books
{
    public class GetBookInfo
    {
        private readonly HttpClient _httpClient;

        public GetBookInfo(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookInfoItems>> GetBookInfoAsync(int bookId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://hapi-books.p.rapidapi.com/book/{bookId}"),
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
                    var books = JsonConvert.DeserializeObject<List<BookInfoItems>>(body);

                    // Ensure that each book has a non-null Authors list
                    if (books != null)
                    {
                        foreach (var book in books)
                        {
                            book.Authors ??= new List<string>(); // Initialize if null
                        }
                    }

                    return books ?? new List<BookInfoItems>(); // Return an empty list if null
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"Error fetching books: {ex.Message}");
                return new List<BookInfoItems>(); // Return an empty list on error
            }
        }
    }

    public class BookInfoItems
    {
        [JsonProperty("book_id")]
        public int BookId { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public string Url { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        public double Rating { get; set; }

        public int Pages { get; set; }

        [JsonProperty("published_date")]
        public string Published_Date { get; set; }

        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }
    }
}
