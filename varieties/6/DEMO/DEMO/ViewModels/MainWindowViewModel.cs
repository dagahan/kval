using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления, отвечающая за загрузку и проверку ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Текущее значение ФИО, полученное из API.
    /// </summary>
    private string fullNameSixth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameSixth;
        set => SetProperty(ref fullNameSixth, value);
    }

    /// <summary>
    /// Результат выполнения проверки ФИО.
    /// </summary>
    private string validationResultSixth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultSixth;
        set => SetProperty(ref validationResultSixth, value);
    }

    /// <summary>
    /// Загружает ФИО из API и отображает его на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameSixth = await LoadFullNameFromApiSixthAsync();
        FIO = loadedFullNameSixth;
    }

    /// <summary>
    /// Запускает проверку введенного ФИО и обновляет итог.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageSixth(FIO);
    }

    /// <summary>
    /// Формирует текст результата проверки ФИО по двум критериям.
    /// </summary>
    private string BuildValidationMessageSixth(string fioValue)
    {
        var containsDigitSixth = HasDigitInFullNameSixth(fioValue);
        var containsSpecialCharSixth = HasSpecialSymbolInFullNameSixth(fioValue);

        if (containsDigitSixth || containsSpecialCharSixth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет наличие цифр в строке ФИО.
    /// </summary>
    private bool HasDigitInFullNameSixth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет наличие специальных символов !@#$%^&* в ФИО.
    /// </summary>
    private bool HasSpecialSymbolInFullNameSixth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Выполняет HTTP-запрос к API и возвращает ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiSixthAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseSixth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelSixth = await apiResponseSixth.Content.ReadFromJsonAsync<Response>();
        return responseModelSixth?.Value ?? string.Empty;
    }
}
