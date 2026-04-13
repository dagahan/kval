// Вариант 21
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Связующий слой между кнопками интерфейса и проверкой ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _cachedFullNameText = string.Empty;
    private string _responseResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get
        {
            return _cachedFullNameText;
        }
        set
        {
            SetProperty(ref _cachedFullNameText, value);
        }
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get
        {
            return _responseResultText;
        }
        set
        {
            SetProperty(ref _responseResultText, value);
        }
    }

    /// <summary>
    /// Читает ФИО из сервиса и отображает результат в интерфейсе.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var apiNameText = await LoadPersonNameFromApi();
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
        var validationNameText = NormalizeLoadedName(FIO);
        var hasNumericChar = HasNumberToken(validationNameText);
        var hasForbiddenSign = ContainsForbiddenSign(validationNameText);
        var invalidState = hasNumericChar || hasForbiddenSign;

        Result = invalidState ? "ФИО содержит запрещённые символы" : "ФИО валидно";
    }

    /// <summary>
    /// Загружает ФИО из симулятора и возвращает безопасную строку.
    /// </summary>
    private async Task<string> LoadPersonNameFromApi()
    {
        using var localHttpClient = new HttpClient();
        var apiResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var contentPayload = await apiResponse.Content.ReadFromJsonAsync<Response>();
        return NormalizeLoadedName(contentPayload?.Value);
    }

    /// <summary>
    /// Приводит исходные данные к корректному строковому виду.
    /// </summary>
    private static string NormalizeLoadedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: поиск цифр в строке ФИО.
    /// </summary>
    private static bool HasNumberToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: проверка запрещённых символов !@#$%^&*.
    /// </summary>
    private static bool ContainsForbiddenSign(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
