using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Основной ViewModel для экрана проверки корректности ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientTenth = new();

    /// <summary>
    /// Текст ФИО, который пришел от внешнего источника.
    /// </summary>
    private string fullNameTenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTenth;
        set => SetProperty(ref fullNameTenth, value);
    }

    /// <summary>
    /// Итоговое сообщение, показываемое пользователю.
    /// </summary>
    private string validationResultTenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTenth;
        set => SetProperty(ref validationResultTenth, value);
    }

    /// <summary>
    /// Запрашивает ФИО и сохраняет его в состоянии экрана.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTenth = await LoadFullNameFromApiTenthAsync();
        FIO = loadedFullNameTenth;
    }

    /// <summary>
    /// Применяет правила валидации к текущему ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTenth(FIO);
    }

    /// <summary>
    /// Проводит валидацию ФИО по фиксированным условиям.
    /// </summary>
    private string BuildValidationMessageTenth(string fioValue)
    {
        var containsDigitTenth = HasDigitInFullNameTenth(fioValue);
        var containsSpecialCharTenth = HasSpecialSymbolInFullNameTenth(fioValue);

        if (containsDigitTenth || containsSpecialCharTenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Ищет числовые символы в тексте ФИО.
    /// </summary>
    private bool HasDigitInFullNameTenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Ищет специальные символы из фиксированного списка.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Делает вызов API и возвращает полученное ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTenthAsync()
    {
        var apiResponseTenth = await httpClientTenth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTenth = await apiResponseTenth.Content.ReadFromJsonAsync<Response>();
        return responseModelTenth?.Value ?? string.Empty;
    }
}
