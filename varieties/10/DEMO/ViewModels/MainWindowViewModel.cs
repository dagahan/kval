// Вариант 10
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Обработчик сценария загрузки ФИО и вывода результата валидации.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _viewFullNameText = string.Empty;
    private string _testResultText = string.Empty;

    private readonly HttpClient _fetchClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO
    {
        get => _viewFullNameText;
        set => SetProperty(ref _viewFullNameText, value);
    }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result
    {
        get => _testResultText;
        set => SetProperty(ref _testResultText, value);
    }

    /// <summary>
    /// Выполняет загрузку ФИО из удалённого источника.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var sourceNameText = await ReadRemoteFullName();
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
        var targetNameText = ResolveNameText(FIO);

        var containsNumber = ContainsNumberCharacter(targetNameText);
        var containsSpecialSign = HasSpecialCharacterSet(targetNameText);

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
    private async Task<string> ReadRemoteFullName()
    {
        var httpResponse = await _fetchClient.GetAsync(SimulatorEndpoint);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responsePayload = await httpResponse.Content.ReadFromJsonAsync<Response>();
        return ResolveNameText(responsePayload?.Value);
    }

    /// <summary>
    /// Преобразует входное значение в строку без null.
    /// </summary>
    private static string ResolveNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: контроль присутствия цифровых знаков.
    /// </summary>
    private static bool ContainsNumberCharacter(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: контроль спецсимволов из правила !@#$%^&*.
    /// </summary>
    private static bool HasSpecialCharacterSet(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
