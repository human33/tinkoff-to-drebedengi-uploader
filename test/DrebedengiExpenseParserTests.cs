using System;
using NUnit.Framework;

namespace T2DUploader.Test
{
    [TestFixture]
    public class DrebedengiExpenseParserTests
    {
        [Test]
        public void ParseHappyPath()
        {
            Expense expectedExpense = new(
                money: -100.09M,
                currency: "руб",
                category: "Тестовая категория",
                account: "Повседневные",
                date: new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "комментарий",
                user: null,
                expenseGroup: null
            );
            string csvLine = "-100.09;руб;\"Тестовая категория\";Повседневные;2015-01-01 00:00:00;\"комментарий\";";

            
            var parsedExpense = DrebedengiExpenseParser.Parse(csvLine);
            
            
            Assert.AreEqual(expectedExpense, parsedExpense);
        }
    }
}