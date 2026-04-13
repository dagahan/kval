using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для сценария загрузки и проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientEighth = new();

    /// <summary>
    /// Строка ФИО, загруженная с сервиса эмулятора.
    /// </summary>
    private string fullNameEighth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameEighth;
        set => SetProperty(ref fullNameEighth, value);
    }

    /// <summary>
    /// Сообщение о результате проверки данных.
    /// </summary>
    private string validationResultEighth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultEighth;
        set => SetProperty(ref validationResultEighth, value);
    }

    /// <summary>
    /// Получает данные клиента из эмулятора.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameEighth = await LoadFullNameFromApiEighthAsync();
        FIO = loadedFullNameEighth;
    }

    /// <summary>
    /// Проверяет ФИО по двум обязательным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageEighth(FIO);
    }

    /// <summary>
    /// Проверяет ФИО на запрещенные признаки и возвращает результат.
    /// </summary>
    private string BuildValidationMessageEighth(string fioValue)
    {
        var containsDigitEighth = HasDigitInFullNameEighth(fioValue);
        var containsSpecialCharEighth = HasSpecialSymbolInFullNameEighth(fioValue);

        if (containsDigitEighth || containsSpecialCharEighth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Возвращает признак присутствия цифр в ФИО.
    /// </summary>
    private bool HasDigitInFullNameEighth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Возвращает признак вхождения запрещенных спецсимволов.
    /// </summary>
    private bool HasSpecialSymbolInFullNameEighth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Читает данные клиента из API и возвращает строку ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiEighthAsync()
    {
        var apiResponseEighth = await sharedHttpClientEighth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelEighth = await apiResponseEighth.Content.ReadFromJsonAsync<Response>();
        return responseModelEighth?.Value ?? string.Empty;
    }
}
