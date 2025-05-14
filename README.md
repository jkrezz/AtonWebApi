# AtonWebApi — это API для аутентификации и управления пользователями

## Для запуска необходимо:
1. Клонировать репозиторий и запустить проект
```
git clone https://github.com/jkrezz/AtonWebApi.git
```

2. Запустить через Docker
```
1) В терминале, находясь в директории проекта, собрать сервис: docker-compose build
2) docker-compose up
3) Перейти по http://localhost:9090/swagger/index.html для просмотра документации API.
```

## Для доступа к защищённым эндпоинтам необходимо получить JWT-токен. (POST /api/Auth/login)
```json
{
  "login": "admin",
  "password": "admin"
}

```
### Успешный ответ (200 OK):
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

## Эндпоинты API
- **POST** `/api/Auth/login` — Аутентификация пользователя и получение JWT-токена.
- **POST** `/api/Users` — Создание нового пользователя (Admin).
- **PUT** `/api/Users/{login}/profile` — Обновление профиля пользователя (имя, пол, дата рождения).
- **PUT** `/api/Users/{login}/password` — Обновление пароля пользователя.
- **PUT** `/api/Users/{login}/login` — Обновление логина пользователя.
- **GET** `/api/Users` — Получение всех активных пользователей (Admin).
- **GET** `/api/Users/{login}` — Получение данных пользователя по логину (Admin).
- **GET** `/api/Users/older-than/{age}` — Получение пользователей старше указанного возраста (Admin).
- **DELETE** `/api/Users/{login}` — Мягкое или полное удаление пользователя (Admin).
- **PUT** `/api/Users/{login}/restore` — Восстановление ранее мягко удалённого пользователя (Admin).
