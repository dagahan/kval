using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Основная логика экрана валидации данных клиента.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientTwentyThird = new();

    /// <summary>
    /// Строка ФИО, загруженная с сервиса эмулятора.
    /// </summary>
    private string fullNameTwentyThird = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentyThird;
        set => SetProperty(ref fullNameTwentyThird, value);
    }

    /// <summary>
    /// Сообщение о результате проверки данных.
    /// </summary>
    private string validationResultTwentyThird = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentyThird;
        set => SetProperty(ref validationResultTwentyThird, value);
    }

    /// <summary>
    /// Получает данные клиента из эмулятора.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentyThird = await LoadFullNameFromApiTwentyThirdAsync();
        FIO = loadedFullNameTwentyThird;
    }

    /// <summary>
    /// Проверяет ФИО по двум обязательным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentyThird(FIO);
    }

    /// <summary>
    /// Проверяет ФИО на запрещенные признаки и возвращает результат.
    /// </summary>
    private string BuildValidationMessageTwentyThird(string fioValue)
    {
        var containsDigitTwentyThird = HasDigitInFullNameTwentyThird(fioValue);
        var containsSpecialCharTwentyThird = HasSpecialSymbolInFullNameTwentyThird(fioValue);

        if (containsDigitTwentyThird || containsSpecialCharTwentyThird)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Возвращает признак присутствия цифр в ФИО.
    /// </summary>
    private bool HasDigitInFullNameTwentyThird(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Возвращает признак вхождения запрещенных спецсимволов.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentyThird(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Читает данные клиента из API и возвращает строку ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentyThirdAsync()
    {
        var apiResponseTwentyThird = await sharedHttpClientTwentyThird.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentyThird = await apiResponseTwentyThird.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentyThird?.Value ?? string.Empty;
    }
}
