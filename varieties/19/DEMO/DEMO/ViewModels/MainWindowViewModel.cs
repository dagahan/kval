using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Класс управления данными формы валидации клиента.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientNineteenth = new();

    /// <summary>
    /// Отображаемое в форме значение ФИО.
    /// </summary>
    private string fullNameNineteenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameNineteenth;
        set => SetProperty(ref fullNameNineteenth, value);
    }

    /// <summary>
    /// Строка статуса после проверки ФИО.
    /// </summary>
    private string validationResultNineteenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultNineteenth;
        set => SetProperty(ref validationResultNineteenth, value);
    }

    /// <summary>
    /// Обновляет ФИО путем вызова внешнего API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameNineteenth = await LoadFullNameFromApiNineteenthAsync();
        FIO = loadedFullNameNineteenth;
    }

    /// <summary>
    /// Рассчитывает результат проверки и показывает его.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageNineteenth(FIO);
    }

    /// <summary>
    /// Оценивает корректность ФИО и подготавливает текст статуса.
    /// </summary>
    private string BuildValidationMessageNineteenth(string fioValue)
    {
        var containsDigitNineteenth = HasDigitInFullNameNineteenth(fioValue);
        var containsSpecialCharNineteenth = HasSpecialSymbolInFullNameNineteenth(fioValue);

        if (containsDigitNineteenth || containsSpecialCharNineteenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет ФИО на вхождение цифр.
    /// </summary>
    private bool HasDigitInFullNameNineteenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет ФИО на символы !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameNineteenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Отправляет запрос к сервису и извлекает значение ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiNineteenthAsync()
    {
        var apiResponseNineteenth = await httpClientNineteenth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelNineteenth = await apiResponseNineteenth.Content.ReadFromJsonAsync<Response>();
        return responseModelNineteenth?.Value ?? string.Empty;
    }
}
