using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using T2DUploader.Model;
using T2DUploader.Services;
using T2DUploader.Services.ExpenseMapper;
using T2DUploader.Utility;

namespace T2DUploader.Test
{
    [TestFixture]
    public class UploaderTests
    {
        [Test]
        public async Task UploadChecksForDuplicatesWithoutFalsePositive()
        {
            Expense expense1 = new(
                money: -100.09M,
                currency: "руб",
                category: "Тестовая категория",
                account: "Повседневные",
                date: new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "комментарий",
                user: null,
                expenseGroup: null
            );
            
            // setup drebedengi file
            var db = new Moq.Mock<IFileInfo>();
            byte[] drebedengiDump = System.Text.Encoding.UTF8.GetBytes(
                "-101.09;руб;\"Тестовая категория1\";Повседневные;2015-01-01 00:00:00;\"комментарий1\";\n"+
                "-202.09;руб;\"Тестовая категория2\";Повседневные;2015-01-02 00:00:00;\"комментарий2\";\n"+
                "-300.09;руб;\"Тестовая категория3\";Повседневные;2015-01-03 01:00:00;\"комментарий3\";\n"
            );
            db.Setup(f => f.OpenRead())
                .Returns(new MemoryStream(drebedengiDump));
            
            UploaderOptions options = new()
            {
                DrebedengiDump = db.Object,
                OutputFilePath = null
            };
            
            
            // setup ui mock
            var ui = new Moq.Mock<IUserInterface>();
            ui.Setup(i => 
                    i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()))
                .Returns((Expense e1, Expense e2) =>
                {
                    return Task.FromResult(true);
                });
            

            Uploader uploader = new(options, ui.Object);
            
            await uploader.Upload(expense1);
            
            ui.Verify(i => 
                i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()), Times.Never);
        }
        
        [Test]
        public async Task UploadChecksForDuplicatesWithoutFalseNegative()
        {
            Expense expense1 = new(
                money: -101.09M,
                currency: "руб",
                category: "Тестовая категория",
                account: "Повседневные",
                date: new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "комментарий",
                user: null,
                expenseGroup: null
            );
            
            // setup drebedengi file
            var db = new Moq.Mock<IFileInfo>();
            byte[] drebedengiDump = System.Text.Encoding.UTF8.GetBytes(
                "-101.09;руб;\"Тестовая категория1\";Повседневные;2015-01-01 00:00:00;\"комментарий1\";\n"+
                "-202.09;руб;\"Тестовая категория2\";Повседневные;2015-01-02 00:00:00;\"комментарий2\";\n"+
                "-300.09;руб;\"Тестовая категория3\";Повседневные;2015-01-03 01:00:00;\"комментарий3\";\n"
            );
            db.Setup(f => f.OpenRead())
                .Returns(new MemoryStream(drebedengiDump));
            
            UploaderOptions options = new()
            {
                DrebedengiDump = db.Object,
                OutputFilePath = null
            };
            
            
            // setup ui mock
            var ui = new Moq.Mock<IUserInterface>();
            bool wasCalledForFirstExpense = false;
            ui.Setup(i => 
                    i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()))
                .Returns((Expense e1, Expense e2) =>
                {
                    if ((e1.Money + 101.09M) <= 0.0001M)
                    {
                        wasCalledForFirstExpense = true;
                    }
                    
                    return Task.FromResult(true);
                });
            

            Uploader uploader = new(options, ui.Object);
            
            await uploader.Upload(expense1);
            
            ui.Verify(i => 
                i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()), Times.Once);
            Assert.True(wasCalledForFirstExpense);
        }
        
    }
}