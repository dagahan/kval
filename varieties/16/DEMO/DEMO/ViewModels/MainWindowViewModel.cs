using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления, отвечающая за загрузку и проверку ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientSixteenth = new();

    /// <summary>
    /// Текущее значение ФИО, полученное из API.
    /// </summary>
    private string fullNameSixteenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameSixteenth;
        set => SetProperty(ref fullNameSixteenth, value);
    }

    /// <summary>
    /// Результат выполнения проверки ФИО.
    /// </summary>
    private string validationResultSixteenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultSixteenth;
        set => SetProperty(ref validationResultSixteenth, value);
    }

    /// <summary>
    /// Загружает ФИО из API и отображает его на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameSixteenth = await LoadFullNameFromApiSixteenthAsync();
        FIO = loadedFullNameSixteenth;
    }

    /// <summary>
    /// Запускает проверку введенного ФИО и обновляет итог.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageSixteenth(FIO);
    }

    /// <summary>
    /// Формирует текст результата проверки ФИО по двум критериям.
    /// </summary>
    private string BuildValidationMessageSixteenth(string fioValue)
    {
        var containsDigitSixteenth = HasDigitInFullNameSixteenth(fioValue);
        var containsSpecialCharSixteenth = HasSpecialSymbolInFullNameSixteenth(fioValue);

        if (containsDigitSixteenth || containsSpecialCharSixteenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет наличие цифр в строке ФИО.
    /// </summary>
    private bool HasDigitInFullNameSixteenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет наличие специальных символов !@#$%^&* в ФИО.
    /// </summary>
    private bool HasSpecialSymbolInFullNameSixteenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Выполняет HTTP-запрос к API и возвращает ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiSixteenthAsync()
    {
        var apiResponseSixteenth = await httpClientSixteenth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelSixteenth = await apiResponseSixteenth.Content.ReadFromJsonAsync<Response>();
        return responseModelSixteenth?.Value ?? string.Empty;
    }
}
