using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace yajusense.Core.Config.JsonConverters;

public class KeyCodeJsonConverter : JsonConverter<KeyCode>
{
    public override KeyCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var intValue))
            {
                if (Enum.IsDefined(typeof(KeyCode), intValue))
                {
                    return (KeyCode)intValue;
                }
                
                YjPlugin.Log.LogWarning($"Invalid KeyCode value: {intValue}");
                return KeyCode.None;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                if (string.IsNullOrEmpty(strValue))
                {
                    return KeyCode.None;
                }
                
                if (Enum.TryParse<KeyCode>(strValue, true, out var result))
                {
                    return result;
                }
                
                YjPlugin.Log.LogWarning($"Could not parse KeyCode from string: {strValue}");
            }
        }
        catch (Exception ex)
        {
            YjPlugin.Log.LogError($"Error parsing KeyCode: {ex.Message}");
        }

        return KeyCode.None;

    }

    public override void Write(Utf8JsonWriter writer, KeyCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}