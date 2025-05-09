using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Assets.Scripts
{
    public class AdversaryToJsonClassConverter
    {
        public List<Adversary> CreateAdversaryList()
        {
            // Path to the JSON file
            string jsonFilePath = @"Assets/Adversary.json";

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            // Read the JSON file
            string jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON into a list of Adversary objects
            var adversaryData = JsonConvert.DeserializeObject<AdversaryData>(jsonData, settings);

            foreach (var adversary in adversaryData.Adversaries)
            {
                adversary.Category = adversaryData.Categories.FirstOrDefault(c => c.CategoryId == adversary.CategoryId);
                adversary.Race = adversaryData.Races.FirstOrDefault(r => r.RaceId == adversary.RaceId);
                adversary.Story = adversaryData.Stories.FirstOrDefault(s => s.StoryId == adversary.StoryId);
            }

            return adversaryData.Adversaries;
        }
    }
}


