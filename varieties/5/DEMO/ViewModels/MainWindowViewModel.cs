// Вариант 05
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления действиями экрана проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _candidateFullNameText = string.Empty;
    private string _checkResultText = string.Empty;

    private static readonly HttpClient _networkClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _candidateFullNameText; set => SetProperty(ref _candidateFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _checkResultText; set => SetProperty(ref _checkResultText, value); }

    /// <summary>
    /// Обновляет поле ФИО значением, полученным от API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fullNameText = await ReadFullNameFromApi();
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
        var preparedNameText = ResolveFullNameText(FIO);
        var digitDetected = HasNumericCharacter(preparedNameText);
        var specialDetected = ContainsBlockedSpecialSymbol(preparedNameText);

        Result = (digitDetected || specialDetected) switch
        {
            true => "ФИО содержит запрещённые символы",
            false => "ФИО валидно",
        };
    }

    /// <summary>
    /// Запрашивает ФИО по сети и подготавливает его для формы.
    /// </summary>
    private async Task<string> ReadFullNameFromApi()
    {
        var httpResponse = await _networkClient.GetAsync(SimulatorEndpoint);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responsePayload = await httpResponse.Content.ReadFromJsonAsync<Response>();
        return ResolveFullNameText(responsePayload?.Value);
    }

    /// <summary>
    /// Гарантирует, что строка ФИО пригодна для проверки.
    /// </summary>
    private static string ResolveFullNameText(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: определение числовых символов в тексте.
    /// </summary>
    private static bool HasNumericCharacter(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: обнаружение символов !@#$%^&* в ФИО.
    /// </summary>
    private static bool ContainsBlockedSpecialSymbol(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
