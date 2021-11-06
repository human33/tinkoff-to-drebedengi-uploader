using System;
using NUnit.Framework;
using T2DUploader;

namespace T2DUploader.Test
{
    [TestFixture]
    public class DrebedengiEpenseExporterTests
    {
        [Test]
        public void ExportAsCsvLineHappyPath()
        {
            Expense e = new Expense(
                money: 100.09M,
                currency: "RUB",
                category: "Категория",
                account: "Повседневные",
                date: new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "комментарий",
                user: null,
                expenseGroup: null
            );
            string expectedCsv = "";

            string csvLine = DrebedengiExpenseExporter.ExportAsCsvLine(e);
            Assert.AreEqual(expectedCsv, csvLine);
        }
    }
}