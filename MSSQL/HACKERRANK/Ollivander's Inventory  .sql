USE master

CREATE DATABASE OLIVANDER
USE OLIVANDER

CREATE TABLE WANDS_PROPERTY
(
    CODE    INT PRIMARY KEY IDENTITY,
    AGE     INT,
    IS_EVIL BIT
)

CREATE TABLE WANDS
(
    ID           INT PRIMARY KEY IDENTITY,
    CODE         INT NOT NULL REFERENCES WANDS_PROPERTY (CODE),
    COINS_NEEDED INT NOT NULL,
    POWER        INT NOT NULL
)

INSERT INTO WANDS_PROPERTY(AGE, IS_EVIL)
VALUES (45, 0),
       (40, 0),
       (4, 1),
       (20, 0),
       (17, 0)

INSERT INTO WANDS(CODE, COINS_NEEDED, POWER)
VALUES (4, 3688, 8),
       (3, 9365, 3),
       (3, 7187, 10),
       (3, 734, 8),
       (1, 6020, 2),
       (2, 6773, 7),
       (3, 9873, 9),
       (3, 7721, 7),
       (1, 1647, 10),
       (4, 504, 5),
       (2, 7587, 5),
       (5, 9897, 10),
       (3, 4651, 8),
       (2, 5408, 1),
       (2, 6018, 7),
       (4, 7710, 5),
       (2, 8798, 7),
       (2, 3312, 3),
       (4, 7651, 6),
       (5, 5689, 3)

SELECT W.ID, WP.AGE, W.COINS_NEEDED, W.POWER
FROM WANDS AS W
         JOIN WANDS_PROPERTY WP ON W.CODE = WP.CODE
WHERE WP.IS_EVIL = 0
  AND W.COINS_NEEDED = (SELECT MIN(COINS_NEEDED)
                        FROM WANDS AS W1
                                 JOIN WANDS_PROPERTY P ON P.CODE = W1.CODE
                        WHERE W.POWER = W1.POWER
                          AND WP.AGE = P.AGE)

ORDER BY W.POWER DESC, WP.AGE DESC


