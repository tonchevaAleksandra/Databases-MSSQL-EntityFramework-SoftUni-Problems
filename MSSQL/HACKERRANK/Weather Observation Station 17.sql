SELECT FORMAT(ROUND(long_w, 4), 'F4')
FROM station
WHERE lat_n = (SELECT MIN(lat_n) FROM station WHERE lat_n > 38.7780)