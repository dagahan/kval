// Вариант 11
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using System;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления действиями экрана проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string SimulatorEndpoint = "http://89.125.39.39:8080/TransferSimulator/fullName";
    private const string DisallowedSymbols = "!@#$%^&*";

    private string _valueFullNameText = string.Empty;
    private string _validationStatusText = string.Empty;

    private static readonly HttpClient _transportClient = new HttpClient();

    /// <summary>
    /// Поле привязки для отображения полученного ФИО.
    /// </summary>
    public string FIO { get => _valueFullNameText; set => SetProperty(ref _valueFullNameText, value); }

    /// <summary>
    /// Поле привязки для отображения результата проверки.
    /// </summary>
    public string Result { get => _validationStatusText; set => SetProperty(ref _validationStatusText, value); }

    /// <summary>
    /// Обновляет поле ФИО значением, полученным от API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var fullNameText = await LoadApiFullName();
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
        var preparedNameText = NormalizeInputName(FIO);
        var digitDetected = HasNumberCharacter(preparedNameText);
        var specialDetected = ContainsSpecialToken(preparedNameText);

        Result = (digitDetected || specialDetected) switch
        {
            true => "ФИО содержит запрещённые символы",
            false => "ФИО валидно",
        };
    }

    /// <summary>
    /// Запрашивает ФИО по сети и подготавливает его для формы.
    /// </summary>
    private async Task<string> LoadApiFullName()
    {
        var apiResponse = await _transportClient.GetAsync(SimulatorEndpoint);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var contentPayload = await apiResponse.Content.ReadFromJsonAsync<Response>();
        return NormalizeInputName(contentPayload?.Value);
    }

    /// <summary>
    /// Гарантирует, что строка ФИО пригодна для проверки.
    /// </summary>
    private static string NormalizeInputName(string? sourceText)
    {
        return sourceText ?? string.Empty;
    }

    /// <summary>
    /// Критерий 1: определение числовых символов в тексте.
    /// </summary>
    private static bool HasNumberCharacter(string sourceText)
    {
        return sourceText.Any(char.IsDigit);
    }

    /// <summary>
    /// Критерий 2: обнаружение символов !@#$%^&* в ФИО.
    /// </summary>
    private static bool ContainsSpecialToken(string sourceText)
    {
        return sourceText.Any(character => DisallowedSymbols.Contains(character));
    }
}
