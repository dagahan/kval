using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Логика взаимодействия интерфейса и правил валидации ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientSeventh = new();

    /// <summary>
    /// ФИО клиента, отображаемое в интерфейсе.
    /// </summary>
    private string fullNameSeventh = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameSeventh;
        set => SetProperty(ref fullNameSeventh, value);
    }

    /// <summary>
    /// Текстовый итог текущей валидации.
    /// </summary>
    private string validationResultSeventh = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultSeventh;
        set => SetProperty(ref validationResultSeventh, value);
    }

    /// <summary>
    /// Выполняет запрос к сервису и обновляет значение ФИО.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameSeventh = await LoadFullNameFromApiSeventhAsync();
        FIO = loadedFullNameSeventh;
    }

    /// <summary>
    /// Выполняет валидацию текущего ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageSeventh(FIO);
    }

    /// <summary>
    /// Выполняет проверку строки ФИО и возвращает итоговое сообщение.
    /// </summary>
    private string BuildValidationMessageSeventh(string fioValue)
    {
        var containsDigitSeventh = HasDigitInFullNameSeventh(fioValue);
        var containsSpecialCharSeventh = HasSpecialSymbolInFullNameSeventh(fioValue);

        if (containsDigitSeventh || containsSpecialCharSeventh)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Определяет, есть ли в ФИО числовые символы.
    /// </summary>
    private bool HasDigitInFullNameSeventh(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Определяет, есть ли в ФИО символы из набора !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameSeventh(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Получает ФИО из удаленного эмулятора по заданному URL.
    /// </summary>
    private async Task<string> LoadFullNameFromApiSeventhAsync()
    {
        var apiResponseSeventh = await httpClientSeventh.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelSeventh = await apiResponseSeventh.Content.ReadFromJsonAsync<Response>();
        return responseModelSeventh?.Value ?? string.Empty;
    }
}
