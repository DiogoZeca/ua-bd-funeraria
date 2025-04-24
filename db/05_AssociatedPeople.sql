USE Funeraria
GO

-- Complete service team - People involved in each funeral service
SELECT 
    p.id AS process_id, 
    p.num_process,
    f.funeral_date,
    per_d.name AS deceased_name,
    u.name AS funeral_director,
    rep.contact AS representative_contact,
    p.degree_kinship AS relation_to_deceased,
    ch.name AS church_name,
    CASE 
        WHEN b.funeral_id IS NOT NULL THEN 'Burial at ' + cem.location
        WHEN c.funeral_id IS NOT NULL THEN 'Cremation at ' + crem.location
        ELSE 'Other service'
    END AS service_details
FROM project.Process p
JOIN project.Funeral f ON p.id = f.num_process 
JOIN project.Users u ON p.users_id = u.id
JOIN project.Client cl ON p.client_id = cl.id
JOIN project.Representative rep ON cl.representative_id = rep.id
JOIN project.Deceased d ON f.deceased_id = d.id
JOIN project.Person per_d ON d.id = per_d.id
LEFT JOIN project.Ceremony cer ON f.num_process = cer.funeral_id  
LEFT JOIN project.Church ch ON cer.church_id = ch.id
LEFT JOIN project.Burial b ON f.num_process = b.funeral_id  
LEFT JOIN project.Cemetery cem ON b.cemetery_id = cem.id
LEFT JOIN project.Cremation c ON f.num_process = c.funeral_id  
LEFT JOIN project.Crematory crem ON c.crematory_id = crem.id
ORDER BY f.funeral_date DESC;

-- Staff performance - Services handled per user
SELECT 
    u.id,
    u.name,
    u.username,
    COUNT(p.id) AS processes_handled,
    AVG(p.budget) AS avg_service_value,
    SUM(p.budget) AS total_revenue_handled,
    COUNT(CASE WHEN b.funeral_id IS NOT NULL THEN 1 END) AS burial_services,
    COUNT(CASE WHEN c.funeral_id IS NOT NULL THEN 1 END) AS cremation_services
FROM project.Users u
LEFT JOIN project.Process p ON u.id = p.users_id
LEFT JOIN project.Funeral f ON p.id = f.num_process  
LEFT JOIN project.Burial b ON f.num_process = b.funeral_id
LEFT JOIN project.Cremation c ON f.num_process = c.funeral_id
GROUP BY u.id, u.name, u.username
ORDER BY processes_handled DESC;

-- Kinship analysis - Relationship patterns between clients and deceased
SELECT 
    degree_kinship,
    COUNT(*) AS frequency,
    ROUND(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM project.Process), 2) AS percentage,
    AVG(budget) AS avg_service_cost,
    CASE 
        WHEN COUNT(CASE WHEN b.funeral_id IS NOT NULL THEN 1 END) > 
             COUNT(CASE WHEN c.funeral_id IS NOT NULL THEN 1 END) THEN 'Burial'
        ELSE 'Cremation' 
    END AS preferred_service_type
FROM project.Process p
JOIN project.Funeral f ON p.id = f.num_process
LEFT JOIN project.Burial b ON f.num_process = b.funeral_id
LEFT JOIN project.Cremation c ON f.num_process = c.funeral_id
GROUP BY degree_kinship
ORDER BY frequency DESC;

-- Service providers analysis - Churches, cemeteries, and crematories usage
SELECT 
    'Church' AS provider_type,
    ch.name AS provider_name,
    ch.location AS location,
    COUNT(cer.funeral_id) AS services_count
FROM project.Church ch
LEFT JOIN project.Ceremony cer ON ch.id = cer.church_id
GROUP BY ch.name, ch.location

UNION ALL

SELECT 
    'Cemetery' AS provider_type,
    cem.location AS provider_name,
    cem.location AS location,
    COUNT(b.funeral_id) AS services_count
FROM project.Cemetery cem
LEFT JOIN project.Burial b ON cem.id = b.cemetery_id
GROUP BY cem.location

UNION ALL

SELECT 
    'Crematory' AS provider_type,
    crem.location AS provider_name,
    crem.location AS location,
    COUNT(c.funeral_id) AS services_count
FROM project.Crematory crem
LEFT JOIN project.Cremation c ON crem.id = c.crematory_id
GROUP BY crem.location

ORDER BY services_count DESC;