// Вариант 04
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Обработчик сценария загрузки ФИО и вывода результата валидации.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _receivedFullNameText = string.Empty;
    private string _statusOutputText = string.Empty;

    private readonly HttpClient _simulatorClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _receivedFullNameText;
        set => SetProperty(ref _receivedFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _statusOutputText;
        set => SetProperty(ref _statusOutputText, value);
    }

    /// <summary>
    /// Выполняет загрузку ФИО из удалённого источника.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var sourceNameText = await GetFullNameFromSimulator();
        FIO = sourceNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Инициирует контроль ФИО по двум критериям.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Выполняет валидацию ФИО и формирует конечное сообщение.
    /// </summary>
    public void Validation()
    {
        var targetNameText = ComposeFullNameText(FIO);

        var containsNumber = ContainsNumericCharacter(targetNameText);
        var containsSpecialSign = HasSpecialSignFromSet(targetNameText);

        var hasValidationError = containsNumber || containsSpecialSign;
        if (hasValidationError)
        {
            Result = "ФИО содержит запрещённые символы";
        }
        else
        {
            Result = "ФИО валидно";
        }
    }

    /// <summary>
    /// Читает данные endpoint и возвращает подготовленное ФИО.
    /// </summary>
    private async Task<string> GetFullNameFromSimulator()
    {
        var serviceResponse = await _simulatorClient.GetAsync(SimulatorEndpoint);

        if (!serviceResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var jsonPayload = await serviceResponse.Content.ReadFromJsonAsync<Response>();
        return ComposeFullNameText(jsonPayload?.Value);
    }

    /// <summary>
    /// Преобразует входное значение в строку без null.
    /// </summary>
    private static string ComposeFullNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: контроль присутствия цифровых знаков.
    /// </summary>
    private static bool ContainsNumericCharacter(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: контроль спецсимволов из правила !@#$%^&*.
    /// </summary>
    private static bool HasSpecialSignFromSet(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
