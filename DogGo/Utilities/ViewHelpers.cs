namespace DogGo.Utilities
{
    public class ViewHelpers
    {
        public static string DurationToText(int duration)
        {
            int hours = duration / 3600;
            int minutes = (duration % 3600)/60;
            string result = "";
            if (hours > 0)
            {
                result += $"{hours} hr "; //1 hr 
            }

            result += $"{minutes} min"; //1 hr 15 min

            return result;
        }

    }
}
