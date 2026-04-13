using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления для сценария загрузки и проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Строка ФИО, загруженная с сервиса эмулятора.
    /// </summary>
    private string fullNameEighteenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameEighteenth;
        set => SetProperty(ref fullNameEighteenth, value);
    }

    /// <summary>
    /// Сообщение о результате проверки данных.
    /// </summary>
    private string validationResultEighteenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultEighteenth;
        set => SetProperty(ref validationResultEighteenth, value);
    }

    /// <summary>
    /// Получает данные клиента из эмулятора.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameEighteenth = await LoadFullNameFromApiEighteenthAsync();
        FIO = loadedFullNameEighteenth;
    }

    /// <summary>
    /// Проверяет ФИО по двум обязательным правилам.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageEighteenth(FIO);
    }

    /// <summary>
    /// Проверяет ФИО на запрещенные признаки и возвращает результат.
    /// </summary>
    private string BuildValidationMessageEighteenth(string fioValue)
    {
        var containsDigitEighteenth = HasDigitInFullNameEighteenth(fioValue);
        var containsSpecialCharEighteenth = HasSpecialSymbolInFullNameEighteenth(fioValue);

        if (containsDigitEighteenth || containsSpecialCharEighteenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Возвращает признак присутствия цифр в ФИО.
    /// </summary>
    private bool HasDigitInFullNameEighteenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Возвращает признак вхождения запрещенных спецсимволов.
    /// </summary>
    private bool HasSpecialSymbolInFullNameEighteenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Читает данные клиента из API и возвращает строку ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiEighteenthAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseEighteenth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelEighteenth = await apiResponseEighteenth.Content.ReadFromJsonAsync<Response>();
        return responseModelEighteenth?.Value ?? string.Empty;
    }
}
