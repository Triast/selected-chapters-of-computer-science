using System;

namespace EntityFrameworkCore
{
    class StateNumber
    {
        static Random rand = new Random();

        string number;

        public StateNumber()
        {
            number = rand.Next(10000).ToString("d4") +
                rand.NextChar() + rand.NextChar() +
                "-" + rand.Next(1, 8);
        }

        public override string ToString()
        {
            return number;
        }
    }
}
