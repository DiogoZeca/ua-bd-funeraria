-- INDICES PARA ACELERAR JOINS e WHERES
CREATE INDEX idx_funeral_num_process ON Funeral(num_process);
CREATE INDEX idx_process_num_process ON Process(num_process);
CREATE INDEX idx_funeral_church_id ON Funeral(chucrh_id);
CREATE INDEX idx_funeral_deceased_bi ON Funeral(deceased_bi);
CREATE INDEX idx_burial_funeral_id ON Burial(funeral_id);
CREATE INDEX idx_creametion_funeral_id ON Cremation(funeral_id);
CREATE INDEX idx_flowers_process_num ON Flowers(process_num);



-- INDICES PARA ACELERAR COLUNAS USADAS MAIS FREQUENTEMENTE
CREATE INDEX idx_users_username ON Users(username);
CREATE INDEX idx_users_email ON Users(email);
CREATE INDEX idx_users_id ON Users(id);
CREATE INDEX idx_person_bi ON Person(bi);



-- INDICES PARA ANALISES
CREATE INDEX idx_process_start_date ON Process(start_date);
CREATE INDEX idx_products_stock ON Products(stock);
CREATE INDEX idx_process_type_of_payment ON Process(type_of_payment);
CREATE INDEX idx_funeral_funeral_date ON Funeral(funeral_date);



-- INDICES COMPOSTOS
CREATE INDEX idx_person_bi_name ON Person(bi, name);
CREATE INDEX idx_funeral_church__deceased ON Funeral(church_id, deceased_bi);
CREATE INDEX idx_produts_price_stock ON Products(price, stock);



-- INDICES UNICOS
CREATE UNIQUE INDEX idx_users_unique_username ON Users(username);
CREATE UNIQUE INDEX idx_users_unique_email ON Users(email);