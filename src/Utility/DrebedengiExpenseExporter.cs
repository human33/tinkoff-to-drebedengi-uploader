using T2DUploader.Model;

namespace T2DUploader
{
    public class DrebedengiExpenseExporter
    {
        private static string DREBEDENGI_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static string ExportAsCsvLine(Expense expense)
        {
            string s = expense.Money.ToString(System.Globalization.CultureInfo.InvariantCulture) + ";" + expense.Currency + ";\"" + expense.Category + "\";" + 
                (expense.Account?.Trim() ?? "СЧЕТ_НЕ_ОПРЕДЕЛЕН") + ";" + expense.Date.ToString(DREBEDENGI_DATE_FORMAT) + ";\"" + 
                expense.Comment + "\";";

            if (!string.IsNullOrWhiteSpace(expense.ExpenseGroup))
            {
                s += expense.ExpenseGroup.Trim() + ";";
            }

            return s;
        }
    }
}