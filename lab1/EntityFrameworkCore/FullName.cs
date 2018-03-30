using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore
{
    class FullName
    {
        static Random rand = new Random();

        static string[] firstNames = { "Александр", "Алексей", "Андрей", "Антон", "Арсений", "Артём", "Василий", "Виктор", "Виталий", "Владимир", "Григорий" };
        static string[] lastNames = { "Курочка", "Петушок", "Соловьев", "Дроздов", "Соболев", "Соколов", "Сыч", "Голод", "Князь", "Чума", "Григорий" };
        static string[] middleNames = { "Артёмович", "Соловьёвич", "Левонович", "Львович", "Ничипорович", "Герыч", "Димонович", "Тирионович", "Андуинович", "Тралл", "Григорий" };

        int firstNameIndex;
        int lastNameIndex;
        int middleNameIndex;

        public FullName()
        {
            int dictionaryLength = firstNames.Length;

            firstNameIndex = rand.Next(dictionaryLength);
            lastNameIndex = rand.Next(dictionaryLength);
            middleNameIndex = rand.Next(dictionaryLength);
        }

        public override string ToString()
        {
            return $"{lastNames[lastNameIndex]} {firstNames[firstNameIndex]} {middleNames[middleNameIndex]}";
        }
    }
}
