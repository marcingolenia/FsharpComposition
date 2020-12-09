create database stock;
create table stockItems (
   id bigint constraint pk_stockItems primary key,
   data json not null
);
