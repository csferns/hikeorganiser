namespace HikeOrganiser.Data.Enums;

[Flags]
public enum DateType
{
    None = 0,
    Manual = 1,
    Period = 1 << 1,
    Recurring = 1 << 2,
    
    RecurringManual = Manual | Recurring,
    RecurringPeriod = Recurring | Period
}