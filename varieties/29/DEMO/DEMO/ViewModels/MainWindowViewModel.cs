using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления данными формы валидации клиента.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientTwentyNinth = new();

    /// <summary>
    /// Отображаемое в форме значение ФИО.
    /// </summary>
    private string fullNameTwentyNinth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentyNinth;
        set => SetProperty(ref fullNameTwentyNinth, value);
    }

    /// <summary>
    /// Строка статуса после проверки ФИО.
    /// </summary>
    private string validationResultTwentyNinth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentyNinth;
        set => SetProperty(ref validationResultTwentyNinth, value);
    }

    /// <summary>
    /// Обновляет ФИО путем вызова внешнего API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentyNinth = await LoadFullNameFromApiTwentyNinthAsync();
        FIO = loadedFullNameTwentyNinth;
    }

    /// <summary>
    /// Рассчитывает результат проверки и показывает его.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentyNinth(FIO);
    }

    /// <summary>
    /// Оценивает корректность ФИО и подготавливает текст статуса.
    /// </summary>
    private string BuildValidationMessageTwentyNinth(string fioValue)
    {
        var containsDigitTwentyNinth = HasDigitInFullNameTwentyNinth(fioValue);
        var containsSpecialCharTwentyNinth = HasSpecialSymbolInFullNameTwentyNinth(fioValue);

        if (containsDigitTwentyNinth || containsSpecialCharTwentyNinth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет ФИО на вхождение цифр.
    /// </summary>
    private bool HasDigitInFullNameTwentyNinth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет ФИО на символы !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentyNinth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Отправляет запрос к сервису и извлекает значение ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentyNinthAsync()
    {
        var apiResponseTwentyNinth = await sharedHttpClientTwentyNinth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentyNinth = await apiResponseTwentyNinth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentyNinth?.Value ?? string.Empty;
    }
}
