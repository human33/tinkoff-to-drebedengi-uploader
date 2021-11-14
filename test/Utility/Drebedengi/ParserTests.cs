using System;
using System.Threading.Tasks;
using NUnit.Framework;
using T2DUploader.Utility;
using T2DUploader.Utility.Drebedengi;

namespace T2DUploader.Test.Utility.Drebedengi
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public async Task ParseTest()
        {
            Currency rub = new Currency()
            {
                Id = 23955471,
                Name = "Руб",
                Code = "RUB",
                Course = 1,
                IsAutoUpdate = true,
                IsHidden = false,
                IsDefault = true
            };
            Currency usd = new Currency()
            {
                Id = 12411511,
                Name = "USD",
                Code = "USD",
                Course = 70.0383M,
                IsAutoUpdate = true,
                IsHidden = false,
                IsDefault = false
            };

            System.Collections.Generic.List<DrebedengiObject> objects = new()
            {
                new DrebedengiObject()
                {
                    Id = 161412835,
                    ParentId = -1,
                    Type = ObjectType.ExpenseCategory,
                    Name = "Еда",
                    UserId = "",
                    IsCreditCard = false,
                    IsHidden = false,
                    IsForDuty = false,
                    Sort = 15904243,
                    IconId = null,
                    IsAutohide = false
                },
                new DrebedengiObject()
                {
                    Id = 164413165,
                    ParentId = -1,
                    Type = ObjectType.AccountsGroup,
                    Name = "Категории",
                    UserId = "",
                    IsCreditCard = false,
                    IsHidden = false,
                    IsForDuty = false,
                    Sort = 15904266,
                    IconId = 1,
                    IsAutohide = false
                },
                new DrebedengiObject()
                {
                    Id = 160412365,
                    ParentId = 161412835,
                    Type = ObjectType.ExpenseCategory,
                    Name = "Супермаркеты",
                    UserId = "",
                    IsCreditCard = false,
                    IsHidden = false,
                    IsForDuty = false,
                    Sort = 15905459,
                    IconId = null,
                    IsAutohide = false
                },
                new DrebedengiObject()
                {
                    Id = 26051324,
                    ParentId = 164413165,
                    Type = ObjectType.Account,
                    Name = "Учеба",
                    UserId = "",
                    IsCreditCard = false,
                    IsHidden = false,
                    IsForDuty = false,
                    Sort = 15904284,
                    IconId = 59334,
                    IsAutohide = false
                },
                new DrebedengiObject()
                {
                    Id = 66451334,
                    ParentId = 164413165,
                    Type = ObjectType.Account,
                    Name = "Отпуск",
                    UserId = "",
                    IsCreditCard = false,
                    IsHidden = false,
                    IsForDuty = false,
                    Sort = 15904271,
                    IconId = 59330,
                    IsAutohide = false
                },
                new DrebedengiObject()
                {
                    Id = 180234436,
                    ParentId = 164413165,
                    Type = ObjectType.Account,
                    Name = "Повседневные",
                    UserId = "",
                    IsCreditCard = false,
                    IsHidden = false,
                    IsForDuty = false,
                    Sort = 15904267,
                    IconId = 59322,
                    IsAutohide = false
                },
            };

            Record[] records = new[]
            {
                new Record()
                {
                    Sum = 320000,
                    CurrencyId = 23955471,
                    ObjectId = 26051324,
                    AccountId = 66451334,
                    Date = new DateTime(2021, 07, 29, 21, 17, 0, DateTimeKind.Unspecified),
                    Comment = "Тестовая запись перевод из \"Отпуска\" в \"Учеба\"",
                    UserId = 1000000474144,
                    GroupId = null
                },
                new Record()
                {
                    Sum = -320000,
                    CurrencyId = 23955471,
                    ObjectId = 66451334,
                    AccountId = 26051324,
                    Date = new DateTime(2021, 07, 29, 21, 17, 0, DateTimeKind.Unspecified),
                    Comment = "Тестовая запись перевод из \"Отпуска\" в \"Учеба\"",
                    UserId = 1000000474144,
                    GroupId = null
                },
                new Record()
                {
                    Sum = -195680,
                    CurrencyId = 23955471,
                    ObjectId = 160412365,
                    AccountId = 180234436,
                    Date = new DateTime(2021, 05, 09, 12, 25, 0, DateTimeKind.Unspecified),
                    Comment = "Повседневная покупка",
                    UserId = 1000000474144,
                    GroupId = null
                },
                new Record()
                {
                    Sum = -219000,
                    CurrencyId = 23955471,
                    ObjectId = -1,
                    AccountId = 180234436,
                    Date = new DateTime(2001, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                    Comment = "Начальный остаток",
                    UserId = 1000000474145,
                    GroupId = null
                }
            };
            
            var file = new System.IO.FileInfo("test/Dumps/derebedengi_dump.txt");
            var internalFileInfo = new FileInfo(file);
            
            var db = await Parser.ParseExtendedFormat(internalFileInfo);
            
            Assert.AreEqual(2, db.Currencies.Count);
            Assert.AreEqual(rub, db.Currencies[0]);
            Assert.AreEqual(usd, db.Currencies[1]);
            
            Assert.AreEqual(6, db.Objects.Count);
            Assert.AreEqual(objects[0], db.Objects[0]);
            Assert.AreEqual(objects[1], db.Objects[1]);
            Assert.AreEqual(objects[2], db.Objects[2]);
            Assert.AreEqual(objects[3], db.Objects[3]);
            Assert.AreEqual(objects[4], db.Objects[4]);
            Assert.AreEqual(objects[5], db.Objects[5]);
            
            Assert.AreEqual(4, db.Records.Count);
            Assert.AreEqual(records[0], db.Records[0]);
            Assert.AreEqual(records[1], db.Records[1]);
            Assert.AreEqual(records[2], db.Records[2]);
            Assert.AreEqual(records[3], db.Records[3]);
        }
        
    }
}