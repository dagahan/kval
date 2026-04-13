using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DEMO.ViewModels;

/// <summary>
/// Представление бизнес-логики формы проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientFourteenth = new();

    /// <summary>
    /// Отображаемое в форме значение ФИО.
    /// </summary>
    private string fullNameFourteenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameFourteenth;
        set => SetProperty(ref fullNameFourteenth, value);
    }

    /// <summary>
    /// Строка статуса после проверки ФИО.
    /// </summary>
    private string validationResultFourteenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultFourteenth;
        set => SetProperty(ref validationResultFourteenth, value);
    }

    /// <summary>
    /// Обновляет ФИО путем вызова внешнего API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameFourteenth = await LoadFullNameFromApiFourteenthAsync();
        FIO = loadedFullNameFourteenth;
    }

    /// <summary>
    /// Рассчитывает результат проверки и показывает его.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageFourteenth(FIO);
    }

    /// <summary>
    /// Оценивает корректность ФИО и подготавливает текст статуса.
    /// </summary>
    private string BuildValidationMessageFourteenth(string fioValue)
    {
        var containsDigitFourteenth = HasDigitInFullNameFourteenth(fioValue);
        var containsSpecialCharFourteenth = HasSpecialSymbolInFullNameFourteenth(fioValue);

        if (containsDigitFourteenth || containsSpecialCharFourteenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет ФИО на вхождение цифр.
    /// </summary>
    private bool HasDigitInFullNameFourteenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет ФИО на символы !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameFourteenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Отправляет запрос к сервису и извлекает значение ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiFourteenthAsync()
    {
        var apiResponseFourteenth = await sharedHttpClientFourteenth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelFourteenth = await apiResponseFourteenth.Content.ReadFromJsonAsync<Response>();
        return responseModelFourteenth?.Value ?? string.Empty;
    }
}
