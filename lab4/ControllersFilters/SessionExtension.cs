using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;

namespace ControllersFilters
{
    public static class SessionExtension
    {
        public static bool TryGet<T>(this ISession session, out T filter)
        {
            filter = default(T);

            if (!session.Keys.Contains(typeof(T).Name))
            {
                return false;
            }

            filter = session.Get<T>();
            
            return true;
        }
        
        public static void Set<T>(this ISession session, T filter) =>
            session.SetString(typeof(T).Name, JsonConvert.SerializeObject(filter));

        public static T Get<T>(this ISession session) =>
            JsonConvert.DeserializeObject<T>(session.GetString(typeof(T).Name));
    }
}
