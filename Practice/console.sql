create table сlient (
    id uuid primary key default gen_random_uuid(),
    first_name varchar(50) not null,
    last_name varchar(50) not null,
    passport_data varchar(11) unique not null,
    birth_date date not null
);

create table employee (
    id uuid primary key default gen_random_uuid(),
    first_name varchar(50) not null,
    last_name varchar(50) not null,
    passport_data varchar(11) unique not null,
    birth_date date not null,
    salary decimal(8,2) not null,
    position varchar(100) not null,
    contract varchar(100) not null
);

create table account (
    id uuid primary key default gen_random_uuid(),
    currency_name varchar(3) not null,
    amount decimal(15,2) not null default 0.00,
    client_id uuid not null,
    foreign key (client_id) references public.сlient(id) on delete cascade
);

insert into public.сlient (first_name, last_name, passport_data, birth_date)
values
       ('Alex', 'Ivanov', 'AA123456789', '1990-10-30'),
       ('Alex', 'Petrov', 'AB123456789', '1991-10-30'),
       ('Alex', 'Sidorov', 'AC123456789', '1992-10-30'),
       ('Alex', 'Drozdov', 'AD123456789', '1993-10-30'),
       ('Alex', 'Skvortsov', 'AE123456789', '1994-10-30');

insert into public.employee (first_name, last_name, passport_data, birth_date, salary, position, contract)
values
       ('Ivan', 'Brown', 'AF987654321', '1995-10-30', 16000.00, 'Manager', 'Full-time'),
       ('Ivan', 'Yellow', 'AG987654321', '1996-10-30', 16000.00, 'Manager', 'Full-time'),
       ('Ivan', 'Black', 'AH987654321', '1997-10-30', 16000.00, 'Manager', 'Full-time'),
       ('Ivan', 'Grey', 'AI987654321', '1998-10-30', 16000.00, 'Manager', 'Full-time'),
       ('Ivan', 'White', 'AJ987654321', '1999-10-30', 16000.00, 'Manager', 'Full-time');

insert into public.account (currency_name, amount, client_id)
values
       ('USD', 1000.00, 'a8a98117-2dfe-4e2a-8013-e594d69967cd'),
       ('USD', 1500.00, '8b6430a2-efa6-4a07-9c21-76b5121a5dfd'),
       ('USD', 1300.00, 'edca9bd9-5cd6-4d19-acf5-2d5630b0b089'),
       ('USD', 1100.00, 'e9521b72-acd8-4cb2-9c3b-8da63a21c469'),
       ('USD', 1900.00, '1260fec7-e62e-4b32-a7fc-05ca01223147');

select c.first_name, c.last_name, a.amount
from public.сlient c
join public.account a on c.id = a.client_id
where a.amount < 1300.00
order by a.amount asc;

select c.first_name, c.last_name, min(a.amount) as min_amount
from public.сlient c
join public.account a on c.id = a.client_id
group by c.id
order by min_amount asc
limit 1;

select sum(a.amount) as result_amount
from public.account a;

select c.first_name, c.last_name, a.currency_name, a.amount
from public.сlient c
join public.account a on c.id = a.client_id;

select c.first_name, c.last_name, c.birth_date
from public.сlient c
order by c.birth_date asc;

select extract(year from age(c.birth_date)) as age, count(*) as count
from public.сlient c
group by age
order by age;

select c.first_name, c.last_name,
       extract(year from age(c.birth_date)) as age
from public.сlient c
order by age;

select * from public.сlient limit 3