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
using MSG = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using CountingJournal.Model.Messages;
using System.ComponentModel;
using System.Text.Json;
using System.Text;
using System;

namespace CountingJournal.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<MessageViewModel>? messages;

    [ObservableProperty]
    private ObservableCollection<MessageViewModel>? countingMessages;

    [ObservableProperty]
    StorageFile? cSVFile = null;
    Settings? appConfig = null;

    [ObservableProperty]
    int latestCountNumber = 0;

    public HomeViewModel()
    {
        appConfig = App.GetService<Settings>();
        OnPropertyChanged(nameof(ShowCSVGather));
        MSG.Default.Register<HomeViewModel, ConfirmThisAsFillerMessage, string>(this, Token.ConfirmFillerMSGToken,
            (home, fillerMSG) => AddOrRemoveConfirmedFillers(fillerMSG));
        ConfirmedFillers = new();
        //Load filler list
        DirectoryInfo dir = new(@"D:\UserData\Desktop\Counting-20221026-1800\Fillers");
        if (!dir.Exists)
            dir.Create();
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var json = File.ReadAllText(file.FullName);
            var deserialized = JsonSerializer.Deserialize<Message?>(json);
            if (deserialized is null)
            {
                continue;
            }
            ConfirmedFillers.Add(deserialized);
        }
    }

    [ObservableProperty]
    ObservableCollection<Message> confirmedFillers = new();

    private void AddOrRemoveConfirmedFillers(ConfirmThisAsFillerMessage fillerMSG)
    {
        if (fillerMSG.ConfirmAsFiller)
        {
            //Add to filler list
            ConfirmedFillers.Add(fillerMSG.ConfirmFiller);
            //Save to json
            DirectoryInfo folder = new(@"D:\UserData\Desktop\Counting-20221026-1800\Fillers");
            if (!folder.Exists)
                folder.Create();
            string fileName = $"{fillerMSG.ConfirmFiller.SendAt.Ticks}.json";
            File.WriteAllTextAsync(Path.Join(folder.FullName, fileName), JsonSerializer.Serialize<Message>(fillerMSG.ConfirmFiller, 
                new JsonSerializerOptions() 
                { 
                    WriteIndented = true 
                }));
        }
        else
        {
            if (!ConfirmedFillers.Contains(fillerMSG.ConfirmFiller))
            {
                return;
            }
            ConfirmedFillers.Remove(fillerMSG.ConfirmFiller);
            string fileName = $"{fillerMSG.ConfirmFiller.SendAt.Ticks}.json";
            DirectoryInfo folder = new(@"D:\UserData\Desktop\Counting-20221026-1800\Fillers");
            if (!folder.Exists)
                return;
            FileInfo savedFiller = new(Path.Join(folder.FullName, fileName));
            if (!savedFiller.Exists)
                return;
            savedFiller.Delete();
        }
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
        var items = csv.GetRecords<Message>().Select(msg => new MessageViewModel(msg, ConfirmedFillers.Any(confirm => msg.SendAt.Ticks == confirm.SendAt.Ticks)));
        Messages = new ObservableCollection<MessageViewModel>(items);
        //CountingMessages = new(Messages.Select(i => IsCounting.Decide(i)));
    }

    [RelayCommand]
    private async void Counting()
    {
        if (CountingMessages is null)
            CountingMessages = new();
        if (Messages is null)
            await ListingMessages();
        else
        {
            Messages.Clear();
            await ListingMessages();
        }
        IsCounting.ResetLastSender();
        LatestCountNumber = 0;
        CountingMessages.Clear();
        MessageViewModel? previousAnchor = null;
        int acceptableGapSize = 2;
        foreach (var msg in Messages)
        {
            if (msg.ConfirmedFiller)
                continue;
            var previous = string.Empty;
            if (previousAnchor is null)
            {
                previous = "0";
            }
            else
            {
                previous = previousAnchor.AsNumber > 0 ?
                    previousAnchor.AsNumber.ToString() :
                    previousAnchor.Content;
            }

            var next = msg.Content;

            bool shouldSwitchAnchor = false;

            //Attempted flags
            bool triedFilterOnlyNumber = false;
            bool presumeAllNumberSpread = false;
            bool triedConvertFromThaiNum = false;
            bool triedYamokConvert = false;

        retry:
            shouldSwitchAnchor = IsCountingDeciderV2.ShouldAddIn(previous, next, acceptableGapSize);
            if (shouldSwitchAnchor)
            {
                acceptableGapSize = 2;
                msg.IsFiller = false;
                msg.AsNumber = int.Parse(next);
                previousAnchor = msg;
                continue;
            }
            else
                next = msg.Content;
            //Is it contains text?
            //2384 fillers: no changes
            //1363 fillers: filter to number only
            //911 fillers: presume mixed with other number
            //1223 fillers: fix bug on presumeNumberSpread allow distinct one false pass through as number
            //1146 fillers: convert thai number to arabic
            //1128 fillers: convert repeater symbol to normal number
            acceptableGapSize++;


            if (!triedFilterOnlyNumber)
            {
                next = IsCountingDeciderV2.PerhapsTheNumberIsMixedWithText(msg);
                triedFilterOnlyNumber = true;
                goto retry;
            }
            if (!presumeAllNumberSpread)
            {
                string testing = IsCountingDeciderV2.PerhapsTheNumberIsMixedWithTextAndNumber(msg, previous);
                next = !string.IsNullOrEmpty(testing) ? testing : msg.Content;
                presumeAllNumberSpread = true;
                goto retry;
            }
            if (!triedConvertFromThaiNum)
            {
                next = IsCountingDeciderV2.PerhapsThereIsThaiNumber(msg);
                triedConvertFromThaiNum = true;
                goto retry;
            }
            if (!triedYamokConvert)
            {
                next = RepeaterSymbol.Translate(next, int.Parse(previous) + 1);
                triedYamokConvert = true;
                goto retry;
            }

            //Finally give up
            msg.ExpectNumber = int.Parse(previous) + 1;
            msg.IsFiller = true;
        }
        CountingMessages = new(Messages.Where(msg => Consider(msg)));
        TotalFiller = Messages.Where(msg => msg.IsFiller).Count();
    }

    private bool Consider(MessageViewModel msg)
    {
        if (ShowFillers && msg.IsFiller)
            return true;
        else if (ShowConfirmed && !msg.IsFiller)
            return true;
        return false;
    }

    [ObservableProperty]
    bool showFillers = true;

    [ObservableProperty]
    bool showConfirmed = true;

    [ObservableProperty]
    int totalFiller = -1;



    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMessageSelected))]
    [NotifyCanExecuteChangedFor(nameof(SkipToNextFillerCommand))]
    [NotifyCanExecuteChangedFor(nameof(SkipToNextFillerNotCommand))]
    int selectedMessage = -1;

    public bool IsMessageSelected
        => SelectedMessage >= 0;

    [RelayCommand(CanExecute = nameof(IsMessageSelected))]
    private void SkipToNextFiller()
    {
        try
        {
            var next = SelectedMessage;
            do
            {
                next++;
            }
            while (!CountingMessages[next].IsFiller);
            SelectedMessage = next;
        }
        catch { }
    }

    [RelayCommand(CanExecute = nameof(IsMessageSelected))]
    private void SkipToNextFillerNot()
    {
        try
        {
            var next = SelectedMessage;
            do
            {
                next++;
            }
            while (CountingMessages[next].IsFiller);
            SelectedMessage = next;
        }
        catch { }
    }

    [RelayCommand]
    private void JumpToLatestConfirmedFiller()
    {
        var lastConfirmed = CountingMessages.Last(msg => msg.ConfirmedFiller);
        if (lastConfirmed is null)
            return;
        SelectedMessage = CountingMessages.IndexOf(lastConfirmed);
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJumpThroughFiller))]
    [NotifyPropertyChangedFor(nameof(IsJumpThroughNumber))]
    [NotifyPropertyChangedFor(nameof(IsJumpThroughConfirmedFiller))]
    private JumpMode selectedJump = JumpMode.Filler;

    public bool IsJumpThroughFiller => SelectedJump == JumpMode.Filler;
    public bool IsJumpThroughNumber => SelectedJump == JumpMode.Message;
    public bool IsJumpThroughConfirmedFiller => SelectedJump == JumpMode.ConfirmedFiller;

    [RelayCommand]
    private void JumpPrevious()
    {
        if (SelectedMessage < 0)
            return;
        try
        {
            var next = SelectedMessage;
            do
            {
                next--;
                if (next < 0)
                {
                    SelectedMessage = -1;
                    return;
                }
            }
            while (ConditionStillMet(next));

            SelectedMessage = next;
        }
        catch { }
    }

    [RelayCommand]
    private void JumpNext()
    {
        if (SelectedMessage >= CountingMessages.Count - 1)
            return;
        try
        {
            var next = SelectedMessage;
            do
            {
                next++;
                if (next >= CountingMessages.Count)
                {
                    SelectedMessage = CountingMessages.Count - 1;
                    return;
                }
            }
            while (ConditionStillMet(next));

            SelectedMessage = next;
        }
        catch { }
    }

    public bool ConditionStillMet(int index)
    {
        try
        {
            switch (SelectedJump)
            {
                case JumpMode.Filler:
                    return !CountingMessages[index].IsFiller;
                case JumpMode.Message:
                    return CountingMessages[index].IsFiller;
                case JumpMode.ConfirmedFiller:
                    return !CountingMessages[index].ConfirmedFiller;
            }
        }
        catch {  }
        return false;
    }

    [RelayCommand]
    private void FirstOfJump()
    {
        switch (SelectedJump)
        {
            case JumpMode.Filler:
                var firstFiller = CountingMessages.FirstOrDefault(msg => msg.IsFiller);
                if (firstFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstFiller);
                break;
            case JumpMode.Message:
                var firstCount = CountingMessages.FirstOrDefault(msg => !msg.IsFiller);
                if (firstCount is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstCount);
                break;
            case JumpMode.ConfirmedFiller:
                var firstVerifiedFiller = CountingMessages.FirstOrDefault(msg => msg.ConfirmedFiller);
                if (firstVerifiedFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstVerifiedFiller);
                break;
        }
    }

    [RelayCommand]
    private void LastOfJump()
    {
        switch (SelectedJump)
        {
            case JumpMode.Filler:
                var firstFiller = CountingMessages.LastOrDefault(msg => msg.IsFiller);
                if (firstFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstFiller);
                break;
            case JumpMode.Message:
                var firstCount = CountingMessages.LastOrDefault(msg => !msg.IsFiller);
                if (firstCount is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstCount);
                break;
            case JumpMode.ConfirmedFiller:
                var firstVerifiedFiller = CountingMessages.LastOrDefault(msg => msg.ConfirmedFiller);
                if (firstVerifiedFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstVerifiedFiller);
                break;
        }
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
            return;
        }

        StorageFile file = await StorageFile.GetFileFromPathAsync(@"D:\UserData\Desktop\Counting-20221026-1800\countTo10k.csv");
        appConfig.CSVID = SAP.FutureAccessList.Add(file);
        CSVFile = file;
    }
}

public enum JumpMode
{
    Filler,
    Message,
    ConfirmedFiller
}
