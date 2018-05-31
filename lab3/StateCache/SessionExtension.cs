using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StateCache.Models;

namespace StateCache
{
    public static class SessionExtension
    {
        public static void SetInspector(this ISession session, string key, Inspector value) => 
            session.SetString(key, JsonConvert.SerializeObject(value));

        public static Inspector GetInspector(this ISession session, string key)
        {
            var inspector = session.GetString(key);

            return inspector != null ?
                JsonConvert.DeserializeObject<Inspector>(inspector) : null;
        }

        public static void SetState(this ISession session, string key, StateFilter value) => 
            session.SetString(key, JsonConvert.SerializeObject(value));

        public static StateFilter GetState(this ISession session, string key)
        {
            var state = session.GetString(key);

            return state != null ?
                JsonConvert.DeserializeObject<StateFilter>(state) : null;
        }
    }
}
