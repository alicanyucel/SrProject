using Ardalis.SmartEnum;
using FluentValidation;

namespace TELERADIOLOGY.Application.Extensions;
public static class SmartEnumValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> IsValidSmartEnum<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : SmartEnum<TProperty>
    {
        return ruleBuilder.Must(value => value != null && SmartEnum<TProperty>.List.Contains(value))
            .WithMessage("'{PropertyName}' geçersiz bir değer.");
    }

    public static IRuleBuilderOptions<T, int> IsValidSmartEnumValue<T, TSmartEnum>(
        this IRuleBuilder<T, int> ruleBuilder)
        where TSmartEnum : SmartEnum<TSmartEnum>
    {
        return ruleBuilder.Must(value => SmartEnum<TSmartEnum>.List.Any(x => x.Value == value))
            .WithMessage($"'{{PropertyName}}' geçersiz bir {typeof(TSmartEnum).Name} değeri.");
    }

    public static IRuleBuilderOptions<T, int?> IsValidSmartEnumValue<T, TSmartEnum>(
        this IRuleBuilder<T, int?> ruleBuilder)
        where TSmartEnum : SmartEnum<TSmartEnum>
    {
        return ruleBuilder.Must(value => value == null || SmartEnum<TSmartEnum>.List.Any(x => x.Value == value))
            .WithMessage($"'{{PropertyName}}' geçersiz bir {typeof(TSmartEnum).Name} değeri.");
    }
}
