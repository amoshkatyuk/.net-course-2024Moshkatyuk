create table client (
    id uuid primary key default gen_random_uuid(),
    first_name varchar(50) not null,
    last_name varchar(50) not null,
    passport_data varchar(11) unique not null,
    birth_date date not null
);

create table position (
    id uuid primary key default gen_random_uuid(),
    name varchar(100) unique not null
);

create table employee (
    id uuid primary key default gen_random_uuid(),
    first_name varchar(50) not null,
    last_name varchar(50) not null,
    passport_data varchar(11) unique not null,
    birth_date date not null,
    salary decimal(8,2) not null,
    position_id uuid not null,
    contract varchar(100) not null,
    foreign key (position_id) references public.position(id) on delete restrict
);

create table account (
    id uuid primary key default gen_random_uuid(),
    currency_name varchar(3) not null,
    amount decimal(15,2) not null default 0.00,
    client_id uuid not null,
    foreign key (client_id) references public.client(id) on delete cascade
);

insert into public.position (name)
values
       ('Manager'),
       ('Developer');

insert into public.client (first_name, last_name, passport_data, birth_date)
values
       ('Alex', 'Ivanov', 'AA123456789', '1990-10-30'),
       ('Alex', 'Petrov', 'AB123456789', '1991-10-30'),
       ('Alex', 'Sidorov', 'AC123456789', '1992-10-30'),
       ('Alex', 'Drozdov', 'AD123456789', '1993-10-30'),
       ('Alex', 'Skvortsov', 'AE123456789', '1994-10-30');

insert into public.employee (first_name, last_name, passport_data, birth_date, salary, position_id, contract)
values
       ('Ivan', 'Brown', 'AF987654321', '1995-10-30', 16000.00, (select id from public.position where name = 'Manager'), 'Full-time'),
       ('Ivan', 'Yellow', 'AG987654321', '1996-10-30', 16000.00, (select id from public.position where name = 'Manager'), 'Full-time'),
       ('Ivan', 'Black', 'AH987654321', '1997-10-30', 16000.00, (select id from public.position where name = 'Developer'), 'Full-time'),
       ('Ivan', 'Grey', 'AI987654321', '1998-10-30', 16000.00, (select id from public.position where name = 'Developer'), 'Full-time'),
       ('Ivan', 'White', 'AJ987654321', '1999-10-30', 16000.00, (select id from public.position where name = 'Manager'), 'Full-time');

insert into public.account (currency_name, amount, client_id)
values
       ('USD', 1000.00, (select id from public.client where passport_data = 'AA123456789')),
       ('USD', 1500.00, (select id from public.client where passport_data = 'AB123456789')),
       ('USD', 1300.00, (select id from public.client where passport_data = 'AC123456789')),
       ('USD', 1100.00, (select id from public.client where passport_data = 'AD123456789')),
       ('USD', 1900.00, (select id from public.client where passport_data = 'AE123456789'));

select c.first_name, c.last_name, a.amount
from public.client c
join public.account a on c.id = a.client_id
where a.amount < 1300.00
order by a.amount asc;

select c.first_name, c.last_name, a.amount
from public.client c
join public.account a on c.id = a.client_id
where a.amount = (
    select min(amount) 
    from public.account
)
order by a.amount asc;

select sum(a.amount) as result_amount
from public.account a;

select c.first_name, c.last_name, a.currency_name, a.amount
from public.client c
join public.account a on c.id = a.client_id;

select c.first_name, c.last_name, c.birth_date
from public.client c
order by c.birth_date asc;

select extract(year from age(c.birth_date)) as age, count(*) as count
from public.client c
group by age
order by age;

select c.first_name, c.last_name,
       extract(year from age(c.birth_date)) as age
from public.client c
group by c.first_name, c.last_name, age
order by age;

select * from public.client limit 3