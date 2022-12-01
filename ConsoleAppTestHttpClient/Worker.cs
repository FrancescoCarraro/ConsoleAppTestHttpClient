using ConsoleAppTestHttpClient.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ConsoleAppTestHttpClient
{
    public class Worker
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Worker> logger;

        public Worker(IConfiguration configuration,
                      ILogger<Worker> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task DoWorkAsync()
        {
            var url = configuration.GetSection("AppSettings:Url").Value;
            if (string.IsNullOrEmpty(url))
            {
                logger.LogWarning($"{nameof(DoCallAsync)} - URL is not defined!");
                return;
            }

            while(true)
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < 20; i++)
                {
                    tasks.Add(DoCallAsync(url));
                }

                await Task.WhenAll(tasks);

                await Task.Delay(1000);
            }
        }

        public async Task<List<UserModel>> DoCallAsync(string url)
        {
            List<UserModel>? users = null;

            bool configureAwait = false;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(new Uri(url)).ConfigureAwait(configureAwait))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(configureAwait);
                            //logger.LogInformation($"responseContent: {responseContent}");
                            if (!string.IsNullOrEmpty(responseContent))
                                users = JsonConvert.DeserializeObject<List<UserModel>>(responseContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(DoCallAsync)} - ex: {ex.Message}");
            }

            return users;
        }
    }
}
