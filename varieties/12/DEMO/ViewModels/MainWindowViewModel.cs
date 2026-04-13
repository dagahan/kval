// Вариант 12
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика шага валидации данных в окне приложения.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _requestFullNameText = string.Empty;
    private string _resultStatusText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _requestFullNameText;
        set => SetProperty(ref _requestFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _resultStatusText;
        set => SetProperty(ref _resultStatusText, value);
    }

    /// <summary>
    /// Забирает ФИО из endpoint и записывает его в привязку.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedNameText = await RequestApiFullName();
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
        var nameForValidation = PrepareInputName(FIO);
        var containsDigit = FindDigitInText(nameForValidation);
        var containsSpecialSymbol = HasSpecialToken(nameForValidation);

        if (containsDigit || containsSpecialSymbol)
            Result = "ФИО содержит запрещённые символы";
        else
            Result = "ФИО валидно";
    }

    /// <summary>
    /// Возвращает строку ФИО, полученную из HTTP-ответа.
    /// </summary>
    private async Task<string> RequestApiFullName()
    {
        using var localHttpClient = new HttpClient();
        var networkResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!networkResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultPayload = await networkResponse.Content.ReadFromJsonAsync<Response>();
        return PrepareInputName(resultPayload?.Value);
    }

    /// <summary>
    /// Упрощает обработку, заменяя null на пустую строку.
    /// </summary>
    private static string PrepareInputName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: обнаружение цифр в данных ФИО.
    /// </summary>
    private static bool FindDigitInText(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: анализ строки на наличие !@#$%^&*.
    /// </summary>
    private static bool HasSpecialToken(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
