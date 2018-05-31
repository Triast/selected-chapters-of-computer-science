using System;

namespace EntityFrameworkCore
{
    class AlphaDigitNumber
    {
        static Random rand = new Random();

        string number = "";

        public AlphaDigitNumber()
        {
            for (int i = 0; i < 4; i++)
            {
                number += rand.NextChar();
            }

            number += rand.Next(10000).ToString("d4");
        }

        public override string ToString()
        {
            return number;
        }
    }
}
