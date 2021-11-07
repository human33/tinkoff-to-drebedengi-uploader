using System;

namespace T2DUploader.Model
{
    public record Expense
    {
        public decimal Money {get;set;}
        public string Currency {get;set;}
        public string Category {get;set;}
        public string? Account {get;set;}
        public DateTime Date {get;set;}
        public string Comment {get;set;}
        public string? User {get;set;}
        public string? ExpenseGroup {get;set;}

        public Expense(decimal money, string currency, string category, string? account, DateTime date, 
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
            return Math.Abs((Date - expense.Date).TotalMinutes) < 10 && 
                Math.Abs(Money - expense.Money) < 1M; // if difference only in one currency point
        }
        
        public bool Similar(Expense expense)
        {
            return Math.Abs((Date - expense.Date).TotalDays) < 1 && 
                Math.Abs(Money - expense.Money) < 1M; // if difference only in one currency point
        }

        public override int GetHashCode() 
        {
            return 0; // implemented to remove warning, use equals instead
        }

        public virtual bool Equals(Expense? other)
        {
            if (other == null)
            {
                return false;
            }
            
            return Money        == other.Money &&
                Currency     == other.Currency &&
                Category     == other.Category &&
                Account      == other.Account &&
                Date         == other.Date &&
                Comment      == other.Comment &&
                User         == other.User &&
                ExpenseGroup == other.ExpenseGroup;
        }
    }
}