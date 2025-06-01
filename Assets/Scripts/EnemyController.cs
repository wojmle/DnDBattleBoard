using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private HealthBarUI healthBar;
    public Adversary adversary;
    private string characterRace;
    private string bieglosciBojowe;
    private string mroczneAtrybuty;
    private string combatProficiencies = "BIEG£OŒCI BOJOWE";
    private string fellAbilities = "MROCZNE ATRYBUTY:";
    public bool isEnemy;
    private Dictionary<string, int> characterStats = new Dictionary<string, int>();    // Predefined list of keys (traits or attributes for example)

    // Start is called before the first frame update
    void Start()
    {
        characterRace = adversary.Race.RaceName;
        bieglosciBojowe = FormatCombatProficiencies(adversary);
        mroczneAtrybuty = FormatFellAbilities(adversary);
        isEnemy = true;
        characterStats = new Dictionary<string, int>
        {            
            { nameof(Adversary.AttributeLevel), adversary.AttributeLevel },           
            { nameof(Adversary.Endurance), adversary.Endurance },
            { nameof(Adversary.Might), adversary.Might },
            { nameof(Adversary.Hate), adversary.Hate },
            { nameof(Adversary.Parry), 0 },
            { nameof(Adversary.Armour), adversary.Armour }
        };
    }

    public void AddPoint(string key)
    {
        if (characterStats.ContainsKey(key))
        {
            characterStats[key] += 1;
            if (key == nameof(Adversary.Endurance) && healthBar != null)
                healthBar.SetHealth(characterStats[key]);
        }
    }

    public void RemovePoint(string key)
    {
        if (characterStats.ContainsKey(key))
        {
            characterStats[key] -= 1;
            if (key == nameof(Adversary.Endurance) && healthBar != null)
                healthBar.SetHealth(characterStats[key]);
        }
    }

    public string GetCharacterRace()
    {
        return characterRace; 
    }

    public string GetDarkAttributes()
    {
        return mroczneAtrybuty;
    }

    public string GetCombatPerks()
    {
        return bieglosciBojowe;
    }

    public Dictionary<string,int> GetCharacterStats()
    {
        return characterStats;
    }

    public void SetHealthBar(HealthBarUI bar)
    {
        healthBar = bar;
    }

    public void RemoveHealthBar()
    {
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
            healthBar = null;
        }
    }

    public string FormatCombatProficiencies(Adversary adversary)
    {
        if (adversary.CombatProficiencies == null || adversary.CombatProficiencies.Count == 0)
            return $"<color=#C6493D><font=\"Oswald Bold SDF\"><size=12>{combatProficiencies}</size></font></color>";

        // Use a StringBuilder for efficient string concatenation
        System.Text.StringBuilder formattedString = new System.Text.StringBuilder();

        // Add the header with red color, Oswald Bold SDF font, and size 12
        formattedString.AppendLine($"<color=#C6493D><font=\"Oswald Bold SDF\"><size=12>{combatProficiencies}</size></font></color>");
        bool isFirst = true;

        foreach (var proficiency in adversary.CombatProficiencies)
        {
            if (!isFirst)
            {
                // Add a new line for subsequent proficiencies
                formattedString.AppendLine();
            }

            string proficiencyText = $"<b>{proficiency.ProficiencyName}</b>: {proficiency.ProficiencyLevel} ({proficiency.ProficiencyDetails})";
            var keywords = new List<string> { "Sword" };
            proficiencyText = AddLinksToKeywords(proficiencyText, keywords); // Add links to keywords
            formattedString.Append(proficiencyText);

            isFirst = false;
        }

        return formattedString.ToString();
    }

    public string FormatFellAbilities(Adversary adversary)
    {
        if (adversary.FellAbilities == null || adversary.FellAbilities.Count == 0)
            return $"$<color=#C6493D><font=\"Oswald Bold SDF\"><size=12>{fellAbilities}</size></font></color>";

        // Use a StringBuilder for efficient string concatenation
        System.Text.StringBuilder formattedString = new System.Text.StringBuilder();

        // Add the header with red color, Oswald Bold SDF font, and size 12
        formattedString.Append($"<color=#C6493D><font=\"Oswald Bold SDF\"><size=12>{fellAbilities}</size></font></color> ");

        bool isFirst = true;
        foreach (var ability in adversary.FellAbilities)
        {
            if (!isFirst)
            {
                // Add a new line for subsequent abilities
                formattedString.AppendLine();
            }

            // Format each ability with TextMesh Pro rich text tags
            formattedString.Append($"<b>{ability.AbilityName}</b>: {ability.AbilityDescription}");
            isFirst = false;
        }

        return formattedString.ToString();
    }

    public string AddLinksToKeywords(string inputText, List<string> keywords)
    {
        if (string.IsNullOrEmpty(inputText) || keywords == null || keywords.Count == 0)
            return inputText;

        // Iterate through the keywords and wrap them with the <link> tag
        foreach (var keyword in keywords)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                // Use Regex to replace the keyword with a <link> tag
                inputText = System.Text.RegularExpressions.Regex.Replace(
                    inputText,
                    $@"\b{System.Text.RegularExpressions.Regex.Escape(keyword)}\b", // Match whole words only
                    $"<link={keyword}><u>{keyword}</u></link>", // Wrap the keyword with a link and underline it
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase // Case-insensitive matching
                );
            }
        }

        return inputText;
    }
}
