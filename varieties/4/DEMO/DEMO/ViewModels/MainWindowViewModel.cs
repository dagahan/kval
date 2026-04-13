using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Представление бизнес-логики формы проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientFourth = new();

    /// <summary>
    /// Отображаемое в форме значение ФИО.
    /// </summary>
    private string fullNameFourth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameFourth;
        set => SetProperty(ref fullNameFourth, value);
    }

    /// <summary>
    /// Строка статуса после проверки ФИО.
    /// </summary>
    private string validationResultFourth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultFourth;
        set => SetProperty(ref validationResultFourth, value);
    }

    /// <summary>
    /// Обновляет ФИО путем вызова внешнего API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameFourth = await LoadFullNameFromApiFourthAsync();
        FIO = loadedFullNameFourth;
    }

    /// <summary>
    /// Рассчитывает результат проверки и показывает его.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageFourth(FIO);
    }

    /// <summary>
    /// Оценивает корректность ФИО и подготавливает текст статуса.
    /// </summary>
    private string BuildValidationMessageFourth(string fioValue)
    {
        var containsDigitFourth = HasDigitInFullNameFourth(fioValue);
        var containsSpecialCharFourth = HasSpecialSymbolInFullNameFourth(fioValue);

        if (containsDigitFourth || containsSpecialCharFourth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет ФИО на вхождение цифр.
    /// </summary>
    private bool HasDigitInFullNameFourth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет ФИО на символы !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameFourth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Отправляет запрос к сервису и извлекает значение ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiFourthAsync()
    {
        var apiResponseFourth = await httpClientFourth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelFourth = await apiResponseFourth.Content.ReadFromJsonAsync<Response>();
        return responseModelFourth?.Value ?? string.Empty;
    }
}
