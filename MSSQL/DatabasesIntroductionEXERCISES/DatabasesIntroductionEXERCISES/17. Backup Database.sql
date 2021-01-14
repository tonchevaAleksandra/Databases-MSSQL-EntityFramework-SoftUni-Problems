BACKUP DATABASE SoftUni1
TO DISK = 'F:\Downloads\Softuni-backup.bak'

USE master

DROP DATABASE SoftUni1

RESTORE DATABASE SoftUni1
FROM DISK = 'F:\Downloads\Softuni-backup.bak'