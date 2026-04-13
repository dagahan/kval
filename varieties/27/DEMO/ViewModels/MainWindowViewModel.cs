// Вариант 27
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Связующий слой между кнопками интерфейса и проверкой ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _recordFullNameText = string.Empty;
    private string _reportResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get
        {
            return _recordFullNameText;
        }
        set
        {
            SetProperty(ref _recordFullNameText, value);
        }
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get
        {
            return _reportResultText;
        }
        set
        {
            SetProperty(ref _reportResultText, value);
        }
    }

    /// <summary>
    /// Читает ФИО из сервиса и отображает результат в интерфейсе.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var apiNameText = await RequestResponseName();
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
        var validationNameText = PrepareFetchedName(FIO);
        var hasNumericChar = HasAnyNumber(validationNameText);
        var hasForbiddenSign = ContainsSpecialSymbolRule(validationNameText);
        var invalidState = hasNumericChar || hasForbiddenSign;

        Result = invalidState ? "ФИО содержит запрещённые символы" : "ФИО валидно";
    }

    /// <summary>
    /// Загружает ФИО из симулятора и возвращает безопасную строку.
    /// </summary>
    private async Task<string> RequestResponseName()
    {
        using var localHttpClient = new HttpClient();
        var networkResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!networkResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultPayload = await networkResponse.Content.ReadFromJsonAsync<Response>();
        return PrepareFetchedName(resultPayload?.Value);
    }

    /// <summary>
    /// Приводит исходные данные к корректному строковому виду.
    /// </summary>
    private static string PrepareFetchedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: поиск цифр в строке ФИО.
    /// </summary>
    private static bool HasAnyNumber(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: проверка запрещённых символов !@#$%^&*.
    /// </summary>
    private static bool ContainsSpecialSymbolRule(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
