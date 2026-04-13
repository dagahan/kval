// Вариант 14
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using System;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для получения строки ФИО и запуска проверки.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _fetchedFullNameText = string.Empty;
    private string _currentResultText = string.Empty;

    private static readonly HttpClient _serviceClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _fetchedFullNameText; set => SetProperty(ref _fetchedFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _currentResultText; set => SetProperty(ref _currentResultText, value); }

    /// <summary>
    /// Получает строку ФИО по API и выводит её пользователю.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fetchedNameText = await GetApiFullName();
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
        var normalizedNameText = ComposeInputName(FIO);
        var digitFound = ContainsDigitMarker(normalizedNameText);
        var specialFound = HasProhibitedSymbol(normalizedNameText);

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
    private async Task<string> GetApiFullName()
    {
        var serviceResponse = await _serviceClient.GetAsync(SimulatorEndpoint);

        if (!serviceResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var jsonPayload = await serviceResponse.Content.ReadFromJsonAsync<Response>();
        return ComposeInputName(jsonPayload?.Value);
    }

    /// <summary>
    /// Подготавливает текст ФИО для безопасной обработки.
    /// </summary>
    private static string ComposeInputName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: проверка на числовые символы.
    /// </summary>
    private static bool ContainsDigitMarker(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: поиск спецсимволов !@#$%^&* в строке.
    /// </summary>
    private static bool HasProhibitedSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
