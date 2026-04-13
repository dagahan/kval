using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для сценария загрузки и проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientTwentyEighth = new();

    /// <summary>
    /// Строка ФИО, загруженная с сервиса эмулятора.
    /// </summary>
    private string fullNameTwentyEighth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentyEighth;
        set => SetProperty(ref fullNameTwentyEighth, value);
    }

    /// <summary>
    /// Сообщение о результате проверки данных.
    /// </summary>
    private string validationResultTwentyEighth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentyEighth;
        set => SetProperty(ref validationResultTwentyEighth, value);
    }

    /// <summary>
    /// Получает данные клиента из эмулятора.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentyEighth = await LoadFullNameFromApiTwentyEighthAsync();
        FIO = loadedFullNameTwentyEighth;
    }

    /// <summary>
    /// Проверяет ФИО по двум обязательным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentyEighth(FIO);
    }

    /// <summary>
    /// Проверяет ФИО на запрещенные признаки и возвращает результат.
    /// </summary>
    private string BuildValidationMessageTwentyEighth(string fioValue)
    {
        var containsDigitTwentyEighth = HasDigitInFullNameTwentyEighth(fioValue);
        var containsSpecialCharTwentyEighth = HasSpecialSymbolInFullNameTwentyEighth(fioValue);

        if (containsDigitTwentyEighth || containsSpecialCharTwentyEighth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Возвращает признак присутствия цифр в ФИО.
    /// </summary>
    private bool HasDigitInFullNameTwentyEighth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Возвращает признак вхождения запрещенных спецсимволов.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentyEighth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Читает данные клиента из API и возвращает строку ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentyEighthAsync()
    {
        var apiResponseTwentyEighth = await httpClientTwentyEighth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentyEighth = await apiResponseTwentyEighth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentyEighth?.Value ?? string.Empty;
    }
}
