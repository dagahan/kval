// Вариант 03
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Связующий слой между кнопками интерфейса и проверкой ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _loadedFullNameText = string.Empty;
    private string _resultOutputText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get
        {
            return _loadedFullNameText;
        }
        set
        {
            SetProperty(ref _loadedFullNameText, value);
        }
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get
        {
            return _resultOutputText;
        }
        set
        {
            SetProperty(ref _resultOutputText, value);
        }
    }

    /// <summary>
    /// Читает ФИО из сервиса и отображает результат в интерфейсе.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var apiNameText = await FetchFullNameFromService();
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
        var validationNameText = AdjustFullNameText(FIO);
        var hasNumericChar = IsDigitPresentInFullName(validationNameText);
        var hasForbiddenSign = ContainsSpecialSignFromSet(validationNameText);
        var invalidState = hasNumericChar || hasForbiddenSign;

        Result = invalidState ? "ФИО содержит запрещённые символы" : "ФИО валидно";
    }

    /// <summary>
    /// Загружает ФИО из симулятора и возвращает безопасную строку.
    /// </summary>
    private async Task<string> FetchFullNameFromService()
    {
        using var localHttpClient = new HttpClient();
        var simulatorResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!simulatorResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var namePayload = await simulatorResponse.Content.ReadFromJsonAsync<Response>();
        return AdjustFullNameText(namePayload?.Value);
    }

    /// <summary>
    /// Приводит исходные данные к корректному строковому виду.
    /// </summary>
    private static string AdjustFullNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: поиск цифр в строке ФИО.
    /// </summary>
    private static bool IsDigitPresentInFullName(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: проверка запрещённых символов !@#$%^&*.
    /// </summary>
    private static bool ContainsSpecialSignFromSet(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
