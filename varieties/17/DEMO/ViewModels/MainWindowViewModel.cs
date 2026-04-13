// Вариант 17
using System.Net.Http;
using System.Linq;
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления действиями экрана проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _apiFullNameText = string.Empty;
    private string _resolvedResultText = string.Empty;

    private static readonly HttpClient _httpClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _apiFullNameText; set => SetProperty(ref _apiFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _resolvedResultText; set => SetProperty(ref _resolvedResultText, value); }

    /// <summary>
    /// Обновляет поле ФИО значением, полученным от API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fullNameText = await RequestSimulatorFullName();
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
        var preparedNameText = PrepareApiValue(FIO);
        var digitDetected = HasDigitValue(preparedNameText);
        var specialDetected = ContainsSpecialSymbolMarker(preparedNameText);

        Result = (digitDetected || specialDetected) switch
        {
            true => "ФИО содержит запрещённые символы",
            false => "ФИО валидно",
        };
    }

    /// <summary>
    /// Запрашивает ФИО по сети и подготавливает его для формы.
    /// </summary>
    private async Task<string> RequestSimulatorFullName()
    {
        var networkResponse = await _httpClient.GetAsync(SimulatorEndpoint);

        if (!networkResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultPayload = await networkResponse.Content.ReadFromJsonAsync<Response>();
        return PrepareApiValue(resultPayload?.Value);
    }

    /// <summary>
    /// Гарантирует, что строка ФИО пригодна для проверки.
    /// </summary>
    private static string PrepareApiValue(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: определение числовых символов в тексте.
    /// </summary>
    private static bool HasDigitValue(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: обнаружение символов !@#$%^&* в ФИО.
    /// </summary>
    private static bool ContainsSpecialSymbolMarker(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
