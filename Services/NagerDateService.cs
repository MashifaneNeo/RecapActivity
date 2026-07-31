using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HolidayShowdown.Models;

namespace HolidayShowdown.Services;

public class NagerDateService
{
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("https://date.nager.at/api/v3/")
    };

    public record CountryOption(string Name, string Code);

    public async Task<OperationResult<List<CountryOption>>> GetAvailableCountriesAsync()
    {
        try
        {
            using var stream = await Client.GetStreamAsync("AvailableCountries");
            var raw = await JsonSerializer.DeserializeAsync<List<RawCountry>>(stream);

            if (raw is null || raw.Count == 0)
                return OperationResult<List<CountryOption>>.Fail("No countries were returned by the server.");

            var options = raw
                .Select(c => new CountryOption(c.name, c.countryCode))
                .OrderBy(c => c.Name)
                .ToList();

            return OperationResult<List<CountryOption>>.Ok(options);
        }
        catch (HttpRequestException)
        {
            return OperationResult<List<CountryOption>>.Fail("Could not reach date.nager.at. Check your internet connection.");
        }
        catch (Exception ex)
        {
            return OperationResult<List<CountryOption>>.Fail($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<OperationResult<HolidayCount>> GetHolidayCountAsync(string countryName, string countryCode, int year)
    {
        try
        {
            using var stream = await Client.GetStreamAsync($"PublicHolidays/{year}/{countryCode}");
            var holidays = await JsonSerializer.DeserializeAsync<List<RawHoliday>>(stream);

            if (holidays is null)
                return OperationResult<HolidayCount>.Fail($"No holiday data returned for {countryName}.");

            var count = new HolidayCount(countryName, countryCode, year, holidays.Count);
            return OperationResult<HolidayCount>.Ok(count);
        }
        catch (HttpRequestException)
        {
            return OperationResult<HolidayCount>.Fail("Could not reach date.nager.at. Check your internet connection.");
        }
        catch (JsonException)
        {
            return OperationResult<HolidayCount>.Fail($"Received an unreadable response for {countryName}.");
        }
        catch (Exception ex)
        {
            return OperationResult<HolidayCount>.Fail($"Unexpected error: {ex.Message}");
        }
    }

    private class RawCountry
    {
        public string countryCode { get; set; } = "";
        public string name { get; set; } = "";
    }

    private class RawHoliday
    {
        public string? date { get; set; }
        public string? localName { get; set; }
    }
}