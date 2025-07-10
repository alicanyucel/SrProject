using Ardalis.SmartEnum;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TELERADIOLOGY.Infrastructure.Converters;
public class SmartEnumJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return IsSmartEnum(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(SmartEnumJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private static bool IsSmartEnum(Type type)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SmartEnum<>))
            {
                return true;
            }
            type = type.BaseType!;
        }
        return false;
    }
}

public class SmartEnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : SmartEnum<TEnum>
{
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number when reader.TryGetInt32(out var intValue)
                    => SmartEnum<TEnum>.FromValue(intValue),

                JsonTokenType.String when !string.IsNullOrWhiteSpace(reader.GetString())
                    => SmartEnum<TEnum>.FromName(reader.GetString()!, ignoreCase: true),

                JsonTokenType.Null
                    => default(TEnum),

                _ => throw new JsonException($"Cannot convert JSON token '{reader.TokenType}' to {typeof(TEnum).Name}. Expected Number.")
            };
        }
        catch (ArgumentException ex) when (ex.Message.Contains("was not found") || ex.Message.Contains("not defined"))
        {
            var tokenValue = reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetInt32().ToString(),
                JsonTokenType.String => $"'{reader.GetString()}'",
                _ => reader.TokenType.ToString()
            };

            throw new JsonException($"Value {tokenValue} is not a valid {typeof(TEnum).Name}.", ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNumberValue(value.Value);
    }
}