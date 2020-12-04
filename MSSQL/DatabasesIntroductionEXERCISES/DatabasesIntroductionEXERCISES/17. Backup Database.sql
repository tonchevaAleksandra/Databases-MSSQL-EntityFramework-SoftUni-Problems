BACKUP DATABASE SoftUni
TO DISK = 'F:\Downloads\Softuni-backup.bak'

USE master

DROP DATABASE SoftUni

RESTORE DATABASE SoftUni 
FROM DISK = 'F:\Downloads\Softuni-backup.bak'