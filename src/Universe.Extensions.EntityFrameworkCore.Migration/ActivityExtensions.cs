// Source reference from https://github.com/dotnet/eShop/tree/release/8.0

namespace Universe.Extensions.EntityFrameworkCore.Migration;

using System.Diagnostics;

internal static class ActivityExtensions
{
    public static void SetExceptionTags(this Activity activity, Exception ex)
    {
        activity.AddTag("exception.message", ex.Message);
        activity.AddTag("exception.stacktrace", ex.ToString());
        activity.AddTag("exception.type", ex.GetType().FullName);
        activity.SetStatus(ActivityStatusCode.Error);
    }
}
