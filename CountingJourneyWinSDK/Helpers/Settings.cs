using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.UI;

namespace CountingJournal.Helpers;

public class Settings : ObservableObject
{
    #region Notification and Broadcaster	
    public ApplicationDataContainer Configs { get; } = ApplicationData.Current.LocalSettings;

    public T Get<T>(T defaultValue, [CallerMemberName] string? key = null)
    {
        if (!Configs.Values.ContainsKey(key))
        {
            Configs.Values.Add(key, defaultValue);
        }

        return (T)Configs.Values[key];
    }

    public bool Set<T>(T value, [CallerMemberName] string? key = null)
    {
        if (Configs.Values.TryGetValue(key, out var t))
        {
            if (t is null)
            {
                Configs.Values.Add(key, value);
                OnPropertyChanged(key);
                return true;
            }
            else if (t != null && Equals(t, value))
            {
                return false;
            }
        }

        Configs.Values[key] = value;
        OnPropertyChanged(key);
        return true;
    }

    //public List<T> GetList<T>(List<T> defaultValue, [CallerMemberName] string key = null)
    //{
    //	if (!Configs.Values.ContainsKey(key))
    //		Configs.Values.Add(key, defaultValue.ToJson());

    //	return Json.FromJson<List<T>>(Configs.Values[key].ToString());
    //}

    //public void SetList<T>(List<T> value, [CallerMemberName]string key = null)
    //{
    //	Configs.Values[key] = Json.ToJson(value);
    //}

    //public Color GetColor(Color defaultValue, [CallerMemberName]string key = null)
    //{
    //	if (!Configs.Values.ContainsKey(key))
    //		Configs.Values.Add(key, Json.ToJson(defaultValue));
    //	if (string.IsNullOrEmpty(Configs.Values[key].ToString()))
    //		return Colors.White;
    //	return Json.FromJson<Color>(Configs.Values[key].ToString());
    //}

    //public void SetColor(Color value, [CallerMemberName]string key = null)
    //{
    //	Configs.Values[key] = Json.ToJson(value);
    //}

    //public bool BroadcastSet<T>(T value, [CallerMemberName] string key = null)
    //{
    //	if (Set(value, key))
    //	{
    //		Messenger.Default.Send(new SettingsChangedMessage(key, value));
    //		return true;
    //	}
    //	return false;
    //}
    #endregion

    #region Singleton access
    //public static Settings Instance => Singleton<Settings>.Instance;
    #endregion

    public string CSVID
    {
        get => Get(string.Empty);
        set => Set(value);
    }
}