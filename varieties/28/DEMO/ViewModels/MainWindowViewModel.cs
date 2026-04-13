// Вариант 28
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Обработчик сценария загрузки ФИО и вывода результата валидации.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _detailFullNameText = string.Empty;
    private string _validationOutputText = string.Empty;

    private readonly HttpClient _serviceClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _detailFullNameText;
        set => SetProperty(ref _detailFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _validationOutputText;
        set => SetProperty(ref _validationOutputText, value);
    }

    /// <summary>
    /// Выполняет загрузку ФИО из удалённого источника.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var sourceNameText = await FetchResponseName();
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
        var targetNameText = AdjustFetchedName(FIO);

        var containsNumber = ContainsNumericToken(targetNameText);
        var containsSpecialSign = HasSpecialSymbolRule(targetNameText);

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
    private async Task<string> FetchResponseName()
    {
        var simulatorResponse = await _serviceClient.GetAsync(SimulatorEndpoint);

        if (!simulatorResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var namePayload = await simulatorResponse.Content.ReadFromJsonAsync<Response>();
        return AdjustFetchedName(namePayload?.Value);
    }

    /// <summary>
    /// Преобразует входное значение в строку без null.
    /// </summary>
    private static string AdjustFetchedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: контроль присутствия цифровых знаков.
    /// </summary>
    private static bool ContainsNumericToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: контроль спецсимволов из правила !@#$%^&*.
    /// </summary>
    private static bool HasSpecialSymbolRule(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
