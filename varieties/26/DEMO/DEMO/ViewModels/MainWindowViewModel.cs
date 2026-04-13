using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления, отвечающая за загрузку и проверку ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientTwentySixth = new();

    /// <summary>
    /// Текущее значение ФИО, полученное из API.
    /// </summary>
    private string fullNameTwentySixth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentySixth;
        set => SetProperty(ref fullNameTwentySixth, value);
    }

    /// <summary>
    /// Результат выполнения проверки ФИО.
    /// </summary>
    private string validationResultTwentySixth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentySixth;
        set => SetProperty(ref validationResultTwentySixth, value);
    }

    /// <summary>
    /// Загружает ФИО из API и отображает его на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentySixth = await LoadFullNameFromApiTwentySixthAsync();
        FIO = loadedFullNameTwentySixth;
    }

    /// <summary>
    /// Запускает проверку введенного ФИО и обновляет итог.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentySixth(FIO);
    }

    /// <summary>
    /// Формирует текст результата проверки ФИО по двум критериям.
    /// </summary>
    private string BuildValidationMessageTwentySixth(string fioValue)
    {
        var containsDigitTwentySixth = HasDigitInFullNameTwentySixth(fioValue);
        var containsSpecialCharTwentySixth = HasSpecialSymbolInFullNameTwentySixth(fioValue);

        if (containsDigitTwentySixth || containsSpecialCharTwentySixth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет наличие цифр в строке ФИО.
    /// </summary>
    private bool HasDigitInFullNameTwentySixth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет наличие специальных символов !@#$%^&* в ФИО.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentySixth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Выполняет HTTP-запрос к API и возвращает ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentySixthAsync()
    {
        var apiResponseTwentySixth = await sharedHttpClientTwentySixth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentySixth = await apiResponseTwentySixth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentySixth?.Value ?? string.Empty;
    }
}
