using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления главного окна проверки данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Текущее значение ФИО, полученное из API.
    /// </summary>
    private string fullNameTwentyFirst = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentyFirst;
        set => SetProperty(ref fullNameTwentyFirst, value);
    }

    /// <summary>
    /// Результат выполнения проверки ФИО.
    /// </summary>
    private string validationResultTwentyFirst = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentyFirst;
        set => SetProperty(ref validationResultTwentyFirst, value);
    }

    /// <summary>
    /// Загружает ФИО из API и отображает его на форме.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentyFirst = await LoadFullNameFromApiTwentyFirstAsync();
        FIO = loadedFullNameTwentyFirst;
    }

    /// <summary>
    /// Запускает проверку введенного ФИО и обновляет итог.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentyFirst(FIO);
    }

    /// <summary>
    /// Формирует текст результата проверки ФИО по двум критериям.
    /// </summary>
    private string BuildValidationMessageTwentyFirst(string fioValue)
    {
        var containsDigitTwentyFirst = HasDigitInFullNameTwentyFirst(fioValue);
        var containsSpecialCharTwentyFirst = HasSpecialSymbolInFullNameTwentyFirst(fioValue);

        if (containsDigitTwentyFirst || containsSpecialCharTwentyFirst)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Проверяет наличие цифр в строке ФИО.
    /// </summary>
    private bool HasDigitInFullNameTwentyFirst(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Проверяет наличие специальных символов !@#$%^&* в ФИО.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentyFirst(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Выполняет HTTP-запрос к API и возвращает ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentyFirstAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseTwentyFirst = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentyFirst = await apiResponseTwentyFirst.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentyFirst?.Value ?? string.Empty;
    }
}
