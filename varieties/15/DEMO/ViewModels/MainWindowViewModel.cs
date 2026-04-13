// Вариант 15
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;

namespace DEMO.ViewModels;

/// <summary>
/// Связующий слой между кнопками интерфейса и проверкой ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _validatedFullNameText = string.Empty;
    private string _activeResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get
        {
            return _validatedFullNameText;
        }
        set
        {
            SetProperty(ref _validatedFullNameText, value);
        }
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get
        {
            return _activeResultText;
        }
        set
        {
            SetProperty(ref _activeResultText, value);
        }
    }

    /// <summary>
    /// Читает ФИО из сервиса и отображает результат в интерфейсе.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var apiNameText = await ReadApiFullName();
        FIO = apiNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Стартует валидацию данных и показывает результат пользователю.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Сравнивает ФИО с ограничениями и записывает итог в Result.
    /// </summary>
    public void Validation()
    {
        var validationNameText = ResolveInputName(FIO);
        var hasNumericChar = HasDigitMarker(validationNameText);
        var hasForbiddenSign = ContainsRestrictedSymbol(validationNameText);
        var invalidState = hasNumericChar || hasForbiddenSign;

        Result = invalidState ? "ФИО содержит запрещённые символы" : "ФИО валидно";
    }

    /// <summary>
    /// Загружает ФИО из симулятора и возвращает безопасную строку.
    /// </summary>
    private async Task<string> ReadApiFullName()
    {
        using var localHttpClient = new HttpClient();
        var httpResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responsePayload = await httpResponse.Content.ReadFromJsonAsync<Response>();
        return ResolveInputName(responsePayload?.Value);
    }

    /// <summary>
    /// Приводит исходные данные к корректному строковому виду.
    /// </summary>
    private static string ResolveInputName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: поиск цифр в строке ФИО.
    /// </summary>
    private static bool HasDigitMarker(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: проверка запрещённых символов !@#$%^&*.
    /// </summary>
    private static bool ContainsRestrictedSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
