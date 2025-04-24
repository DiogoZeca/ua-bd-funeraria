USE Funeraria
GO

-- Low stock alert - Products below threshold
SELECT 
    p.id,
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers - ' + f.type + ' (' + f.color + ')'
        WHEN c.id IS NOT NULL AND co.id IS NOT NULL THEN 'Coffin - ' + co.color + ' (' + c.size + ')'
        WHEN c.id IS NOT NULL AND u.id IS NOT NULL THEN 'Urn - ' + c.size
        WHEN c.id IS NOT NULL THEN 'Container - ' + c.size
        ELSE 'Other Product'
    END AS product_description,
    p.stock,
    p.price,
    CASE 
        WHEN p.stock < 3 THEN 'URGENT'
        WHEN p.stock < 5 THEN 'LOW'
        ELSE 'OK'
    END AS stock_status
FROM project.Products p
LEFT JOIN project.Container c ON p.id = c.id
LEFT JOIN project.Coffin co ON c.id = co.id
LEFT JOIN project.Urn u ON c.id = u.id
LEFT JOIN project.Flowers f ON p.id = f.id
WHERE p.stock < 10
ORDER BY p.stock ASC;

-- Product usage frequency - Most used products in funerals
SELECT 
    p.id,
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers - ' + f.type
        WHEN co.id IS NOT NULL THEN 'Coffin'
        WHEN u.id IS NOT NULL THEN 'Urn'
        WHEN c.id IS NOT NULL THEN 'Container'
        ELSE 'Other Product'
    END AS product_type,
    p.price,
    COUNT(fp.funeral_id) AS times_used,
    SUM(fp.quantity) AS total_quantity,
    SUM(p.price * fp.quantity) AS total_revenue
FROM project.Products p
LEFT JOIN project.Container c ON p.id = c.id
LEFT JOIN project.Coffin co ON c.id = co.id
LEFT JOIN project.Urn u ON c.id = u.id
LEFT JOIN project.Flowers f ON p.id = f.id
LEFT JOIN project.Funeral_Products fp ON p.id = fp.product_id
GROUP BY p.id, p.price, 
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers - ' + f.type
        WHEN co.id IS NOT NULL THEN 'Coffin'
        WHEN u.id IS NOT NULL THEN 'Urn'
        WHEN c.id IS NOT NULL THEN 'Container'
        ELSE 'Other Product'
    END
ORDER BY times_used DESC;

-- Product usage by service type (burial vs cremation)
SELECT 
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers - ' + f.type
        WHEN co.id IS NOT NULL THEN 'Coffin'
        WHEN u.id IS NOT NULL THEN 'Urn'
        WHEN c.id IS NOT NULL THEN 'Container'
        ELSE 'Other Product'
    END AS product_type,
    CASE 
        WHEN b.funeral_id IS NOT NULL THEN 'Burial'
        WHEN cr.funeral_id IS NOT NULL THEN 'Cremation'
        ELSE 'Other'
    END AS service_type,
    COUNT(*) AS frequency,
    AVG(fp.quantity) AS avg_quantity_per_service
FROM project.Funeral_Products fp
JOIN project.Products p ON fp.product_id = p.id
JOIN project.Funeral fu ON fp.funeral_id = fu.num_process
LEFT JOIN project.Burial b ON fu.num_process = b.funeral_id
LEFT JOIN project.Cremation cr ON fu.num_process = cr.funeral_id
LEFT JOIN project.Container c ON p.id = c.id
LEFT JOIN project.Coffin co ON c.id = co.id
LEFT JOIN project.Urn u ON c.id = u.id
LEFT JOIN project.Flowers f ON p.id = f.id
GROUP BY 
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers - ' + f.type
        WHEN co.id IS NOT NULL THEN 'Coffin'
        WHEN u.id IS NOT NULL THEN 'Urn'
        WHEN c.id IS NOT NULL THEN 'Container'
        ELSE 'Other Product'
    END,
    CASE 
        WHEN b.funeral_id IS NOT NULL THEN 'Burial'
        WHEN cr.funeral_id IS NOT NULL THEN 'Cremation'
        ELSE 'Other'
    END
ORDER BY service_type, frequency DESC;