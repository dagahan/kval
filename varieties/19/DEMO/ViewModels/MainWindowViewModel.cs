// Вариант 19
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Логика окна: загрузка ФИО и выполнение проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _uiFullNameText = string.Empty;
    private string _displayResultText = string.Empty;

    private readonly HttpClient _networkClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _uiFullNameText;
        set => SetProperty(ref _uiFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _displayResultText;
        set => SetProperty(ref _displayResultText, value);
    }

    /// <summary>
    /// Запрашивает ФИО у симулятора и заполняет поле на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var receivedNameText = await GetSimulatorFullName();
        FIO = receivedNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Запускает проверку текущего ФИО и обновляет итоговое сообщение.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Проверяет строку ФИО по двум правилам и выставляет статус.
    /// </summary>
    public void Validation()
    {
        var currentNameText = ComposeApiValue(FIO);
        var hasDigit = HasDigitToken(currentNameText);
        var hasSpecialSymbol = ContainsSpecialSymbolInName(currentNameText);

        Result = hasDigit || hasSpecialSymbol
            ? "ФИО содержит запрещённые символы"
            : "ФИО валидно";
    }

    /// <summary>
    /// Делает HTTP-запрос и возвращает текст ФИО из ответа.
    /// </summary>
    private async Task<string> GetSimulatorFullName()
    {
        var serviceResponse = await _networkClient.GetAsync(SimulatorEndpoint);

        if (!serviceResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var jsonPayload = await serviceResponse.Content.ReadFromJsonAsync<Response>();
        return ComposeApiValue(jsonPayload?.Value);
    }

    /// <summary>
    /// Нормализует входную строку и исключает null-значения.
    /// </summary>
    private static string ComposeApiValue(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: наличие хотя бы одной цифры в ФИО.
    /// </summary>
    private static bool HasDigitToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: наличие символов из набора !@#$%^&*.
    /// </summary>
    private static bool ContainsSpecialSymbolInName(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
