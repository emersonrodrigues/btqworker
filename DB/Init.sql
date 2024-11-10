--CREATE DATABASE BTG;

CREATE TABLE products (
    id BIGSERIAL PRIMARY KEY,
	description  VARCHAR(100),
	stock int,
	price NUMERIC(2) 
);
CREATE TABLE orders  (
    id bigint PRIMARY KEY,
		customer_id int not null
);


CREATE TABLE orderitems   (
    id BIGSERIAL PRIMARY KEY,
		order_id int not null ,
		product_id int not null ,
		amount int not null,
		price  NUMERIC(2) not null,

	   CONSTRAINT fk_order
      FOREIGN KEY(order_id)
        REFERENCES orders(id),	
	CONSTRAINT fk_product
      FOREIGN KEY(product_id)
        REFERENCES products(id)	
);
