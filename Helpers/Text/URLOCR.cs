using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;

namespace CountingJournal.Helpers.Text;
public static class URLOCR
{
    public static List<Cached> cached = null;

    public static async Task<string> GetTexts(string url)
    {
        if (cached is null)
            await Initialize();

        //Check if it's cached
        if (cached.FirstOrDefault(i => i.URL == url) is Cached item)
        {
            System.Diagnostics.Debug.WriteLine($"URL {url}:\r\n" +
                $"Has a text: {item.Result}");
            return item.Result;
        }

        HttpClient client = new();
        var response = await client.GetAsync(url);
        var stream = await response.Content.ReadAsStreamAsync();

        IRandomAccessStream random = stream.AsRandomAccessStream();
        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(random);
        SoftwareBitmap image = await decoder.GetSoftwareBitmapAsync();

        OcrEngine engine = OcrEngine.TryCreateFromUserProfileLanguages();

        var result = await engine.RecognizeAsync(image);

        if (cached.FirstOrDefault(i => i.URL == url) is null)
        {
            cached.Add(new(url, result.Text));
            SaveResult();
        }
        return result.Text;
    }

    public static async Task Initialize()
    {
        var tryget = await ApplicationData.Current.LocalFolder.TryGetItemAsync("ocr.csv");
        if (tryget is null)
        {
            cached = new();
            return;
        }
        StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("ocr.csv");
        var stream = await file.OpenStreamForReadAsync();

        using var reader = new StreamReader(stream);
        using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
        cached = new List<Cached>(csv.GetRecords<Cached>());
    }

    private static Debouncer _dc;
    public static void SaveResult()
    {
        if (_dc is null)
        {
            _dc = new Debouncer();            
        }
        _dc.Debounce(1000, async () =>
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("ocr.csv", CreationCollisionOption.OpenIfExists);
            using var writer = new StreamWriter(await file.OpenStreamForWriteAsync());
            using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(cached);
        });
    }
}

public class Cached
{
    [CsvHelper.Configuration.Attributes.Name("URL")]
    public string URL
    {
        get;set;
    }

    [CsvHelper.Configuration.Attributes.Name("Result")]
    public string Result
    {
        get;set;
    }
    public Cached(string uRL, string result)
    {
        URL = uRL;
        Result = result;
    }

    public Cached() { }
}