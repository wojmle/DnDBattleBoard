using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts;
using Newtonsoft.Json;
using UnityEngine;

public class PromptsGenerator : MonoBehaviour
{
    // Keywords to identify real weapons (Polish + English)
    readonly string[] KnownWeapons = {
        "sword", "axe", "bow", "spear", "dagger", "mace", "club", "staff",
        "hammer", "whip", "crossbow", "shield", "blade", "halberd", "scythe",
        "flail", "polearm", "morning star", "javelin", "saber", "cleaver", "scimitar", "knife",
        
        "pike", "mattock", "rake",

        "miecz", "topór", "³uk", "w³ócznia", "sztylet", "bu³awa", "maczuga",
        "kij", "m³ot", "bat", "kusza", "tarcza", "ostrze", "halabarda", "kosa",
        "cep", "drzewiec", "gwiazda poranna", "oszczep", "szabla", "tasak", "nó¿",

        "pa³ka"
    };

    bool IsLikelyWeapon(string name)
    {
        name = name.ToLower();
        return KnownWeapons.Any(k => name.Contains(k));
    }

    string GetWeaponLocation(string weaponName)
    {
        if (weaponName == null) return "side";
        weaponName = weaponName.ToLower();
        return (weaponName.Contains("bow") || weaponName.Contains("³uk") ||
                weaponName.Contains("spear") || weaponName.Contains("w³ócznia") ||
                weaponName.Contains("halberd") || weaponName.Contains("oszczep"))
            ? "back" : "side";
    }

    void Start()
    {
        string outputCsvPath = "Midjourney_Prompts.csv";

        var converter = new AdversaryToJsonClassConverter();
        var adversaries = converter.CreateAdversaryList();
        var combatProficiencies = adversaries.Where(x => x.CombatProficiencies != null)
            .SelectMany(x => x.CombatProficiencies).Select(cp => cp.ProficiencyName).Distinct().ToList();
        var notWeapons = combatProficiencies.Where(x => !IsLikelyWeapon(x));
        var sb = new StringBuilder();
        sb.AppendLine("Adversary ID;Name;Race;Prompt");

        foreach (var adv in adversaries)
        {
            if (adv == null || adv.Name == null || adv.Race.RaceName == null) continue;

            // Filter valid weapons
            var validWeapons = adv.CombatProficiencies?
                .Where(w => IsLikelyWeapon(w.ProficiencyName))
                .ToList() ?? new List<CombatProficiency>();

            string wielded = validWeapons.Count > 0 ? validWeapons[0].ProficiencyName : "no visible weapon";
            string secondary = validWeapons.Count > 1 ? validWeapons[1].ProficiencyName : "";

            string weaponText = $"wielding a {wielded}";
            if (!string.IsNullOrWhiteSpace(secondary))
            {
                string location = GetWeaponLocation(secondary);
                weaponText += $" with a {secondary} strapped to their {location}";
            }

            /*
            string prompt = $"{adv.Name}, a character of the {adv.Race.RaceName} race from Tolkien's Middle-earth, " +
                            $"full body, standing in neutral pose, facing forward, realistic proportions, " +
                            $"{armorText}{weaponText}, highly detailed, realistic fantasy illustration, " +
                            "white background, no shadows, no scene, suitable for 3D model creation";*/

            string prompt = $"Create a high-quality, full-body portrait of character named {adv.Name}, who is a {adv.Race.RaceName} from Tolkien's Middle-earth, " +
                            $"{adv.Description}, {weaponText}." +
                            "The image should have no background (transparent), no shadows, and be suitable for use as a 3D model reference. " +
                            "Please provide the image as a PNG with a transparent background if possible.";

            string line = $"{adv.AdversaryId};\"{adv.Name}\";\"{adv.Race.RaceName}\";\"{prompt.Replace("\"", "\"\"")}\"";
            sb.AppendLine(line);
        }

        File.WriteAllText(outputCsvPath, sb.ToString(), Encoding.UTF8);
        Console.WriteLine($"CSV exported: {outputCsvPath}");
    }
}
