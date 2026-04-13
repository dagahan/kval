using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Класс, который управляет получением и проверкой входных данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientTwentyFifth = new();

    /// <summary>
    /// Текст ФИО, который пришел от внешнего источника.
    /// </summary>
    private string fullNameTwentyFifth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentyFifth;
        set => SetProperty(ref fullNameTwentyFifth, value);
    }

    /// <summary>
    /// Итоговое сообщение, показываемое пользователю.
    /// </summary>
    private string validationResultTwentyFifth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentyFifth;
        set => SetProperty(ref validationResultTwentyFifth, value);
    }

    /// <summary>
    /// Запрашивает ФИО и сохраняет его в состоянии экрана.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentyFifth = await LoadFullNameFromApiTwentyFifthAsync();
        FIO = loadedFullNameTwentyFifth;
    }

    /// <summary>
    /// Применяет правила валидации к текущему ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentyFifth(FIO);
    }

    /// <summary>
    /// Проводит валидацию ФИО по фиксированным условиям.
    /// </summary>
    private string BuildValidationMessageTwentyFifth(string fioValue)
    {
        var containsDigitTwentyFifth = HasDigitInFullNameTwentyFifth(fioValue);
        var containsSpecialCharTwentyFifth = HasSpecialSymbolInFullNameTwentyFifth(fioValue);

        if (containsDigitTwentyFifth || containsSpecialCharTwentyFifth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Ищет числовые символы в тексте ФИО.
    /// </summary>
    private bool HasDigitInFullNameTwentyFifth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Ищет специальные символы из фиксированного списка.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentyFifth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Делает вызов API и возвращает полученное ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentyFifthAsync()
    {
        var apiResponseTwentyFifth = await httpClientTwentyFifth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentyFifth = await apiResponseTwentyFifth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentyFifth?.Value ?? string.Empty;
    }
}
