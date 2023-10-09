using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace SecurityLabs.Extensions;

public class RefitSettingsExtension
{
    public static RefitSettings ProjectDefaultSettings => new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        })
    };
}