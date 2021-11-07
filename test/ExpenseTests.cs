using System;
using NUnit.Framework;
using T2DUploader.Model;

namespace T2DUploader.Test
{
    [TestFixture]
    public class ExpenseTests
    {
        [Test]
        public void LikeHappyPath()
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
            
            Expense expense2 = new(
                money: -100M,
                currency: "руб",
                category: "",
                account: "",
                date: new DateTime(2015, 1, 1, 0, 5, 0, DateTimeKind.Unspecified),
                comment: "",
                user: null,
                expenseGroup: null
            );

            bool theyAlike = expense1.Like(expense2);
            
            
            Assert.True(theyAlike);
        }
        
        [Test]
        public void LikeHappyPath2()
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
            
            Expense expense2 = new(
                money: -101M,
                currency: "руб",
                category: "",
                account: "",
                date: new DateTime(2015, 1, 1, 1, 0, 0, DateTimeKind.Unspecified),
                comment: "",
                user: null,
                expenseGroup: null
            );

            bool theyAlike = expense1.Like(expense2);
            
            
            Assert.False(theyAlike);
        }
    }
}