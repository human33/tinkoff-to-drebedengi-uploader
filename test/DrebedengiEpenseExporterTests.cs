using System;
using NUnit.Framework;
using T2DUploader;
using T2DUploader.Model;

namespace T2DUploader.Test
{
    [TestFixture]
    public class DrebedengiEpenseExporterTests
    {
        [Test]
        public void ExportAsCsvLineHappyPath()
        {
            Expense e = new Expense(
                money: -100.09M,
                currency: "руб",
                category: "Тестовая категория",
                account: "Повседневные",
                date: new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "комментарий",
                user: null,
                expenseGroup: null
            );
            string expectedCsv = "-100.09;руб;\"Тестовая категория\";Повседневные;2015-01-01 00:00:00;\"комментарий\";";

            string csvLine = DrebedengiExpenseExporter.ExportAsCsvLine(e);
            Assert.AreEqual(expectedCsv, csvLine);
        }
    }
}