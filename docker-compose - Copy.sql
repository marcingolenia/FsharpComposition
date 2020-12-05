create database hotel
create table rooms (
   id bigint constraint pk_rooms primary key,
   data json not null
);
