// Вариант 29
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления действиями экрана проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _packetFullNameText = string.Empty;
    private string _resultLineText = string.Empty;

    private static readonly HttpClient _apiClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _packetFullNameText; set => SetProperty(ref _packetFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _resultLineText; set => SetProperty(ref _resultLineText, value); }

    /// <summary>
    /// Обновляет поле ФИО значением, полученным от API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fullNameText = await GetResponseName();
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
        var preparedNameText = ComposeFetchedName(FIO);
        var digitDetected = HasNumericToken(preparedNameText);
        var specialDetected = ContainsSpecialCharacterRule(preparedNameText);

        Result = (digitDetected || specialDetected) switch
        {
            true => "ФИО содержит запрещённые символы",
            false => "ФИО валидно",
        };
    }

    /// <summary>
    /// Запрашивает ФИО по сети и подготавливает его для формы.
    /// </summary>
    private async Task<string> GetResponseName()
    {
        var serviceResponse = await _apiClient.GetAsync(SimulatorEndpoint);

        if (!serviceResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var jsonPayload = await serviceResponse.Content.ReadFromJsonAsync<Response>();
        return ComposeFetchedName(jsonPayload?.Value);
    }

    /// <summary>
    /// Гарантирует, что строка ФИО пригодна для проверки.
    /// </summary>
    private static string ComposeFetchedName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: определение числовых символов в тексте.
    /// </summary>
    private static bool HasNumericToken(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: обнаружение символов !@#$%^&* в ФИО.
    /// </summary>
    private static bool ContainsSpecialCharacterRule(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
