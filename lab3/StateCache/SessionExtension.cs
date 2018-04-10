using Microsoft.AspNetCore.Http;
using StateCache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateCache
{
    public static class SessionExtension
    {
        public static void SetInspector(this ISession session, string key, Inspector value)
        {
            throw new NotImplementedException();
        }

        public static Inspector GetInspector(this ISession session, string key)
        {
            throw new NotImplementedException();
        }

        public static void SetCarTechState(this ISession session, string key, CarTechState value)
        {
            throw new NotImplementedException();
        }

        public static CarTechState GetCarTechState(this ISession session, string key)
        {
            throw new NotImplementedException();
        }
    }
}
