# kval_variants

Этот репозиторий содержит 30 уникальных вариаций проекта Avalonia/.NET 8 для задания по валидации данных.

## Структура

- `original_project/DEMO` — сохранённый исходный базовый проект.
- `varieties/1..30` — варианты, каждый содержит `DEMO/` и `DEMO.zip`.

## Как выбрать вариант

Выберите номер варианта из таблицы и перейдите в папку `varieties/<номер>/`.

| Вариант | Стиль HttpClient | Сдвиг using-порядка | Базовое имя поля ФИО | Базовое имя helper-метода |
| --- | --- | --- | --- | --- |
| 1 | instance field | 1 | `_fullNameText` | `LoadFullNameFromApi` |
| 2 | static field | 2 | `_personFullNameText` | `RequestFullNameFromApi` |
| 3 | method-local | 3 | `_loadedFullNameText` | `FetchFullNameFromService` |
| 4 | instance field | 4 | `_receivedFullNameText` | `GetFullNameFromSimulator` |
| 5 | static field | 5 | `_candidateFullNameText` | `ReadFullNameFromApi` |
| 6 | method-local | 6 | `_currentFullNameText` | `LoadFullNameValue` |
| 7 | instance field | 7 | `_simulatorFullNameText` | `RequestNameValue` |
| 8 | static field | 8 | `_screenFullNameText` | `FetchNameText` |
| 9 | method-local | 1 | `_outputFullNameText` | `GetRemoteFullName` |
| 10 | instance field | 2 | `_viewFullNameText` | `ReadRemoteFullName` |
| 11 | static field | 3 | `_valueFullNameText` | `LoadApiFullName` |
| 12 | method-local | 4 | `_requestFullNameText` | `RequestApiFullName` |
| 13 | instance field | 5 | `_responseFullNameText` | `FetchApiFullName` |
| 14 | static field | 6 | `_fetchedFullNameText` | `GetApiFullName` |
| 15 | method-local | 7 | `_validatedFullNameText` | `ReadApiFullName` |
| 16 | instance field | 8 | `_rawFullNameText` | `LoadSimulatorFullName` |
| 17 | static field | 1 | `_apiFullNameText` | `RequestSimulatorFullName` |
| 18 | method-local | 2 | `_displayFullNameText` | `FetchSimulatorFullName` |
| 19 | instance field | 3 | `_uiFullNameText` | `GetSimulatorFullName` |
| 20 | static field | 4 | `_savedFullNameText` | `ReadSimulatorFullName` |
| 21 | method-local | 5 | `_cachedFullNameText` | `LoadPersonNameFromApi` |
| 22 | instance field | 6 | `_resolvedFullNameText` | `RequestPersonNameFromApi` |
| 23 | static field | 7 | `_userFullNameText` | `FetchPersonNameFromApi` |
| 24 | method-local | 8 | `_activeFullNameText` | `GetPersonNameFromApi` |
| 25 | instance field | 1 | `_receivedNameText` | `ReadPersonNameFromApi` |
| 26 | static field | 2 | `_profileFullNameText` | `LoadResponseName` |
| 27 | method-local | 3 | `_recordFullNameText` | `RequestResponseName` |
| 28 | instance field | 4 | `_detailFullNameText` | `FetchResponseName` |
| 29 | static field | 5 | `_packetFullNameText` | `GetResponseName` |
| 30 | method-local | 6 | `_entryFullNameText` | `ReadResponseName` |

## Общие инварианты для всех 30 вариантов

- Заголовок окна: `Валидация данных`.
- Кнопка 1: `Получить данные`.
- Кнопка 2: `Отправить результат теста`.
- Структура MVVM: `MainWindowViewModel`, `MainWindow`, `Response`.
- Endpoint API: `http://89.125.39.39:8080/TransferSimulator/fullName`.
- Валидация по 2 критериям: наличие цифр и наличие символов из `!@#$%^&*`.
- Целевая платформа: `.NET 8`, прежний набор NuGet-пакетов.

## Команды сборки и запуска (как запрошено)

```bash
dotnet publish DEMO/DEMO.csproj -r linux-x64 --self-contained -p:PublishSingleFile=true -o ./dist
cd dist && ./DEMO
```
