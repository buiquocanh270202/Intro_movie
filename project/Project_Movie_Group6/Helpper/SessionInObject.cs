using Newtonsoft.Json;
using Project_Movie_Group6.Models;
using System.Drawing;

namespace Project_Movie_Group6.Helpper
{
    public class SessionInObject
    {
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}

// Examble
//HttpContext.Session.SetObjectAsJson("account", pers);
//Person p = HttpContext.Session.GetObjectFromJson<Person>("account");
// p.properties