using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика экрана валидации данных клиента.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Строка ФИО, загруженная с сервиса эмулятора.
    /// </summary>
    private string fullNameThird = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameThird;
        set => SetProperty(ref fullNameThird, value);
    }

    /// <summary>
    /// Сообщение о результате проверки данных.
    /// </summary>
    private string validationResultThird = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultThird;
        set => SetProperty(ref validationResultThird, value);
    }

    /// <summary>
    /// Получает данные клиента из эмулятора.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameThird = await LoadFullNameFromApiThirdAsync();
        FIO = loadedFullNameThird;
    }

    /// <summary>
    /// Проверяет ФИО по двум обязательным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageThird(FIO);
    }

    /// <summary>
    /// Проверяет ФИО на запрещенные признаки и возвращает результат.
    /// </summary>
    private string BuildValidationMessageThird(string fioValue)
    {
        var containsDigitThird = HasDigitInFullNameThird(fioValue);
        var containsSpecialCharThird = HasSpecialSymbolInFullNameThird(fioValue);

        if (containsDigitThird || containsSpecialCharThird)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Возвращает признак присутствия цифр в ФИО.
    /// </summary>
    private bool HasDigitInFullNameThird(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Возвращает признак вхождения запрещенных спецсимволов.
    /// </summary>
    private bool HasSpecialSymbolInFullNameThird(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Читает данные клиента из API и возвращает строку ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiThirdAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseThird = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelThird = await apiResponseThird.Content.ReadFromJsonAsync<Response>();
        return responseModelThird?.Value ?? string.Empty;
    }
}
