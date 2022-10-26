using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountingJournal.Helpers;
using CountingJournal.Helpers.Text;
using CountingJournal.Model;
using Microsoft.UI.Xaml;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;
using static CountingJournal.MainWindow;
using static CountingJournal.Helpers.Text.Manual;
using SAP = Windows.Storage.AccessCache.StorageApplicationPermissions;

namespace CountingJournal.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<Message>? messages;


    [ObservableProperty]
    private ObservableCollection<Message>? countingMessages;

    [ObservableProperty]
    StorageFile? cSVFile = null;
    Settings? appConfig = null;

    [ObservableProperty]
    int latestCountNumber = 0;

    public HomeViewModel()
    {
        appConfig = App.GetService<Settings>();
        OnPropertyChanged(nameof(ShowCSVGather));
    }

    [RelayCommand]
    private async Task ListingMessages()
    {
        //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        //StorageFile storageFile = await storageFolder.GetFileAsync(filename);
        //var stream = System.IO.File.Open(storageFile.Path, FileMode.Open);
        //bool check = stream is FileStream;
        if (CSVFile is null)
            await GetCSVFile();

        var stream = File.Open(CSVFile.Path, FileMode.Open);
        if (stream is null)
            return;
        using var reader = new StreamReader(stream);
        using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<MessageMapper>();
        var items = csv.GetRecords<Message>();
        Messages = new ObservableCollection<Message>(items);
        //CountingMessages = new(Messages.Select(i => IsCounting.Decide(i)));
    }

    [RelayCommand]
    private async void Counting()
    {
        if (CountingMessages is null)
            CountingMessages = new();
        if (Messages is null)
            await ListingMessages();
        IsCounting.ResetLastSender();
        LatestCountNumber = 0;
        foreach (var msg in Messages)
        {
            bool decide = await IsCounting.Decide(LatestCountNumber, msg);
            if (decide)
            {
                System.Diagnostics.Debug.WriteLine($"Counting to number: {LatestCountNumber}");
                LatestCountNumber++;
                if (LatestCountNumber == 1980) //Missing number by Rews_red#9505
                    LatestCountNumber++;
                else if (LatestCountNumber == 2018) //Another missing number by Toon#9209
                    LatestCountNumber++;
                else if (LatestCountNumber == 2084) //Another missing number by Rews_red#9505
                    LatestCountNumber++;
                else if (LatestCountNumber == 2787) //Another missing number by Rews_red#9505
                    LatestCountNumber++;
                else if (LatestCountNumber == 4953) //Another missing number by Rews_red#9505
                    LatestCountNumber++;
                CountingMessages.Add(new Message() { Attachments = msg.Attachments, Content = LatestCountNumber.ToString(), SendAt = msg.SendAt, Sender = msg.Sender });
                //if (!CountingMessages.Contains(msg))
                //{
                //    CountingMessages.Insert(0, msg);
                //    continue;
                //}
            }
            else
            {
                if (IsNoise(msg))
                    continue;                
                else if (string.IsNullOrWhiteSpace(msg.Content))
                {
                    CountingMessages.Insert(0, msg);
                    break;
                }
                CountingMessages.Insert(0, msg);
                break;
            }
        }
        //Save to new CSV
        
    }

    public Visibility ShowCSVGather
    {
        get
        {
            if (appConfig is null)
                return Visibility.Visible;
            return string.IsNullOrEmpty(appConfig.CSVID)
            ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    [RelayCommand]
    private async Task GetCSVFile()
    {
        if (appConfig is not null &&
            !string.IsNullOrEmpty(appConfig.CSVID) &&
            !string.IsNullOrWhiteSpace(appConfig.CSVID))
        {
            CSVFile = await SAP.FutureAccessList.GetFileAsync(appConfig.CSVID);
        }

        var picker = new FileOpenPicker()
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            ViewMode = PickerViewMode.List
        };
        picker.FileTypeFilter.Add(".csv");
        var result = await picker.PickSingleFileAsync();
        if (result is StorageFile picked)
        {
            //CSVFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("data.csv");
            appConfig.CSVID = SAP.FutureAccessList.Add(picked);
            CSVFile = picked;
            OnPropertyChanged(nameof(ShowCSVGather));
        }
    }
}
