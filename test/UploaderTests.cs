using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using T2DUploader.Utility;

namespace T2DUploader.Test
{    
    [TestFixture]
    public class UploaderTests
    {
        [Test]
        public async Task ChecksExpensesInDrebedengiWithoutFalsePositives()
        {
            // setup tinkoff file
            var td = new Moq.Mock<IFileInfo>();
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var tEncoding = System.Text.Encoding.GetEncoding("windows-1251");
            byte[] tinkoffDump = tEncoding.GetBytes(
                "01.01.2015 00:00:00;-100,09;RUB;Тестовая категория1;комментарий1;Повседневные\n"+
                "02.01.2015 00:00:00;-200,09;RUB;Тестовая категория2;комментарий2;Повседневные\n"+
                "03.01.2015 00:00:00;-300,09;RUB;Тестовая категория3;комментарий3;Повседневные"
            );
            td.Setup(f => f.OpenRead())
                .Returns(new MemoryStream(tinkoffDump));
            
            // setup drebedengi file
            var db = new Moq.Mock<IFileInfo>();
            byte[] drebedengiDump = tEncoding.GetBytes(
                "-101.09;руб;\"Тестовая категория1\";Повседневные;2015-01-01 00:00:00;\"комментарий1\";\n"+
                "-202.09;руб;\"Тестовая категория2\";Повседневные;2015-01-02 00:00:00;\"комментарий2\";\n"+
                "-300.09;руб;\"Тестовая категория3\";Повседневные;2015-01-03 01:00:00;\"комментарий3\";\n"
            );
            db.Setup(f => f.OpenRead())
                .Returns(new MemoryStream(drebedengiDump));
            
            UploaderOptions options = new()
            {
                tinkoffDump = td.Object,
                drebedengiDump = db.Object,
                o = null
            };
            
            
            // setup ui mock
            var ui = new Moq.Mock<IUserInterface>();
            ui.Setup(i => 
                    i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()))
                .Returns((Expense e1, Expense e2) =>
                {
                    return Task.FromResult(true);
                });

            Uploader uploader = new Uploader(options, ui.Object);

            await uploader.Upload();
            ui.Verify(i => 
                i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()), Times.Never);
        }
        
        [Test]
        public async Task ChecksExpensesInDrebedengiWithoutFalseNegatives()
        {
            // setup tinkoff file
            var td = new Moq.Mock<IFileInfo>();
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var tEncoding = System.Text.Encoding.GetEncoding("windows-1251");
            byte[] tinkoffDump = tEncoding.GetBytes(
                "01.01.2015 00:00:00;-100,09;RUB;Тестовая категория1;комментарий1;Повседневные\n"+
                "02.01.2015 00:00:00;-200,09;RUB;Тестовая категория2;комментарий2;Повседневные\n"+
                "03.01.2015 00:00:00;-300,09;RUB;Тестовая категория3;комментарий3;Повседневные"
            );
            td.Setup(f => f.OpenRead())
                .Returns(new MemoryStream(tinkoffDump));
            
            // setup drebedengi file
            var db = new Moq.Mock<IFileInfo>();
            byte[] drebedengiDump = tEncoding.GetBytes(
                "-101.09;руб;\"Тестовая категория1\";Повседневные;2015-01-01 00:00:00;\"комментарий1\";\n"+
                "-200.09;руб;\"Тестовая категория2\";Повседневные;2015-01-02 00:00:00;\"комментарий2\";\n"+
                "-300.09;руб;\"Тестовая категория3\";Повседневные;2015-01-03 01:00:00;\"комментарий3\";\n"
            );
            db.Setup(f => f.OpenRead())
                .Returns(new MemoryStream(drebedengiDump));
            
            UploaderOptions options = new()
            {
                tinkoffDump = td.Object,
                drebedengiDump = db.Object,
                o = null
            };
            
            
            // setup ui mock
            var ui = new Moq.Mock<IUserInterface>();
            bool wasCalledForSecondLine = false;
            ui.Setup(i => 
                    i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()))
                .Returns((Expense e1, Expense e2) =>
                {
                    if (e1.Money + 200.09M < 0.0001M)
                    {
                        wasCalledForSecondLine = true;
                    }
                    return Task.FromResult(true);
                });

            Uploader uploader = new Uploader(options, ui.Object);

            await uploader.Upload();
            ui.Verify(i => 
                i.ShouldUploadAlike(It.IsAny<Expense>(), It.IsAny<Expense>()), Times.Once);
            Assert.True(wasCalledForSecondLine);
        }
        
        // [Test]
        // public async Task WritesResultToFile()
        // {
        //     Assert.Fail();
        // }
    }
}