// Вариант 23
using System.Linq;
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления действиями экрана проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _userFullNameText = string.Empty;
    private string _stateResultText = string.Empty;

    private static readonly HttpClient _nameClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _userFullNameText; set => SetProperty(ref _userFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _stateResultText; set => SetProperty(ref _stateResultText, value); }

    /// <summary>
    /// Обновляет поле ФИО значением, полученным от API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fullNameText = await FetchPersonNameFromApi();
        FIO = fullNameText;
        Result = string.Empty;
    }

    /// <summary>
    /// Проводит проверку загруженного ФИО при нажатии кнопки.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Validation();
    }

    /// <summary>
    /// Проводит анализ строки ФИО по заданным условиям.
    /// </summary>
    public void Validation()
    {
        var preparedNameText = AdjustLoadedName(FIO);
        var digitDetected = DetectDigitToken(preparedNameText);
        var specialDetected = ContainsDisallowedSpecialSymbol(preparedNameText);

        Result = (digitDetected || specialDetected) switch
        {
            true => "ФИО содержит запрещённые символы",
            false => "ФИО валидно",
        };
    }

    /// <summary>
    /// Запрашивает ФИО по сети и подготавливает его для формы.
    /// </summary>
    private async Task<string> FetchPersonNameFromApi()
    {
        var simulatorResponse = await _nameClient.GetAsync(SimulatorEndpoint);

        if (!simulatorResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var namePayload = await simulatorResponse.Content.ReadFromJsonAsync<Response>();
        return AdjustLoadedName(namePayload?.Value);
    }

    /// <summary>
    /// Гарантирует, что строка ФИО пригодна для проверки.
    /// </summary>
    private static string AdjustLoadedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: определение числовых символов в тексте.
    /// </summary>
    private static bool DetectDigitToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: обнаружение символов !@#$%^&* в ФИО.
    /// </summary>
    private static bool ContainsDisallowedSpecialSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
