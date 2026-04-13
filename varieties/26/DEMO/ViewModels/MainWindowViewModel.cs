// Вариант 26
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для получения строки ФИО и запуска проверки.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _profileFullNameText = string.Empty;
    private string _summaryResultText = string.Empty;

    private static readonly HttpClient _fetchClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _profileFullNameText; set => SetProperty(ref _profileFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _summaryResultText; set => SetProperty(ref _summaryResultText, value); }

    /// <summary>
    /// Получает строку ФИО по API и выводит её пользователю.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fetchedNameText = await LoadResponseName();
        FIO = fetchedNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Выполняет тестирование строки ФИО по установленным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Определяет валидность ФИО по критериям цифр и спецсимволов.
    /// </summary>
    public void Validation()
    {
        var normalizedNameText = NormalizeFetchedName(FIO);
        var digitFound = ContainsAnyNumber(normalizedNameText);
        var specialFound = HasSpecialFromRule(normalizedNameText);

        if (digitFound || specialFound)
        {
            Result = "ФИО содержит запрещённые символы";
            return;
        }

        Result = "ФИО валидно";
    }

    /// <summary>
    /// Получает объект ответа от API и извлекает поле Value.
    /// </summary>
    private async Task<string> LoadResponseName()
    {
        var apiResponse = await _fetchClient.GetAsync(SimulatorEndpoint);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var contentPayload = await apiResponse.Content.ReadFromJsonAsync<Response>();
        return NormalizeFetchedName(contentPayload?.Value);
    }

    /// <summary>
    /// Подготавливает текст ФИО для безопасной обработки.
    /// </summary>
    private static string NormalizeFetchedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: проверка на числовые символы.
    /// </summary>
    private static bool ContainsAnyNumber(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: поиск спецсимволов !@#$%^&* в строке.
    /// </summary>
    private static bool HasSpecialFromRule(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
