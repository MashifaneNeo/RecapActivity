using System;

namespace HolidayShowdown.Models;

public readonly struct HolidayCount : IComparable<HolidayCount>, IEquatable<HolidayCount>
{
    public string CountryName { get; }
    public string CountryCode { get; }
    public int Year { get; }
    public int Count { get; }

    public HolidayCount(string countryName, string countryCode, int year, int count)
    {
        CountryName = countryName;
        CountryCode = countryCode;
        Year = year;
        Count = count;
    }

    public static bool operator >(HolidayCount a, HolidayCount b) => a.Count > b.Count;
    public static bool operator <(HolidayCount a, HolidayCount b) => a.Count < b.Count;
    public static bool operator ==(HolidayCount a, HolidayCount b) => a.Count == b.Count;
    public static bool operator !=(HolidayCount a, HolidayCount b) => a.Count != b.Count;

    public static HolidayCount operator +(HolidayCount a, HolidayCount b)
        => new($"{a.CountryName} + {b.CountryName}", $"{a.CountryCode}+{b.CountryCode}",
                a.Year, a.Count + b.Count);

    public int CompareTo(HolidayCount other) => Count.CompareTo(other.Count);

    public bool Equals(HolidayCount other) => this == other;

    public override bool Equals(object? obj) => obj is HolidayCount hc && Equals(hc);

    public override int GetHashCode() => HashCode.Combine(CountryCode, Year, Count);

    public override string ToString() => $"{CountryName} ({Year}): {Count} public holiday(s)";
}