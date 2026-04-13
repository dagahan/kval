// Вариант 01
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;

namespace DEMO.ViewModels;

/// <summary>
/// Логика окна: загрузка ФИО и выполнение проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _fullNameText = string.Empty;
    private string _validationText = string.Empty;

    private readonly HttpClient _httpClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _fullNameText;
        set => SetProperty(ref _fullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _validationText;
        set => SetProperty(ref _validationText, value);
    }

    /// <summary>
    /// Запрашивает ФИО у симулятора и заполняет поле на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var receivedNameText = await LoadFullNameFromApi();
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
        var currentNameText = NormalizeFullNameText(FIO);
        var hasDigit = ContainsDigitInFullName(currentNameText);
        var hasSpecialSymbol = ContainsForbiddenSpecialSymbol(currentNameText);

        Result = hasDigit || hasSpecialSymbol
            ? "ФИО содержит запрещённые символы"
            : "ФИО валидно";
    }

    /// <summary>
    /// Делает HTTP-запрос и возвращает текст ФИО из ответа.
    /// </summary>
    private async Task<string> LoadFullNameFromApi()
    {
        var apiResponse = await _httpClient.GetAsync(SimulatorEndpoint);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var contentPayload = await apiResponse.Content.ReadFromJsonAsync<Response>();
        return NormalizeFullNameText(contentPayload?.Value);
    }

    /// <summary>
    /// Нормализует входную строку и исключает null-значения.
    /// </summary>
    private static string NormalizeFullNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: наличие хотя бы одной цифры в ФИО.
    /// </summary>
    private static bool ContainsDigitInFullName(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: наличие символов из набора !@#$%^&*.
    /// </summary>
    private static bool ContainsForbiddenSpecialSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
