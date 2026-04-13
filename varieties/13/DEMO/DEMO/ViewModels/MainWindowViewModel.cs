using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика экрана валидации данных клиента.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientThirteenth = new();

    /// <summary>
    /// Строка ФИО, загруженная с сервиса эмулятора.
    /// </summary>
    private string fullNameThirteenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameThirteenth;
        set => SetProperty(ref fullNameThirteenth, value);
    }

    /// <summary>
    /// Сообщение о результате проверки данных.
    /// </summary>
    private string validationResultThirteenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultThirteenth;
        set => SetProperty(ref validationResultThirteenth, value);
    }

    /// <summary>
    /// Получает данные клиента из эмулятора.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameThirteenth = await LoadFullNameFromApiThirteenthAsync();
        FIO = loadedFullNameThirteenth;
    }

    /// <summary>
    /// Проверяет ФИО по двум обязательным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageThirteenth(FIO);
    }

    /// <summary>
    /// Проверяет ФИО на запрещенные признаки и возвращает результат.
    /// </summary>
    private string BuildValidationMessageThirteenth(string fioValue)
    {
        var containsDigitThirteenth = HasDigitInFullNameThirteenth(fioValue);
        var containsSpecialCharThirteenth = HasSpecialSymbolInFullNameThirteenth(fioValue);

        if (containsDigitThirteenth || containsSpecialCharThirteenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Возвращает признак присутствия цифр в ФИО.
    /// </summary>
    private bool HasDigitInFullNameThirteenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Возвращает признак вхождения запрещенных спецсимволов.
    /// </summary>
    private bool HasSpecialSymbolInFullNameThirteenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Читает данные клиента из API и возвращает строку ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiThirteenthAsync()
    {
        var apiResponseThirteenth = await httpClientThirteenth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelThirteenth = await apiResponseThirteenth.Content.ReadFromJsonAsync<Response>();
        return responseModelThirteenth?.Value ?? string.Empty;
    }
}
