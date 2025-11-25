-- Включаем расширение для UUID (если еще не включено)
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Очистка таблиц (если нужно)
TRUNCATE TABLE "Histories" CASCADE;
TRUNCATE TABLE "Users" CASCADE;

-- Заполнение таблицы Users (Пользователи) - 5 записей
INSERT INTO "Users" ("Id", "FullName", "Email", "Password")
VALUES
    (gen_random_uuid(), 'Александр Васильев', 'alex.vasiliev@example.com', 'hashed_password_1'),
    (gen_random_uuid(), 'Мария Петрова', 'maria.petrova@example.com', 'hashed_password_2'),
    (gen_random_uuid(), 'Андрей Соколов', 'andrey.sokolov@example.com', 'hashed_password_3'),
    (gen_random_uuid(), 'Екатерина Иванова', 'ekaterina.ivanova@example.com', 'hashed_password_4'),
    (gen_random_uuid(), 'Денис Кузнецов', 'denis.kuznetsov@example.com', 'hashed_password_5');

-- Заполнение таблицы Histories (Истории игр) - 30 записей
DO $$
DECLARE
    user1 UUID;
    user2 UUID;
    user3 UUID;
    user4 UUID;
    user5 UUID;
    base_date TIMESTAMP := CURRENT_TIMESTAMP - INTERVAL '30 days';
BEGIN
    -- Получаем ID пользователей
    SELECT "Id" INTO user1 FROM "Users" WHERE "Email" = 'alex.vasiliev@example.com';
    SELECT "Id" INTO user2 FROM "Users" WHERE "Email" = 'maria.petrova@example.com';
    SELECT "Id" INTO user3 FROM "Users" WHERE "Email" = 'andrey.sokolov@example.com';
    SELECT "Id" INTO user4 FROM "Users" WHERE "Email" = 'ekaterina.ivanova@example.com';
    SELECT "Id" INTO user5 FROM "Users" WHERE "Email" = 'denis.kuznetsov@example.com';

    -- Вставляем 30 записей истории
    INSERT INTO "Histories" ("Id", "Score", "DateTime", "UserId")
    VALUES
        -- Александр Васильев (6 записей)
        (gen_random_uuid(), 1250, base_date + INTERVAL '1 day', user1),
        (gen_random_uuid(), 890, base_date + INTERVAL '2 days', user1),
        (gen_random_uuid(), 2100, base_date + INTERVAL '5 days', user1),
        (gen_random_uuid(), 1560, base_date + INTERVAL '8 days', user1),
        (gen_random_uuid(), 980, base_date + INTERVAL '12 days', user1),
        (gen_random_uuid(), 1750, base_date + INTERVAL '15 days', user1),
        
        -- Мария Петрова (6 записей)
        (gen_random_uuid(), 1420, base_date + INTERVAL '1 day', user2),
        (gen_random_uuid(), 1950, base_date + INTERVAL '3 days', user2),
        (gen_random_uuid(), 1100, base_date + INTERVAL '6 days', user2),
        (gen_random_uuid(), 2250, base_date + INTERVAL '9 days', user2),
        (gen_random_uuid(), 1680, base_date + INTERVAL '14 days', user2),
        (gen_random_uuid(), 1320, base_date + INTERVAL '18 days', user2),
        
        -- Андрей Соколов (6 записей)
        (gen_random_uuid(), 890, base_date + INTERVAL '2 days', user3),
        (gen_random_uuid(), 1450, base_date + INTERVAL '4 days', user3),
        (gen_random_uuid(), 1780, base_date + INTERVAL '7 days', user3),
        (gen_random_uuid(), 990, base_date + INTERVAL '10 days', user3),
        (gen_random_uuid(), 2050, base_date + INTERVAL '13 days', user3),
        (gen_random_uuid(), 1120, base_date + INTERVAL '16 days', user3),
        
        -- Екатерина Иванова (6 записей)
        (gen_random_uuid(), 1650, base_date + INTERVAL '1 day', user4),
        (gen_random_uuid(), 1280, base_date + INTERVAL '5 days', user4),
        (gen_random_uuid(), 1920, base_date + INTERVAL '8 days', user4),
        (gen_random_uuid(), 1450, base_date + INTERVAL '11 days', user4),
        (gen_random_uuid(), 2180, base_date + INTERVAL '15 days', user4),
        (gen_random_uuid(), 1050, base_date + INTERVAL '20 days', user4),
        
        -- Денис Кузнецов (6 записей)
        (gen_random_uuid(), 1180, base_date + INTERVAL '3 days', user5),
        (gen_random_uuid(), 1850, base_date + INTERVAL '6 days', user5),
        (gen_random_uuid(), 920, base_date + INTERVAL '9 days', user5),
        (gen_random_uuid(), 1550, base_date + INTERVAL '12 days', user5),
        (gen_random_uuid(), 1980, base_date + INTERVAL '17 days', user5),
        (gen_random_uuid(), 1350, base_date + INTERVAL '22 days', user5);

END $$;

-- Проверяем результат
SELECT 'Users: ' || COUNT(*)::TEXT || ' записей' FROM "Users"
UNION ALL
SELECT 'Histories: ' || COUNT(*)::TEXT || ' записей' FROM "Histories";