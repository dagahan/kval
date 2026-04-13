// Вариант 07
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Логика окна: загрузка ФИО и выполнение проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _simulatorFullNameText = string.Empty;
    private string _uiResultText = string.Empty;

    private readonly HttpClient _nameClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _simulatorFullNameText;
        set => SetProperty(ref _simulatorFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _uiResultText;
        set => SetProperty(ref _uiResultText, value);
    }

    /// <summary>
    /// Запрашивает ФИО у симулятора и заполняет поле на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var receivedNameText = await RequestNameValue();
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
        var currentNameText = PrepareNameText(FIO);
        var hasDigit = DetectNumericSymbol(currentNameText);
        var hasSpecialSymbol = ContainsSpecialCharacterFromList(currentNameText);

        Result = hasDigit || hasSpecialSymbol
            ? "ФИО содержит запрещённые символы"
            : "ФИО валидно";
    }

    /// <summary>
    /// Делает HTTP-запрос и возвращает текст ФИО из ответа.
    /// </summary>
    private async Task<string> RequestNameValue()
    {
        var networkResponse = await _nameClient.GetAsync(SimulatorEndpoint);

        if (!networkResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultPayload = await networkResponse.Content.ReadFromJsonAsync<Response>();
        return PrepareNameText(resultPayload?.Value);
    }

    /// <summary>
    /// Нормализует входную строку и исключает null-значения.
    /// </summary>
    private static string PrepareNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: наличие хотя бы одной цифры в ФИО.
    /// </summary>
    private static bool DetectNumericSymbol(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: наличие символов из набора !@#$%^&*.
    /// </summary>
    private static bool ContainsSpecialCharacterFromList(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
