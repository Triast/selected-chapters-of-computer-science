using System;

namespace EntityFrameworkCore
{
    class TechnicalPassport
    {
        static Random rand = new Random();

        string passport;

        public TechnicalPassport()
        {
            passport = rand.NextChar() + rand.NextChar() + rand.Next() +
                rand.Next(1000000).ToString("d6");
        }

        public override string ToString()
        {
            return passport;
        }
    }
}
