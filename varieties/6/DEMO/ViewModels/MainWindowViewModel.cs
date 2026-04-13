// Вариант 06
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика шага валидации данных в окне приложения.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _currentFullNameText = string.Empty;
    private string _screenResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _currentFullNameText;
        set => SetProperty(ref _currentFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _screenResultText;
        set => SetProperty(ref _screenResultText, value);
    }

    /// <summary>
    /// Забирает ФИО из endpoint и записывает его в привязку.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedNameText = await LoadFullNameValue();
        FIO = loadedNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Формирует текст результата после проверки ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Обновляет итог проверки в зависимости от найденных символов.
    /// </summary>
    public void Validation()
    {
        var nameForValidation = NormalizeNameText(FIO);
        var containsDigit = DetectDigitSymbol(nameForValidation);
        var containsSpecialSymbol = HasBlockedSpecialSymbol(nameForValidation);

        if (containsDigit || containsSpecialSymbol)
            Result = "ФИО содержит запрещённые символы";
        else
            Result = "ФИО валидно";
    }

    /// <summary>
    /// Возвращает строку ФИО, полученную из HTTP-ответа.
    /// </summary>
    private async Task<string> LoadFullNameValue()
    {
        using var localHttpClient = new HttpClient();
        var apiResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var contentPayload = await apiResponse.Content.ReadFromJsonAsync<Response>();
        return NormalizeNameText(contentPayload?.Value);
    }

    /// <summary>
    /// Упрощает обработку, заменяя null на пустую строку.
    /// </summary>
    private static string NormalizeNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: обнаружение цифр в данных ФИО.
    /// </summary>
    private static bool DetectDigitSymbol(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: анализ строки на наличие !@#$%^&*.
    /// </summary>
    private static bool HasBlockedSpecialSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
