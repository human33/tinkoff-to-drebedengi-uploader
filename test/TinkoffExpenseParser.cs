using System;
using NUnit.Framework;

namespace T2DUploader.Test
{
    [TestFixture]
    public class TinkoffExpenseParserTests
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
            
            string csvLine = "01.01.2015 00:00:00;-100,09;RUB;Тестовая категория;комментарий;Повседневные";

            
            var parsedExpense = TinkoffExpenseParser.Parse(csvLine);
            
            
            Assert.AreEqual(expectedExpense, parsedExpense);
        }
    }
}