using System;

namespace T2DUploader
{
    public record Expense
    {
        public decimal Money {get;set;}
        public string Currency {get;set;}
        public string Category {get;set;}
        public string Account {get;set;}
        public DateTime Date {get;set;}
        public string Comment {get;set;}
        public string? User {get;set;}
        public string? ExpenseGroup {get;set;}

        public Expense(decimal money, string currency, string category, string account, DateTime date, 
            string comment, string? user, string? expenseGroup)
        {
            Money = money;
            Currency = currency;
            Category = category;
            Account = account;
            Date = date;
            Comment = comment;
            User = user;
            ExpenseGroup = expenseGroup;
        }

        public bool Like(Expense expense)
        {
            return Date == expense.Date && Money == expense.Money;
        }
    }
}