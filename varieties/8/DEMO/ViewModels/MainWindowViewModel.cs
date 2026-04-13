// Вариант 08
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для получения строки ФИО и запуска проверки.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _screenFullNameText = string.Empty;
    private string _viewResultText = string.Empty;

    private static readonly HttpClient _requestClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _screenFullNameText; set => SetProperty(ref _screenFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _viewResultText; set => SetProperty(ref _viewResultText, value); }

    /// <summary>
    /// Получает строку ФИО по API и выводит её пользователю.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fetchedNameText = await FetchNameText();
        FIO = fetchedNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Выполняет тестирование строки ФИО по установленным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Определяет валидность ФИО по критериям цифр и спецсимволов.
    /// </summary>
    public void Validation()
    {
        var normalizedNameText = AdjustNameText(FIO);
        var digitFound = ContainsAnyDigitSymbol(normalizedNameText);
        var specialFound = HasSpecialCharacterFromList(normalizedNameText);

        if (digitFound || specialFound)
        {
            Result = "ФИО содержит запрещённые символы";
            return;
        }

        Result = "ФИО валидно";
    }

    /// <summary>
    /// Получает объект ответа от API и извлекает поле Value.
    /// </summary>
    private async Task<string> FetchNameText()
    {
        var simulatorResponse = await _requestClient.GetAsync(SimulatorEndpoint);

        if (!simulatorResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var namePayload = await simulatorResponse.Content.ReadFromJsonAsync<Response>();
        return AdjustNameText(namePayload?.Value);
    }

    /// <summary>
    /// Подготавливает текст ФИО для безопасной обработки.
    /// </summary>
    private static string AdjustNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: проверка на числовые символы.
    /// </summary>
    private static bool ContainsAnyDigitSymbol(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: поиск спецсимволов !@#$%^&* в строке.
    /// </summary>
    private static bool HasSpecialCharacterFromList(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
