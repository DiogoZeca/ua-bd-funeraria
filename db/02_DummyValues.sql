USE Funeraria
GO

-- USERS
INSERT INTO project.Users (id, name, username, password, mail, num_func) VALUES
(1, 'João Fernandes', 'jfernandes', 'pass123', 'joao@funeraria.pt', 101),
(2, 'Rita Alves', 'ralves', '123rita', 'rita@funeraria.pt', 102);

-- REPRESENTATIVES
INSERT INTO project.Representative (id, contact) VALUES
(1, '912345678'),
(2, '934567890');

-- CLIENTS
INSERT INTO project.Client (id, representative_id) VALUES
(1, 1),
(2, 2);

-- PERSONS
INSERT INTO project.Person (id, name, bi) VALUES
(1, 'José Silva', '123456789'),
(2, 'Helena Martins', '987654321');

-- DECEASED
INSERT INTO project.Deceased (id, sex, birth_date, marital_estate, residence, nationality) VALUES
(1, 'M', '1950-06-12', 'Casado', 'Lisboa', 'Português'),
(2, 'F', '1945-03-20', 'Viúva', 'Porto', 'Portuguesa');

-- PROCESS
INSERT INTO project.Process (id, num_process, start_date, status, budget, description, type_of_payment, users_id, client_id, degree_kinship) VALUES
(1, 1001, '2025-04-01', 'Em Curso', 2000.00, 'Serviço com enterro tradicional', 'Dinheiro', 1, 1, 'Filho'),
(2, 1002, '2025-04-02', 'Concluído', 1500.00, 'Serviço de cremação simples', 'Transferência', 2, 2, 'Esposa');

-- FUNERAL
INSERT INTO project.Funeral (num_process, funeral_date, location, deceased_id, client_id) VALUES
(1, '2025-04-03', 'Cemitério de Lisboa', 1, 1),
(2, '2025-04-04', 'Crematério do Porto', 2, 2);

-- CEMETERY
INSERT INTO project.Cemetery (id, location, price) VALUES
(1, 'Cemitério de Lisboa', 300.00),
(2, 'Cemitério do Porto', 350.00);

-- CREMATORY
INSERT INTO project.Crematory (id, location, price) VALUES
(1, 'Crematório do Porto', 500.00);

-- PRIEST
INSERT INTO project.Priest (id, price) VALUES
(1, 150.00);

-- CHURCH
INSERT INTO project.Church (id, name, priest_id, location) VALUES
(1, 'Igreja de Santo António', 1, 'Lisboa');

-- CEREMONY
INSERT INTO project.Ceremony (funeral_id, church_id) VALUES
(1, 1);

-- PRODUCTS
INSERT INTO project.Products (id, price, stock) VALUES
(1, 75.00, 10),  -- Flores
(2, 300.00, 5),  -- Container
(3, 400.00, 2),  -- Caixão
(4, 150.00, 3);  -- Urna

-- CONTAINER
INSERT INTO project.Container (id, supplier, size) VALUES
(2, 'Fornecedor A', 'Grande'),
(3, 'Madeiras Pedro', 'Extra Grande'),
(4, 'Cerâmica Porto', 'Pequena');

-- COFFIN
INSERT INTO project.Coffin (id, color, weight) VALUES
(3, 'Castanho', 35.5);

-- URN
INSERT INTO project.Urn (id) VALUES
(4);

-- FLOWERS
INSERT INTO project.Flowers (id, type, color) VALUES
(1, 'Rosas', 'Vermelho');

-- FLORIST
INSERT INTO project.Florist (id, name, address, contact, nif) VALUES
(1, 'Florista Bela Flor', 'Rua das Flores, Braga', '912345000', '509123456');

-- FLORIST_FLOWERS
INSERT INTO project.Florist_Flowers (florist_id, flower_id) VALUES
(1, 1);

-- FUNERAL_PRODUCTS
INSERT INTO project.Funeral_Products (funeral_id, product_id, quantity) VALUES
(1, 1, 3),  -- Flores
(1, 3, 1),  -- Caixão
(2, 4, 1);  -- Urna

-- BURIAL
INSERT INTO project.Burial (funeral_id, cemetery_id, num_grave) VALUES
(1, 1, 101);

-- CREMATION
INSERT INTO project.Cremation (funeral_id, crematory_id) VALUES
(2, 1);