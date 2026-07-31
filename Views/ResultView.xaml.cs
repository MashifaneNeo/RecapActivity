using System;
using System.Windows;
using System.Windows.Controls;
using HolidayShowdown.Models;

namespace HolidayShowdown.Views;

public partial class ResultView : UserControl
{
    private readonly HolidayCount _left;
    private readonly HolidayCount _right;
    private HolidayCount _winner;

    public event Action<string>? SavedToHistory;
    public event Action<HolidayCount>? FavouriteAdded;
    public event EventHandler? BackToCompareRequested;

    public ResultView(HolidayCount left, HolidayCount right)
    {
        InitializeComponent();
        _left = left;
        _right = right;

        LeftText.Text = _left.ToString();
        RightText.Text = _right.ToString();

        if (_left > _right)
        {
            _winner = _left;
            VerdictText.Text = $"{_left.CountryName} wins!";
            VerdictText.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["WinColor"];
        }
        else if (_right > _left)
        {
            _winner = _right;
            VerdictText.Text = $"{_right.CountryName} wins!";
            VerdictText.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["WinColor"];
        }
        else if (_left == _right)
        {
            _winner = _left;
            VerdictText.Text = "It's a tie!";
            VerdictText.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TieColor"];
        }

        var combined = _left + _right; 
        CombinedText.Text = $"Combined total: {combined.Count} public holidays between both countries.";

        SavedToHistory?.Invoke(_winner.CountryName);
    }

    private void Favourite_Click(object sender, RoutedEventArgs e) => FavouriteAdded?.Invoke(_winner);

    private void Back_Click(object sender, RoutedEventArgs e) => BackToCompareRequested?.Invoke(this, EventArgs.Empty);
}