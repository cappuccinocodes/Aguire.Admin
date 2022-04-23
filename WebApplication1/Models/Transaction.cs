using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebApplication1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public TransactionType TransactionType { get; set; }
    }

    public enum TransactionType
    {
        [Display(Name = "Income")]
        Income = 1,

        [Display(Name = "Expense")]
        Expense = 2
    }

    public static class EnumExtensions
    {
        public static string ToDescription(this TransactionType enumValue)
        {
            var enumType = enumValue.GetType();

        return enumType
                .GetMember(enumValue.ToString())
                .Where(x => x.MemberType == MemberTypes.Field && ((FieldInfo)x).FieldType == enumType)
                .First()
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
        }
    }
}
