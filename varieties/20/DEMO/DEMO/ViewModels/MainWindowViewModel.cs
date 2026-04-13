using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Основной ViewModel для экрана проверки корректности ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientTwentieth = new();

    /// <summary>
    /// Текст ФИО, который пришел от внешнего источника.
    /// </summary>
    private string fullNameTwentieth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentieth;
        set => SetProperty(ref fullNameTwentieth, value);
    }

    /// <summary>
    /// Итоговое сообщение, показываемое пользователю.
    /// </summary>
    private string validationResultTwentieth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentieth;
        set => SetProperty(ref validationResultTwentieth, value);
    }

    /// <summary>
    /// Запрашивает ФИО и сохраняет его в состоянии экрана.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentieth = await LoadFullNameFromApiTwentiethAsync();
        FIO = loadedFullNameTwentieth;
    }

    /// <summary>
    /// Применяет правила валидации к текущему ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentieth(FIO);
    }

    /// <summary>
    /// Проводит валидацию ФИО по фиксированным условиям.
    /// </summary>
    private string BuildValidationMessageTwentieth(string fioValue)
    {
        var containsDigitTwentieth = HasDigitInFullNameTwentieth(fioValue);
        var containsSpecialCharTwentieth = HasSpecialSymbolInFullNameTwentieth(fioValue);

        if (containsDigitTwentieth || containsSpecialCharTwentieth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Ищет числовые символы в тексте ФИО.
    /// </summary>
    private bool HasDigitInFullNameTwentieth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Ищет специальные символы из фиксированного списка.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentieth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Делает вызов API и возвращает полученное ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentiethAsync()
    {
        var apiResponseTwentieth = await sharedHttpClientTwentieth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentieth = await apiResponseTwentieth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentieth?.Value ?? string.Empty;
    }
}
