using System;
using NUnit.Framework;
using T2DUploader.Model;

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
                currency: "Руб",
                category: "Тестовая категория",
                account: null,
                date: new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "комментарий",
                user: null,
                expenseGroup: null
            );
            
            string csvLine = "\"01.01.2015 00:00:00\";\"01.01.2015\";\"*1111\";\"OK\";\"-100\";\"RUB\";\"-100,09\";\"RUB\";\"\";\"Тестовая категория\";\"\";\"комментарий\";\"0,00\";\"0,00\";\"400,00\"";

            TinkoffExpenseParser.ParseResult parseResult = TinkoffExpenseParser.Parse(csvLine);
            
            Assert.AreEqual(TinkoffExpenseParser.ParseStatus.OK, parseResult.Status);
            Assert.AreEqual(expectedExpense, parseResult.Expense!);
        }
    }
}