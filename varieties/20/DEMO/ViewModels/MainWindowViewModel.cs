// Вариант 20
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using System;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для получения строки ФИО и запуска проверки.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _savedFullNameText = string.Empty;
    private string _savedResultText = string.Empty;

    private static readonly HttpClient _simulatorClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _savedFullNameText; set => SetProperty(ref _savedFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _savedResultText; set => SetProperty(ref _savedResultText, value); }

    /// <summary>
    /// Получает строку ФИО по API и выводит её пользователю.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fetchedNameText = await ReadSimulatorFullName();
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
        var normalizedNameText = ResolveApiValue(FIO);
        var digitFound = ContainsNumberToken(normalizedNameText);
        var specialFound = HasSpecialSymbolInName(normalizedNameText);

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
    private async Task<string> ReadSimulatorFullName()
    {
        var httpResponse = await _simulatorClient.GetAsync(SimulatorEndpoint);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responsePayload = await httpResponse.Content.ReadFromJsonAsync<Response>();
        return ResolveApiValue(responsePayload?.Value);
    }

    /// <summary>
    /// Подготавливает текст ФИО для безопасной обработки.
    /// </summary>
    private static string ResolveApiValue(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: проверка на числовые символы.
    /// </summary>
    private static bool ContainsNumberToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: поиск спецсимволов !@#$%^&* в строке.
    /// </summary>
    private static bool HasSpecialSymbolInName(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
