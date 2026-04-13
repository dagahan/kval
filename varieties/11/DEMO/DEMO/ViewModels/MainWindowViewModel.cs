using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления главного окна проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientEleventh = new();

    /// <summary>
    /// Текущее значение ФИО, полученное из API.
    /// </summary>
    private string fullNameEleventh = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameEleventh;
        set => SetProperty(ref fullNameEleventh, value);
    }

    /// <summary>
    /// Результат выполнения проверки ФИО.
    /// </summary>
    private string validationResultEleventh = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultEleventh;
        set => SetProperty(ref validationResultEleventh, value);
    }

    /// <summary>
    /// Загружает ФИО из API и отображает его на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameEleventh = await LoadFullNameFromApiEleventhAsync();
        FIO = loadedFullNameEleventh;
    }

    /// <summary>
    /// Запускает проверку введенного ФИО и обновляет итог.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageEleventh(FIO);
    }

    /// <summary>
    /// Формирует текст результата проверки ФИО по двум критериям.
    /// </summary>
    private string BuildValidationMessageEleventh(string fioValue)
    {
        var containsDigitEleventh = HasDigitInFullNameEleventh(fioValue);
        var containsSpecialCharEleventh = HasSpecialSymbolInFullNameEleventh(fioValue);

        if (containsDigitEleventh || containsSpecialCharEleventh)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет наличие цифр в строке ФИО.
    /// </summary>
    private bool HasDigitInFullNameEleventh(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет наличие специальных символов !@#$%^&* в ФИО.
    /// </summary>
    private bool HasSpecialSymbolInFullNameEleventh(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Выполняет HTTP-запрос к API и возвращает ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiEleventhAsync()
    {
        var apiResponseEleventh = await sharedHttpClientEleventh.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelEleventh = await apiResponseEleventh.Content.ReadFromJsonAsync<Response>();
        return responseModelEleventh?.Value ?? string.Empty;
    }
}
