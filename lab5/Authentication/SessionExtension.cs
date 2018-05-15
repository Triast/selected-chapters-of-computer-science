using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;

namespace Authentication
{
    public static class SessionExtension
    {
        public static bool TryGet<T>(this ISession session, out T filter, string key)
        {
            filter = default(T);

            if (!session.Keys.Contains(key))
            {
                return false;
            }

            filter = session.Get<T>(key);
            
            return true;
        }
        
        public static void Set<T>(this ISession session, T filter, string key) =>
            session.SetString(key, JsonConvert.SerializeObject(filter));

        public static T Get<T>(this ISession session, string key) =>
            JsonConvert.DeserializeObject<T>(session.GetString(key));
    }
}
