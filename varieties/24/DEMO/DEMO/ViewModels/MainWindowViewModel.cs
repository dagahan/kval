using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Представление бизнес-логики формы проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Отображаемое в форме значение ФИО.
    /// </summary>
    private string fullNameTwentyFourth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentyFourth;
        set => SetProperty(ref fullNameTwentyFourth, value);
    }

    /// <summary>
    /// Строка статуса после проверки ФИО.
    /// </summary>
    private string validationResultTwentyFourth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentyFourth;
        set => SetProperty(ref validationResultTwentyFourth, value);
    }

    /// <summary>
    /// Обновляет ФИО путем вызова внешнего API.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentyFourth = await LoadFullNameFromApiTwentyFourthAsync();
        FIO = loadedFullNameTwentyFourth;
    }

    /// <summary>
    /// Рассчитывает результат проверки и показывает его.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentyFourth(FIO);
    }

    /// <summary>
    /// Оценивает корректность ФИО и подготавливает текст статуса.
    /// </summary>
    private string BuildValidationMessageTwentyFourth(string fioValue)
    {
        var containsDigitTwentyFourth = HasDigitInFullNameTwentyFourth(fioValue);
        var containsSpecialCharTwentyFourth = HasSpecialSymbolInFullNameTwentyFourth(fioValue);

        if (containsDigitTwentyFourth || containsSpecialCharTwentyFourth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет ФИО на вхождение цифр.
    /// </summary>
    private bool HasDigitInFullNameTwentyFourth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет ФИО на символы !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentyFourth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Отправляет запрос к сервису и извлекает значение ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentyFourthAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseTwentyFourth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentyFourth = await apiResponseTwentyFourth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentyFourth?.Value ?? string.Empty;
    }
}
