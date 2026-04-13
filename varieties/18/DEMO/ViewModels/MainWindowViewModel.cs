// Вариант 18
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика шага валидации данных в окне приложения.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _displayFullNameText = string.Empty;
    private string _latestResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _displayFullNameText;
        set => SetProperty(ref _displayFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _latestResultText;
        set => SetProperty(ref _latestResultText, value);
    }

    /// <summary>
    /// Забирает ФИО из endpoint и записывает его в привязку.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedNameText = await FetchSimulatorFullName();
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
        var nameForValidation = AdjustApiValue(FIO);
        var containsDigit = ContainsDigitToken(nameForValidation);
        var containsSpecialSymbol = HasSpecialSymbolMarker(nameForValidation);

        if (containsDigit || containsSpecialSymbol)
            Result = "ФИО содержит запрещённые символы";
        else
            Result = "ФИО валидно";
    }

    /// <summary>
    /// Возвращает строку ФИО, полученную из HTTP-ответа.
    /// </summary>
    private async Task<string> FetchSimulatorFullName()
    {
        using var localHttpClient = new HttpClient();
        var simulatorResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!simulatorResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var namePayload = await simulatorResponse.Content.ReadFromJsonAsync<Response>();
        return AdjustApiValue(namePayload?.Value);
    }

    /// <summary>
    /// Упрощает обработку, заменяя null на пустую строку.
    /// </summary>
    private static string AdjustApiValue(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: обнаружение цифр в данных ФИО.
    /// </summary>
    private static bool ContainsDigitToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: анализ строки на наличие !@#$%^&*.
    /// </summary>
    private static bool HasSpecialSymbolMarker(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
