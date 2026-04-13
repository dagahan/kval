// Вариант 16
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Обработчик сценария загрузки ФИО и вывода результата валидации.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _rawFullNameText = string.Empty;
    private string _computedResultText = string.Empty;

    private readonly HttpClient _queryClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _rawFullNameText;
        set => SetProperty(ref _rawFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _computedResultText;
        set => SetProperty(ref _computedResultText, value);
    }

    /// <summary>
    /// Выполняет загрузку ФИО из удалённого источника.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var sourceNameText = await LoadSimulatorFullName();
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
        var targetNameText = NormalizeApiValue(FIO);

        var containsNumber = ContainsDigitValue(targetNameText);
        var containsSpecialSign = HasRestrictedSymbol(targetNameText);

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
    private async Task<string> LoadSimulatorFullName()
    {
        var apiResponse = await _queryClient.GetAsync(SimulatorEndpoint);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var contentPayload = await apiResponse.Content.ReadFromJsonAsync<Response>();
        return NormalizeApiValue(contentPayload?.Value);
    }

    /// <summary>
    /// Преобразует входное значение в строку без null.
    /// </summary>
    private static string NormalizeApiValue(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: контроль присутствия цифровых знаков.
    /// </summary>
    private static bool ContainsDigitValue(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: контроль спецсимволов из правила !@#$%^&*.
    /// </summary>
    private static bool HasRestrictedSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
