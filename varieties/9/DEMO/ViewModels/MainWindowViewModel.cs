// Вариант 09
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Связующий слой между кнопками интерфейса и проверкой ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _outputFullNameText = string.Empty;
    private string _finalResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get
        {
            return _outputFullNameText;
        }
        set
        {
            SetProperty(ref _outputFullNameText, value);
        }
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get
        {
            return _finalResultText;
        }
        set
        {
            SetProperty(ref _finalResultText, value);
        }
    }

    /// <summary>
    /// Читает ФИО из сервиса и отображает результат в интерфейсе.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var apiNameText = await GetRemoteFullName();
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
        var validationNameText = ComposeNameText(FIO);
        var hasNumericChar = HasAnyDigitSymbol(validationNameText);
        var hasForbiddenSign = ContainsSpecialCharacterSet(validationNameText);
        var invalidState = hasNumericChar || hasForbiddenSign;

        Result = invalidState ? "ФИО содержит запрещённые символы" : "ФИО валидно";
    }

    /// <summary>
    /// Загружает ФИО из симулятора и возвращает безопасную строку.
    /// </summary>
    private async Task<string> GetRemoteFullName()
    {
        using var localHttpClient = new HttpClient();
        var serviceResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!serviceResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var jsonPayload = await serviceResponse.Content.ReadFromJsonAsync<Response>();
        return ComposeNameText(jsonPayload?.Value);
    }

    /// <summary>
    /// Приводит исходные данные к корректному строковому виду.
    /// </summary>
    private static string ComposeNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: поиск цифр в строке ФИО.
    /// </summary>
    private static bool HasAnyDigitSymbol(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: проверка запрещённых символов !@#$%^&*.
    /// </summary>
    private static bool ContainsSpecialCharacterSet(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
