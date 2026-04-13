// Вариант 22
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;

namespace DEMO.ViewModels;

/// <summary>
/// Обработчик сценария загрузки ФИО и вывода результата валидации.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _resolvedFullNameText = string.Empty;
    private string _confirmResultText = string.Empty;

    private readonly HttpClient _requestClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _resolvedFullNameText;
        set => SetProperty(ref _resolvedFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _confirmResultText;
        set => SetProperty(ref _confirmResultText, value);
    }

    /// <summary>
    /// Выполняет загрузку ФИО из удалённого источника.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var sourceNameText = await RequestPersonNameFromApi();
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
        var targetNameText = PrepareLoadedName(FIO);

        var containsNumber = DetectNumberToken(targetNameText);
        var containsSpecialSign = HasForbiddenSign(targetNameText);

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
    private async Task<string> RequestPersonNameFromApi()
    {
        var networkResponse = await _requestClient.GetAsync(SimulatorEndpoint);

        if (!networkResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultPayload = await networkResponse.Content.ReadFromJsonAsync<Response>();
        return PrepareLoadedName(resultPayload?.Value);
    }

    /// <summary>
    /// Преобразует входное значение в строку без null.
    /// </summary>
    private static string PrepareLoadedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: контроль присутствия цифровых знаков.
    /// </summary>
    private static bool DetectNumberToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: контроль спецсимволов из правила !@#$%^&*.
    /// </summary>
    private static bool HasForbiddenSign(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
