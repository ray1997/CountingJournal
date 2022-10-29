using CountingJournal.Model;
using CountingJournal.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CountingJournal.Views;

[CommunityToolkit.Mvvm.ComponentModel.INotifyPropertyChanged]
public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }

    private void SetJumpMode(object sender, RoutedEventArgs e)
    {
        if (sender is RadioMenuFlyoutItem item)
        {
            switch (item.Tag.ToString())
            {
                case "filler": ViewModel.SelectedJump = JumpMode.Filler; break;
                case "number": ViewModel.SelectedJump = JumpMode.Message; break;
                case "confirm": ViewModel.SelectedJump = JumpMode.ConfirmedFiller; break;
            }
        }
    }
}

public class MessageTemplate : DataTemplateSelector
{
    public DataTemplate PlainText
    {
        get;set;
    }

    public DataTemplate WithPics
    {
        get;set;
    }
    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is not Message || item is not MessageViewModel)
            return base.SelectTemplate(item);
        if (item is MessageViewModel msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Attachments))
                return WithPics;
            return PlainText;
        }
        return base.SelectTemplate(item);
    }
}
