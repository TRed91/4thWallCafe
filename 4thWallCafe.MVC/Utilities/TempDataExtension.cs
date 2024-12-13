using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace _4thWallCafe.MVC.Utilities;

/// <summary>
/// Extends the MVC TempData type to take an object and pass it to the view
/// </summary>
public static class TempDataExtension
{
    public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        object o;
        tempData.TryGetValue(key, out o);
        return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
    }
}

/// <summary>
/// Takes a bool to specify wether a message represents success or fail and the message.
/// </summary>
public class TempDataMessage
{
    public bool Success { get; private set; }
    public string Message { get; private set; }

    public TempDataMessage(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}