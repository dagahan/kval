// Вариант 30
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика шага валидации данных в окне приложения.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _entryFullNameText = string.Empty;
    private string _outputResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _entryFullNameText;
        set => SetProperty(ref _entryFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _outputResultText;
        set => SetProperty(ref _outputResultText, value);
    }

    /// <summary>
    /// Забирает ФИО из endpoint и записывает его в привязку.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedNameText = await ReadResponseName();
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
        var nameForValidation = ResolveFetchedName(FIO);
        var containsDigit = ContainsNumberSymbol(nameForValidation);
        var containsSpecialSymbol = HasSpecialCharacterRule(nameForValidation);

        if (containsDigit || containsSpecialSymbol)
            Result = "ФИО содержит запрещённые символы";
        else
            Result = "ФИО валидно";
    }

    /// <summary>
    /// Возвращает строку ФИО, полученную из HTTP-ответа.
    /// </summary>
    private async Task<string> ReadResponseName()
    {
        using var localHttpClient = new HttpClient();
        var httpResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responsePayload = await httpResponse.Content.ReadFromJsonAsync<Response>();
        return ResolveFetchedName(responsePayload?.Value);
    }

    /// <summary>
    /// Упрощает обработку, заменяя null на пустую строку.
    /// </summary>
    private static string ResolveFetchedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: обнаружение цифр в данных ФИО.
    /// </summary>
    private static bool ContainsNumberSymbol(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: анализ строки на наличие !@#$%^&*.
    /// </summary>
    private static bool HasSpecialCharacterRule(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
