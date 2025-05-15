using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts
{
    public class Adversary : IModelObject
    {
        [JsonProperty("adversary_id")]
        public int AdversaryId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("attribute_level")]
        public int AttributeLevel { get; set; }
        [JsonProperty("endurance")]
        public int Endurance { get; set; }
        [JsonProperty("might")]
        public int Might { get; set; }
        [JsonProperty("hate")]
        public int Hate { get; set; }
        [JsonProperty("resolve")]
        public int Resolve { get; set; }
        [JsonProperty("parry")]
        public string Parry { get; set; }
        [JsonProperty("armour")]
        public int Armour { get; set; }
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
        [JsonProperty("race_id")]
        public int RaceId { get; set; }
        [JsonProperty("story_id")]
        public int? StoryId { get; set; }
        [JsonProperty("fell_abilities")]
        public List<FellAbility> FellAbilities { get; set; }
        [JsonProperty("combat_proficiencies")]
        public List<CombatProficiency> CombatProficiencies { get; set; }
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        // Related data
        public Category Category { get; set; }
        public Race Race { get; set; }
        public Story Story { get; set; }
    }

    public class FellAbility
    {
        [JsonProperty("ability_name")]
        public string AbilityName { get; set; }
        [JsonProperty("ability_description")]
        public string AbilityDescription { get; set; }
    }

    public class CommonFellAbility
    {
        [JsonProperty("common_fell_ability_id")]
        public string CommonFellAbilityId { get; set; }
        [JsonProperty("common_fell_ability_name")]
        public string CommonFellAbilityName { get; set; }
        [JsonProperty("common_fell_ability_description")]
        public string CommonFellAbilityDescription { get; set; }
    }

    public class CombatProficiency
    {
        [JsonProperty("proficiency_name")]
        public string ProficiencyName { get; set; }
        [JsonProperty("proficiency_level")]
        public int ProficiencyLevel { get; set; }
        [JsonProperty("proficiency_details")]
        public string ProficiencyDetails { get; set; }
    }
    public class Category
    {
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
    }

    public class Race
    {
        [JsonProperty("race_id")]
        public int RaceId { get; set; }
        [JsonProperty("race_name")]
        public string RaceName { get; set; }
        [JsonProperty("common_fell_abilities")]
        public List<CommonFellAbility> CommonFellAbilities { get; set; }
    }

    public class Story
    {
        [JsonProperty("story_id")]
        public int StoryId { get; set; }
        [JsonProperty("story_name")]
        public string StoryName { get; set; }
        [JsonProperty("story_description")]
        public string StoryDescription { get; set; }
    }

    public class AdversaryData
    {
        [JsonProperty("adversaries")]
        public List<Adversary> Adversaries { get; set; }
        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }
        [JsonProperty("races")]
        public List<Race> Races { get; set; }
        [JsonProperty("stories")]
        public List<Story> Stories { get; set; }
    }
}