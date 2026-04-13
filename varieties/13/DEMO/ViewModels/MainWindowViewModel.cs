// Вариант 13
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Логика окна: загрузка ФИО и выполнение проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _responseFullNameText = string.Empty;
    private string _resultMessageText = string.Empty;

    private readonly HttpClient _apiClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _responseFullNameText;
        set => SetProperty(ref _responseFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _resultMessageText;
        set => SetProperty(ref _resultMessageText, value);
    }

    /// <summary>
    /// Запрашивает ФИО у симулятора и заполняет поле на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var receivedNameText = await FetchApiFullName();
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
        var currentNameText = AdjustInputName(FIO);
        var hasDigit = FindNumericInText(currentNameText);
        var hasSpecialSymbol = ContainsProhibitedSymbol(currentNameText);

        Result = hasDigit || hasSpecialSymbol
            ? "ФИО содержит запрещённые символы"
            : "ФИО валидно";
    }

    /// <summary>
    /// Делает HTTP-запрос и возвращает текст ФИО из ответа.
    /// </summary>
    private async Task<string> FetchApiFullName()
    {
        var simulatorResponse = await _apiClient.GetAsync(SimulatorEndpoint);

        if (!simulatorResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var namePayload = await simulatorResponse.Content.ReadFromJsonAsync<Response>();
        return AdjustInputName(namePayload?.Value);
    }

    /// <summary>
    /// Нормализует входную строку и исключает null-значения.
    /// </summary>
    private static string AdjustInputName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: наличие хотя бы одной цифры в ФИО.
    /// </summary>
    private static bool FindNumericInText(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: наличие символов из набора !@#$%^&*.
    /// </summary>
    private static bool ContainsProhibitedSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
