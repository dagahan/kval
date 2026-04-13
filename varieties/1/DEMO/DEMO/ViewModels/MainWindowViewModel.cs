using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления главного окна проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientFirst = new();

    /// <summary>
    /// Текущее значение ФИО, полученное из API.
    /// </summary>
    private string fullNameFirst = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameFirst;
        set => SetProperty(ref fullNameFirst, value);
    }

    /// <summary>
    /// Результат выполнения проверки ФИО.
    /// </summary>
    private string validationResultFirst = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultFirst;
        set => SetProperty(ref validationResultFirst, value);
    }

    /// <summary>
    /// Загружает ФИО из API и отображает его на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameFirst = await LoadFullNameFromApiFirstAsync();
        FIO = loadedFullNameFirst;
    }

    /// <summary>
    /// Запускает проверку введенного ФИО и обновляет итог.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageFirst(FIO);
    }

    /// <summary>
    /// Формирует текст результата проверки ФИО по двум критериям.
    /// </summary>
    private string BuildValidationMessageFirst(string fioValue)
    {
        var containsDigitFirst = HasDigitInFullNameFirst(fioValue);
        var containsSpecialCharFirst = HasSpecialSymbolInFullNameFirst(fioValue);

        if (containsDigitFirst || containsSpecialCharFirst)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет наличие цифр в строке ФИО.
    /// </summary>
    private bool HasDigitInFullNameFirst(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет наличие специальных символов !@#$%^&* в ФИО.
    /// </summary>
    private bool HasSpecialSymbolInFullNameFirst(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Выполняет HTTP-запрос к API и возвращает ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiFirstAsync()
    {
        var apiResponseFirst = await httpClientFirst.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelFirst = await apiResponseFirst.Content.ReadFromJsonAsync<Response>();
        return responseModelFirst?.Value ?? string.Empty;
    }
}
