-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS cockroachdb_crud_db;

-- Set the active database
USE cockroachdb_crud_db;

-- Drop existing table and sequence if they exist
DROP TABLE IF EXISTS products;
DROP SEQUENCE IF EXISTS products_id_seq;

-- Create a new sequence starting from 1
CREATE SEQUENCE products_id_seq
    START WITH 1
    INCREMENT BY 1;

-- Create products table
CREATE TABLE IF NOT EXISTS products (
    id BIGINT PRIMARY KEY DEFAULT nextval('products_id_seq'),
    name STRING NOT NULL,
    price DECIMAL(18,2) NOT NULL
);

-- Create some sample data
INSERT INTO products (name, price) VALUES 
    ('Laptop', 999.99),
    ('Smartphone', 699.99),
    ('Headphones', 199.99),
    ('Tablet', 449.99),
    ('Smartwatch', 299.99);