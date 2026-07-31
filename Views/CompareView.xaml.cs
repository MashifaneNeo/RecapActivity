using System;
using System.Windows;
using System.Windows.Controls;
using HolidayShowdown.Models;
using HolidayShowdown.Services;

namespace HolidayShowdown.Views;

public partial class CompareView : UserControl
{
    private readonly NagerDateService _service;

    public event Action<HolidayCount, HolidayCount>? CompareRequested;

    public CompareView(NagerDateService service)
    {
        InitializeComponent();
        _service = service;
        Loaded += CompareView_Loaded;
    }

    private async void CompareView_Loaded(object sender, RoutedEventArgs e)
    {
        for (int y = DateTime.Now.Year - 1; y <= DateTime.Now.Year + 1; y++)
            YearBox.Items.Add(y);
        YearBox.SelectedItem = DateTime.Now.Year;

        StatusText.Text = "Loading country list...";
        var result = await _service.GetAvailableCountriesAsync();

        if (!result.Success || result.Data is null)
        {
            StatusText.Text = result.Message;
            return;
        }

        CountryABox.ItemsSource = result.Data;
        CountryBBox.ItemsSource = result.Data;
        StatusText.Text = "";
    }

    private async void CompareButton_Click(object sender, RoutedEventArgs e)
    {
        if (CountryABox.SelectedItem is not NagerDateService.CountryOption a ||
            CountryBBox.SelectedItem is not NagerDateService.CountryOption b ||
            YearBox.SelectedItem is not int year)
        {
            StatusText.Text = "Please select both countries and a year.";
            return;
        }

        CompareButton.IsEnabled = false;
        StatusText.Text = "Fetching live data...";

        var resultA = await _service.GetHolidayCountAsync(a.Name, a.Code, year);
        var resultB = await _service.GetHolidayCountAsync(b.Name, b.Code, year);

        CompareButton.IsEnabled = true;

        if (!resultA.Success) { StatusText.Text = resultA.Message; return; }
        if (!resultB.Success) { StatusText.Text = resultB.Message; return; }

        StatusText.Text = "";
        CompareRequested?.Invoke(resultA.Data!, resultB.Data!);
    }
}