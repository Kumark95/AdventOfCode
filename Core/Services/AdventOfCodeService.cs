using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Services;
internal sealed partial class AdventOfCodeService
{
    private readonly ILogger<AdventOfCodeService> _logger;
    private readonly HttpClient _httpClient;

    [GeneratedRegex("--- Day \\d{1,2}: (?<PuzzleName>[\\w ]+) ---")]
    private static partial Regex PuzzleNameRegex();

    public AdventOfCodeService(
        ILogger<AdventOfCodeService> logger,
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://adventofcode.com");
        var sessionCookie = configuration["SessionCookie"] ?? throw new ArgumentException("Cannot access AdventOfCode session cookie");

        _httpClient.DefaultRequestHeaders.Add("Cookie", sessionCookie);
    }

    public async Task<string> GetPuzzleName(int year, int day)
    {
        _logger.LogInformation("Requesting puzzle description");
        var htmlContent = await _httpClient.GetStringAsync($"/{year}/day/{day}");

        // Parse
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var titleNode = doc.DocumentNode.SelectSingleNode("//h2");
        var titleText = titleNode?.InnerText ?? throw new InvalidOperationException("Could not extract the puzzle name");

        // Extract the name
        var match = PuzzleNameRegex().Match(titleText);
        if (!match.Success)
        {
            throw new InvalidOperationException("Could not extract the puzzle name");
        }

        return match.Groups["PuzzleName"].Value;
    }

    public async Task<string> GetPuzzleInput(int year, int day)
    {
        _logger.LogInformation("Requesting puzzle input");
        return await _httpClient.GetStringAsync($"/{year}/day/{day}/input");
    }
}
