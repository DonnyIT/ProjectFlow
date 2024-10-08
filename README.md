# ProjectFlow

ProjectFlow - це веб-додаток для управління проектами та завданнями, створений з використанням ASP.NET Core MVC, Identity та Entity Framework.

## Можливості

- Управління проектами та завданнями
- Реєстрація та аутентифікація користувачів
- Призначення ролей користувачам (Admin, Project Manager, Team Member)
- Захист даних та управління доступом
- Відображення проектів та завдань, які потрібно виконати сьогодні

## Встановлення

1. Клонувати репозиторій:

    ```bash
    git clone https://github.com/yourusername/ProjectFlow.git
    ```

2. Перейти до директорії проекту:

    ```bash
    cd ProjectFlow
    ```

3. Встановити залежності:

    ```bash
    dotnet restore
    ```

4. Налаштувати базу даних:

    - Відредагувати файл `appsettings.json`, щоб вказати правильний рядок підключення до бази даних.
    - Виконати міграції для створення бази даних:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5. Запустити додаток:

    ```bash
    dotnet run
    ```

## Використання

### Реєстрація та вхід

- Користувачі можуть зареєструватися та увійти в систему.
- Під час реєстрації користувач може вибрати свою роль (Project Manager або Team Member).

### Управління проектами

- Користувачі з ролями "Admin" та "Project Manager" можуть створювати, редагувати та видаляти проекти.
- Кожен проект може мати кілька завдань.

### Управління завданнями

- Користувачі з ролями "Admin" та "Project Manager" можуть створювати, редагувати та видаляти завдання.
- Завдання можуть бути призначені користувачам та мати терміни виконання.

### Головна сторінка

- Головна сторінка відображає проекти та завдання, які потрібно виконати сьогодні.

## Структура проекту

- **Controllers**: Містить контролери для обробки запитів.
- **Models**: Містить моделі даних.
- **Views**: Містить представлення для відображення даних.
- **Data**: Містить контекст бази даних та клас для ініціалізації даних.

## Внесок

Якщо ви хочете внести свій внесок у проект, будь ласка, створіть форк репозиторію, внесіть зміни та надішліть pull request. Ми будемо раді вашим пропозиціям та покращенням!

## Ліцензія

Цей проект ліцензовано під ліцензією MIT. Дивіться файл LICENSE для отримання додаткової інформації.
