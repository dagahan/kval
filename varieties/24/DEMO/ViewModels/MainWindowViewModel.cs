// Вариант 24
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика шага валидации данных в окне приложения.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _activeFullNameText = string.Empty;
    private string _outcomeResultText = string.Empty;

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _activeFullNameText;
        set => SetProperty(ref _activeFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _outcomeResultText;
        set => SetProperty(ref _outcomeResultText, value);
    }

    /// <summary>
    /// Забирает ФИО из endpoint и записывает его в привязку.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedNameText = await GetPersonNameFromApi();
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
        var nameForValidation = ComposeLoadedName(FIO);
        var containsDigit = ContainsDigitCharacter(nameForValidation);
        var containsSpecialSymbol = HasDisallowedSpecialSymbol(nameForValidation);

        if (containsDigit || containsSpecialSymbol)
            Result = "ФИО содержит запрещённые символы";
        else
            Result = "ФИО валидно";
    }

    /// <summary>
    /// Возвращает строку ФИО, полученную из HTTP-ответа.
    /// </summary>
    private async Task<string> GetPersonNameFromApi()
    {
        using var localHttpClient = new HttpClient();
        var serviceResponse = await localHttpClient.GetAsync(SimulatorEndpoint);

        if (!serviceResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var jsonPayload = await serviceResponse.Content.ReadFromJsonAsync<Response>();
        return ComposeLoadedName(jsonPayload?.Value);
    }

    /// <summary>
    /// Упрощает обработку, заменяя null на пустую строку.
    /// </summary>
    private static string ComposeLoadedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: обнаружение цифр в данных ФИО.
    /// </summary>
    private static bool ContainsDigitCharacter(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: анализ строки на наличие !@#$%^&*.
    /// </summary>
    private static bool HasDisallowedSpecialSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
