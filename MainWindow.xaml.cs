using System.Windows;
using HolidayShowdown.Models;
using HolidayShowdown.Services;
using HolidayShowdown.Views;

namespace HolidayShowdown;

public partial class MainWindow : Window
{
    private readonly NagerDateService _service = new();
    private readonly RankedList<HolidayCount> _history = new();
    private readonly RankedList<HolidayCount> _favourites = new();

    public MainWindow()
    {
        InitializeComponent();
        ShowCompare();
    }

    private void ShowCompare()
    {
        var compareView = new CompareView(_service);
        compareView.CompareRequested += OnCompareCompleted;
        MainContent.Content = compareView;
    }

    private void OnCompareCompleted(HolidayCount left, HolidayCount right)
    {
        var resultView = new ResultView(left, right);
        resultView.SavedToHistory += (winnerOrTie) =>
        {
            _history.Add(left);
            _history.Add(right);
        };
        resultView.FavouriteAdded += (fav) => _favourites.Add(fav);
        resultView.BackToCompareRequested += (_, _) => ShowCompare();
        MainContent.Content = resultView;
    }

    private void GoCompare_Click(object sender, RoutedEventArgs e) => ShowCompare();

    private void GoHistory_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new HistoryView(_history, _favourites);
    }
}