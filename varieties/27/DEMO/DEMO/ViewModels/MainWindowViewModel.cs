using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Логика взаимодействия интерфейса и правил валидации ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// ФИО клиента, отображаемое в интерфейсе.
    /// </summary>
    private string fullNameTwentySeventh = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentySeventh;
        set => SetProperty(ref fullNameTwentySeventh, value);
    }

    /// <summary>
    /// Текстовый итог текущей валидации.
    /// </summary>
    private string validationResultTwentySeventh = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentySeventh;
        set => SetProperty(ref validationResultTwentySeventh, value);
    }

    /// <summary>
    /// Выполняет запрос к сервису и обновляет значение ФИО.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentySeventh = await LoadFullNameFromApiTwentySeventhAsync();
        FIO = loadedFullNameTwentySeventh;
    }

    /// <summary>
    /// Выполняет валидацию текущего ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentySeventh(FIO);
    }

    /// <summary>
    /// Выполняет проверку строки ФИО и возвращает итоговое сообщение.
    /// </summary>
    private string BuildValidationMessageTwentySeventh(string fioValue)
    {
        var containsDigitTwentySeventh = HasDigitInFullNameTwentySeventh(fioValue);
        var containsSpecialCharTwentySeventh = HasSpecialSymbolInFullNameTwentySeventh(fioValue);

        if (containsDigitTwentySeventh || containsSpecialCharTwentySeventh)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Определяет, есть ли в ФИО числовые символы.
    /// </summary>
    private bool HasDigitInFullNameTwentySeventh(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Определяет, есть ли в ФИО символы из набора !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentySeventh(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Получает ФИО из удаленного эмулятора по заданному URL.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentySeventhAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseTwentySeventh = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentySeventh = await apiResponseTwentySeventh.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentySeventh?.Value ?? string.Empty;
    }
}
