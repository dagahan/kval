# Kval Variants

Этот репозиторий содержит исходный проект Avalonia/.NET 8 и 30 его кодовых вариаций для тренировки и демонстрации альтернативных реализаций одной и той же логики.

## Структура

- `original_project/DEMO` — сохраненный исходный вариант проекта.
- `varieties/<номер>/DEMO` — конкретная вариация проекта.
- `varieties/<номер>/DEMO.zip` — архив соответствующей вариации.

## Как выбрать вариацию

Выберите номер из таблицы https://1drv.ms/x/c/2de8c79582d17d97/IQBVthF8_AF3SKWoEHB9CvTUAQkKGSCKE32GzKt7eOXWr2s?e=uEysQl и перейдите в папку `varieties/<номер>/DEMO`.

## Сборка и запуск

```bash
dotnet publish DEMO/DEMO.csproj -r linux-x64 --self-contained -p:PublishSingleFile=true -o ./dist
```

```bash
cd dist && ./DEMO
```
