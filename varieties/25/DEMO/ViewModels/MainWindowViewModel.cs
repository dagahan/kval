// Вариант 25
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Логика окна: загрузка ФИО и выполнение проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _receivedNameText = string.Empty;
    private string _assessmentResultText = string.Empty;

    private readonly HttpClient _transportClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _receivedNameText;
        set => SetProperty(ref _receivedNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _assessmentResultText;
        set => SetProperty(ref _assessmentResultText, value);
    }

    /// <summary>
    /// Запрашивает ФИО у симулятора и заполняет поле на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var receivedNameText = await ReadPersonNameFromApi();
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
        var currentNameText = ResolveLoadedName(FIO);
        var hasDigit = HasDigitCharacter(currentNameText);
        var hasSpecialSymbol = ContainsSpecialFromRule(currentNameText);

        Result = hasDigit || hasSpecialSymbol
            ? "ФИО содержит запрещённые символы"
            : "ФИО валидно";
    }

    /// <summary>
    /// Делает HTTP-запрос и возвращает текст ФИО из ответа.
    /// </summary>
    private async Task<string> ReadPersonNameFromApi()
    {
        var httpResponse = await _transportClient.GetAsync(SimulatorEndpoint);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responsePayload = await httpResponse.Content.ReadFromJsonAsync<Response>();
        return ResolveLoadedName(responsePayload?.Value);
    }

    /// <summary>
    /// Нормализует входную строку и исключает null-значения.
    /// </summary>
    private static string ResolveLoadedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: наличие хотя бы одной цифры в ФИО.
    /// </summary>
    private static bool HasDigitCharacter(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: наличие символов из набора !@#$%^&*.
    /// </summary>
    private static bool ContainsSpecialFromRule(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
