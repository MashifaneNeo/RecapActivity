using System.Linq;
using System.Windows.Controls;
using HolidayShowdown.Models;

namespace HolidayShowdown.Views;

public partial class HistoryView : UserControl
{
    public HistoryView(RankedList<HolidayCount> history, RankedList<HolidayCount> favourites)
    {
        InitializeComponent();

        var ranked = history.Ranked
            .Select((h, i) => new { Rank = i + 1, h.CountryName, h.Year, h.Count })
            .ToList();

        HistoryListView.ItemsSource = ranked;
        FavouritesListView.ItemsSource = favourites.Ranked;
    }
}