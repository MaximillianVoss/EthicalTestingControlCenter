# Diploma Project MVP

Основная реализация для дипломного проекта теперь собрана на `C# / ASP.NET Core`.

Рекомендуемый для защиты и дальнейшего развития проект:
- `EthicalTestingControlCenter.Web`

Функции MVP:
- ведение контуров проверки и формальных разрешений;
- учет активов в согласованном `scope`;
- регистрация находок и рекомендаций;
- формирование сводного отчета по проверке.

Проект намеренно не содержит наступательных модулей, сканеров и эксплойтов. Он реализует безопасный управленческий и отчетный контур легитимного security-assessment.

## Стек

- `ASP.NET Core Razor Pages`
- `Entity Framework Core`
- `SQLite`
- `.NET 10`

## Запуск

```powershell
dotnet restore .\EthicalTestingControlCenter.Web\EthicalTestingControlCenter.Web.csproj
dotnet run --project .\EthicalTestingControlCenter.Web\EthicalTestingControlCenter.Web.csproj
```

После запуска приложение будет доступно по адресу, который выведет `dotnet run`.

## Проверка

```powershell
dotnet build .\EthicalTestingControlCenter.Web\EthicalTestingControlCenter.Web.csproj
```

## Пояснительная записка

Генерация черновика пояснительной записки и итогового `.docx`:

```powershell
python .\docs\generate_explanatory_note.py
```

Генератор использует шаблон `C:\Users\FossW\OneDrive\Репетиторство\Шаблон пояснительной записки.dotx`, поэтому заголовки собираются на стилях ГОСТ и поддерживают автоматическую нумерацию глав и подразделов.

Артефакты:
- `output\doc\Пояснительная_записка_дипломный_проект.docx`
- `docs\Пояснительная_записка_черновик.md`

## Примечание

Репозиторий ориентирован на финальную реализацию на `.NET`. Ранний Python-прототип в локальной рабочей папке может сохраняться как черновой этап, но в публикационный состав проекта не включается.
