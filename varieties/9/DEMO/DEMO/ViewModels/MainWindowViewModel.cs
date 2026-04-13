using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления данными формы валидации клиента.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Отображаемое в форме значение ФИО.
    /// </summary>
    private string fullNameNinth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameNinth;
        set => SetProperty(ref fullNameNinth, value);
    }

    /// <summary>
    /// Строка статуса после проверки ФИО.
    /// </summary>
    private string validationResultNinth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultNinth;
        set => SetProperty(ref validationResultNinth, value);
    }

    /// <summary>
    /// Обновляет ФИО путем вызова внешнего API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameNinth = await LoadFullNameFromApiNinthAsync();
        FIO = loadedFullNameNinth;
    }

    /// <summary>
    /// Рассчитывает результат проверки и показывает его.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageNinth(FIO);
    }

    /// <summary>
    /// Оценивает корректность ФИО и подготавливает текст статуса.
    /// </summary>
    private string BuildValidationMessageNinth(string fioValue)
    {
        var containsDigitNinth = HasDigitInFullNameNinth(fioValue);
        var containsSpecialCharNinth = HasSpecialSymbolInFullNameNinth(fioValue);

        if (containsDigitNinth || containsSpecialCharNinth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет ФИО на вхождение цифр.
    /// </summary>
    private bool HasDigitInFullNameNinth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет ФИО на символы !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameNinth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Отправляет запрос к сервису и извлекает значение ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiNinthAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseNinth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelNinth = await apiResponseNinth.Content.ReadFromJsonAsync<Response>();
        return responseModelNinth?.Value ?? string.Empty;
    }
}
