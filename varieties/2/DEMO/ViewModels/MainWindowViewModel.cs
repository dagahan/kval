// Вариант 02
using System.Linq;
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для получения строки ФИО и запуска проверки.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _personFullNameText = string.Empty;
    private string _validationMessageText = string.Empty;

    private static readonly HttpClient _queryClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _personFullNameText; set => SetProperty(ref _personFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _validationMessageText; set => SetProperty(ref _validationMessageText, value); }

    /// <summary>
    /// Получает строку ФИО по API и выводит её пользователю.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fetchedNameText = await RequestFullNameFromApi();
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
        var normalizedNameText = PrepareFullNameText(FIO);
        var digitFound = HasDigitInFullName(normalizedNameText);
        var specialFound = HasForbiddenSpecialSymbol(normalizedNameText);

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
    private async Task<string> RequestFullNameFromApi()
    {
        var networkResponse = await _queryClient.GetAsync(SimulatorEndpoint);

        if (!networkResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultPayload = await networkResponse.Content.ReadFromJsonAsync<Response>();
        return PrepareFullNameText(resultPayload?.Value);
    }

    /// <summary>
    /// Подготавливает текст ФИО для безопасной обработки.
    /// </summary>
    private static string PrepareFullNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: проверка на числовые символы.
    /// </summary>
    private static bool HasDigitInFullName(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: поиск спецсимволов !@#$%^&* в строке.
    /// </summary>
    private static bool HasForbiddenSpecialSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
