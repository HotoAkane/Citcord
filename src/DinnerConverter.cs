namespace Citcord;

public class DinnerConverter
{
    public static string GetUrl(string placeCode)
    {
        return $"https://www.cit-s.com/wp/wp-content/themes/cit/menu/{placeCode}_{GetDayCode()}.png";
    }
    
    private static string GetDayCode()
    {
        DateTimeOffset jpTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9));
        
        DateTimeOffset monday = jpTime.AddDays(- GetMondayDiff(jpTime.DayOfWeek));

        if (jpTime.Month != monday.Month)
        {
            return $"{jpTime.Year}{jpTime.Month:00}_1";
        }
        else
        {
            DateTimeOffset firstDay = new DateTimeOffset(monday.Year, monday.Month, 1, 0, 0, 0, TimeSpan.FromHours(9));
            
            return $"{monday.Year}{monday.Month:00}_{(monday.Day + (int) firstDay.DayOfWeek - 2) / 7 + 1}";
        }
    }

    private static int GetMondayDiff(DayOfWeek dayOfWeek)
    {
        return dayOfWeek == DayOfWeek.Sunday ? 6 : (int) dayOfWeek - 1;
    }
}