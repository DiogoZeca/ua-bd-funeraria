USE Funeraria
GO

-- Total revenue by month/year
SELECT 
    YEAR(p.start_date) AS year,
    MONTH(p.start_date) AS month,
    COUNT(p.id) AS total_services,
    SUM(p.budget) AS total_revenue,
    AVG(p.budget) AS average_service_value
FROM project.Process p
GROUP BY YEAR(p.start_date), MONTH(p.start_date)
ORDER BY year DESC, month DESC;

-- Revenue by service type (burial vs cremation)
SELECT 
    CASE 
        WHEN b.funeral_id IS NOT NULL THEN 'Burial'
        WHEN c.funeral_id IS NOT NULL THEN 'Cremation'
        ELSE 'Other'
    END AS service_type,
    COUNT(p.id) AS service_count,
    SUM(p.budget) AS total_revenue,
    AVG(p.budget) AS avg_service_cost,
    MIN(p.budget) AS min_service_cost,
    MAX(p.budget) AS max_service_cost
FROM project.Process p
JOIN project.Funeral f ON p.id = f.num_process
LEFT JOIN project.Burial b ON f.num_process = b.funeral_id
LEFT JOIN project.Cremation c ON f.num_process = c.funeral_id
GROUP BY 
    CASE 
        WHEN b.funeral_id IS NOT NULL THEN 'Burial'
        WHEN c.funeral_id IS NOT NULL THEN 'Cremation'
        ELSE 'Other'
    END
ORDER BY total_revenue DESC;

-- Product contribution to revenue
SELECT 
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers'
        WHEN co.id IS NOT NULL THEN 'Coffin'
        WHEN u.id IS NOT NULL THEN 'Urn'
        WHEN c.id IS NOT NULL AND co.id IS NULL AND u.id IS NULL THEN 'Container'
        ELSE 'Other Product'
    END AS product_category,
    COUNT(DISTINCT p.id) AS unique_products,
    SUM(fp.quantity) AS total_quantity_used,
    SUM(p.price * fp.quantity) AS total_revenue_contribution,
    ROUND(SUM(p.price * fp.quantity) / 
        (SELECT SUM(price * quantity) FROM project.Products pr 
         JOIN project.Funeral_Products fpr ON pr.id = fpr.product_id) * 100, 2) AS percent_of_product_revenue
FROM project.Products p
LEFT JOIN project.Container c ON p.id = c.id
LEFT JOIN project.Coffin co ON c.id = co.id
LEFT JOIN project.Urn u ON c.id = u.id
LEFT JOIN project.Flowers f ON p.id = f.id
JOIN project.Funeral_Products fp ON p.id = fp.product_id
GROUP BY 
    CASE 
        WHEN f.id IS NOT NULL THEN 'Flowers'
        WHEN co.id IS NOT NULL THEN 'Coffin'
        WHEN u.id IS NOT NULL THEN 'Urn'
        WHEN c.id IS NOT NULL AND co.id IS NULL AND u.id IS NULL THEN 'Container'
        ELSE 'Other Product'
    END
ORDER BY total_revenue_contribution DESC;

-- Payment methods analysis
SELECT 
    type_of_payment,
    COUNT(*) AS number_of_services,
    SUM(budget) AS total_revenue,
    AVG(budget) AS average_service_cost
FROM project.Process
GROUP BY type_of_payment
ORDER BY total_revenue DESC;