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

        public async Task<BookInfoItems> GetBookInfoAsync(int bookId)
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

                    var book = JsonConvert.DeserializeObject<BookInfoItems>(body); // Deserialize to a single object

                    // Ensure that the book has a non-null Authors list
                    if (book != null)
                    {
                        book.Authors ??= new List<string>(); // Initialize if null
                    }

                    return book;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"Error fetching book: {ex.Message}");
                return null; // Return null on error
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
